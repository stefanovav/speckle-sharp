# nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Speckle.Core.Api.GraphQL.Serializer;
using Speckle.Core.Credentials;
using Speckle.Core.Helpers;
using Speckle.Core.Logging;
using Speckle.Newtonsoft.Json;

namespace Speckle.Core.Api;

public sealed partial class Client : IDisposable
{
  internal Client() { } //TODO: Not sure if this is needed for serialization?

  public Client(Account account)
  {
    if (account == null)
      throw new ArgumentNullException(nameof(account));

    Account = account;

    _httpClient = Http.GetHttpProxyClient();

    if (account.token.ToLowerInvariant().Contains("bearer"))
      _httpClient.DefaultRequestHeaders.Add("Authorization", account.token);
    else
      _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {account.token}");

    _httpClient.DefaultRequestHeaders.Add("apollographql-client-name", Setup.HostApplication);
    _httpClient.DefaultRequestHeaders.Add(
      "apollographql-client-version",
      Assembly.GetExecutingAssembly().GetName().Version.ToString()
    );

    _gqlClient = new GraphQLHttpClient(
      new GraphQLHttpClientOptions
      {
        EndPoint = new Uri(new Uri(account.serverInfo.url), "/graphql"),
        UseWebSocketForQueriesAndMutations = false,
        ConfigureWebSocketConnectionInitPayload = opts =>
        {
          return new { Authorization = $"Bearer {account.token}" };
        },
        OnWebsocketConnected = OnWebSocketConnect
      },
      new NewtonsoftJsonSerializer(),
      _httpClient
    );

    _gqlClient.WebSocketReceiveErrors.Subscribe(e =>
    {
      if (e is WebSocketException we)
        Console.WriteLine(
          $"WebSocketException: {we.Message} (WebSocketError {we.WebSocketErrorCode}, ErrorCode {we.ErrorCode}, NativeErrorCode {we.NativeErrorCode}"
        );
      else
        Console.WriteLine($"Exception in websocket receive stream: {e}");
    });
  }

  public string ServerUrl => Account.serverInfo.url;

  public string ApiToken => Account.token;

  public System.Version ServerVersion { get; set; }

  [JsonIgnore]
  public Account Account { get; }

  private readonly HttpClient _httpClient;
  private readonly GraphQLHttpClient _gqlClient;

  private bool isDisposed;

  public void Dispose()
  {
    if (isDisposed)
      return;

    _userStreamAddedSubscription?.Dispose();
    _userStreamRemovedSubscription?.Dispose();
    _streamUpdatedSubscription?.Dispose();
    _branchCreatedSubscription?.Dispose();
    _branchUpdatedSubscription?.Dispose();
    _branchDeletedSubscription?.Dispose();
    _commitCreatedSubscription?.Dispose();
    _commitUpdatedSubscription?.Dispose();
    _commitDeletedSubscription?.Dispose();
    _commentActivitySubscription?.Dispose();

    _httpClient.Dispose();

    int counter = 0;
    while (counter < 200)
    {
      try
      {
        counter++;
        _gqlClient.Dispose();
        break;
      }
      catch (Exception ex)
      {
        SpeckleLog.Logger.Error(ex, "Exception while disposing {disposable}", nameof(GraphQLHttpClient));
      }
    }
    
    isDisposed = true;
  }

  public Task OnWebSocketConnect(GraphQLHttpClient client)
  {
    return Task.CompletedTask;
  }

  internal async Task<T> ExecuteWithResiliencePolicies<T>(Func<Task<T>> func)
  {
    // TODO: handle these in the HttpClient factory with a custom RequestHandler class
    // 408 Request Timeout
    // 425 Too Early
    // 429 Too Many Requests
    // 500 Internal Server Error
    // 502 Bad Gateway
    // 503 Service Unavailable
    // 504 Gateway Timeout

    var delay = Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5);
    var graphqlRetry = Policy
      .Handle<SpeckleGraphQLInternalErrorException<T>>()
      .WaitAndRetryAsync(
        delay,
        (ex, timeout, context) =>
        {
          var graphqlEx = ex as SpeckleGraphQLException<T>;
          SpeckleLog.Logger
            .ForContext("graphqlExtensions", graphqlEx.Extensions)
            .ForContext("graphqlErrorMessages", graphqlEx.ErrorMessages)
            .Warning(
              ex,
              "The previous attempt at executing function to get {resultType} failed with {exceptionMessage}. Retrying after {timeout}.",
              typeof(T).Name,
              ex.Message,
              timeout
            );
        }
      );

    return await graphqlRetry.ExecuteAsync(func).ConfigureAwait(false);
  }

  internal async Task<T> ExecuteGraphQLRequest<T>(GraphQLRequest request, CancellationToken cancellationToken = default)
  {
    using IDisposable context0 = LogContext.Push(CreateEnrichers<T>(request));

    SpeckleLog.Logger.Debug("Starting execution of graphql request to get {resultType}", typeof(T).Name);
    var timer = new Stopwatch();
    var success = false;
    timer.Start();
    try
    {
      var result = await ExecuteWithResiliencePolicies(async () =>
        {
          var result = await _gqlClient.SendMutationAsync<T>(request, cancellationToken).ConfigureAwait(false);
          MaybeThrowFromGraphQLErrors(request, result);
          return result.Data;
        })
        .ConfigureAwait(false);
      success = true;
      return result;
    }
    // cancellations are bubbling up with no logging
    catch (OperationCanceledException)
    {
      throw;
    }
    // we catch forbidden to rethrow, making sure its not logged.
    catch (SpeckleGraphQLForbiddenException<T>)
    {
      throw;
    }
    // anything else related to graphql gets logged
    catch (SpeckleGraphQLException<T> gqlException)
    {
      SpeckleLog.Logger
        .ForContext("graphqlResponse", gqlException.Response)
        .ForContext("graphqlExtensions", gqlException.Extensions)
        .ForContext("graphqlErrorMessages", gqlException.ErrorMessages.ToList())
        .Warning(
          gqlException,
          "Execution of the graphql request to get {resultType} failed with {graphqlExceptionType} {exceptionMessage}.",
          typeof(T).Name,
          gqlException.GetType().Name,
          gqlException.Message
        );
      throw;
    }
    // we log and wrap anything that is not a graphql exception.
    // this makes sure, that any graphql operation only throws SpeckleGraphQLExceptions
    catch (Exception ex)
    {
      SpeckleLog.Logger.Warning(
        ex,
        "Execution of the graphql request to get {resultType} failed without a graphql response. Cause {exceptionMessage}",
        typeof(T).Name,
        ex.Message
      );
      throw new SpeckleGraphQLException<T>("The graphql request failed without a graphql response", ex, request, null);
    }
    finally
    {
      // this is a performance metric log operation
      // this makes sure that both success and failed operations report
      // the same performance log
      timer.Stop();
      var status = success ? "succeeded" : "failed";
      SpeckleLog.Logger.Information(
        "Execution of graphql request to get {resultType} {resultStatus} after {elapsed} seconds",
        typeof(T).Name,
        status,
        timer.Elapsed.TotalSeconds
      );
    }
  }

  internal void MaybeThrowFromGraphQLErrors<T>(GraphQLRequest request, GraphQLResponse<T> response)
  {
    // The errors reflect the Apollo server v2 API, which is deprecated. It is bound to change,
    // once we migrate to a newer version.
    var errors = response.Errors;
    if (errors != null && errors.Any())
    {
      var errorMessages = errors.Select(e => e.Message);
      if (
        errors.Any(
          e =>
            e.Extensions != null
            && (
              e.Extensions.Contains(new KeyValuePair<string, object>("code", "FORBIDDEN"))
              || e.Extensions.Contains(new KeyValuePair<string, object>("code", "UNAUTHENTICATED"))
            )
        )
      )
        throw new SpeckleGraphQLForbiddenException<T>(request, response);

      if (
        errors.Any(
          e =>
            e.Extensions != null
            && e.Extensions.Contains(new KeyValuePair<string, object>("code", "INTERNAL_SERVER_ERROR"))
        )
      )
        throw new SpeckleGraphQLInternalErrorException<T>(request, response);

      throw new SpeckleGraphQLException<T>("Request failed with errors", request, response);
    }
  }

  private static Dictionary<string, object?> ConvertExpandoToDict(ExpandoObject expando)
  {
    var variables = new Dictionary<string, object?>();
    foreach (KeyValuePair<string, object> kvp in expando)
    {
      object value;
      if (kvp.Value is ExpandoObject ex)
        value = ConvertExpandoToDict(ex);
      else
        value = kvp.Value;
      variables[kvp.Key] = value;
    }
    return variables;
  }

  private ILogEventEnricher[] CreateEnrichers<T>(GraphQLRequest request)
  {
    // i know this is double  (de)serializing, but we need a recursive convert to
    // dict<str, object> here
    var expando = JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(request.Variables));
    var variables = request.Variables != null && expando != null ? ConvertExpandoToDict(expando) : null;
    return new ILogEventEnricher[]
    {
      new PropertyEnricher("serverUrl", ServerUrl),
      new PropertyEnricher("graphqlQuery", request.Query),
      new PropertyEnricher("graphqlVariables", variables),
      new PropertyEnricher("resultType", typeof(T).Name)
    };
  }

  internal IDisposable SubscribeTo<T>(GraphQLRequest request, Action<object, T> callback)
  {
    using (LogContext.Push(CreateEnrichers<T>(request)))
      try
      {
        var res = _gqlClient.CreateSubscriptionStream<T>(request);
        return res.Subscribe(
          response =>
          {
            try
            {
              MaybeThrowFromGraphQLErrors(request, response);

              if (response.Data != null)
                callback(this, response.Data);
              else
                SpeckleLog.Logger
                  .ForContext("graphqlResponse", response)
                  .Error("Cannot execute graphql callback for {resultType}, the response has no data.", typeof(T).Name);
            }
            // we catch forbidden to rethrow, making sure its not logged.
            catch (SpeckleGraphQLForbiddenException<T>)
            {
              throw;
            }
            // anything else related to graphql gets logged
            catch (SpeckleGraphQLException<T> gqlException)
            {
              SpeckleLog.Logger
                .ForContext("graphqlResponse", gqlException.Response)
                .ForContext("graphqlExtensions", gqlException.Extensions)
                .ForContext("graphqlErrorMessages", gqlException.ErrorMessages.ToList())
                .Warning(
                  gqlException,
                  "Execution of the graphql request to get {resultType} failed with {graphqlExceptionType} {exceptionMessage}.",
                  typeof(T).Name,
                  gqlException.GetType().Name,
                  gqlException.Message
                );
              throw;
            }
            // we're not handling the bare Exception type here,
            // since we have a response object on the callback, we know the Exceptions
            // can only be thrown from the MaybeThrowFromGraphQLErrors which wraps
            // every exception into SpeckleGraphQLException
          },
          ex =>
          {
            // we're logging this as an error for now, to keep track of failures
            // so far we've swallowed these errors
            SpeckleLog.Logger.Error(
              ex,
              "Subscription request for {resultType} failed with {exceptionMessage}",
              typeof(T).Name,
              ex.Message
            );
            // we could be throwing like this:
            // throw ex;
          }
        );
      }
      catch (Exception ex)
      {
        SpeckleLog.Logger.Warning(
          ex,
          "Subscribing to graphql {resultType} failed without a graphql response. Cause {exceptionMessage}",
          typeof(T).Name,
          ex.Message
        );
        throw new SpeckleGraphQLException<T>(
          "The graphql request failed without a graphql response",
          ex,
          request,
          null
        );
      }
  }
}
