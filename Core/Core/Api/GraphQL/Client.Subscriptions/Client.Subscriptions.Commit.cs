using System;
using GraphQL;
using Speckle.Core.Api.SubscriptionModels;

namespace Speckle.Core.Api;

public partial class Client
{
  #region CommitCreated

  public delegate void CommitCreatedHandler(object sender, CommitInfo e);

  public event CommitCreatedHandler OnCommitCreated;
  private IDisposable _commitCreatedSubscription;

  /// <summary>
  /// Subscribe to events of commit created for a stream
  /// </summary>
  /// <returns></returns>
  public void SubscribeCommitCreated(string streamId)
  {
    var request = new GraphQLRequest { Query = $@"subscription {{ commitCreated (streamId: ""{streamId}"") }}" };

    _commitCreatedSubscription = SubscribeTo<CommitCreatedResult>(
      request,
      (sender, result) => OnCommitCreated?.Invoke(sender, result.commitCreated)
    );
  }

  public bool HasSubscribedCommitCreated => _commitCreatedSubscription != null;

  #endregion

  #region CommitUpdated

  public delegate void CommitUpdatedHandler(object sender, CommitInfo e);

  public event CommitUpdatedHandler OnCommitUpdated;
  private IDisposable _commitUpdatedSubscription;

  /// <summary>
  /// Subscribe to events of commit updated for a stream
  /// </summary>
  /// <returns></returns>
  public void SubscribeCommitUpdated(string streamId, string commitId = null)
  {
    var request = new GraphQLRequest
    {
      Query = $@"subscription {{ commitUpdated (streamId: ""{streamId}"", commitId: ""{commitId}"") }}"
    };

    var res = _gqlClient.CreateSubscriptionStream<CommitUpdatedResult>(request);
    _commitUpdatedSubscription = SubscribeTo<CommitUpdatedResult>(
      request,
      (sender, result) => OnCommitUpdated?.Invoke(sender, result.commitUpdated)
    );
  }

  public bool HasSubscribedCommitUpdated => _commitUpdatedSubscription != null;

  #endregion

  #region CommitDeleted

  public delegate void CommitDeletedHandler(object sender, CommitInfo e);

  public event CommitDeletedHandler OnCommitDeleted;
  private IDisposable _commitDeletedSubscription;

  /// <summary>
  /// Subscribe to events of commit updated for a stream
  /// </summary>
  /// <returns></returns>
  public void SubscribeCommitDeleted(string streamId)
  {
    var request = new GraphQLRequest { Query = $@"subscription {{ commitDeleted (streamId: ""{streamId}"") }}" };
    _commitDeletedSubscription = SubscribeTo<CommitDeletedResult>(
      request,
      (sender, result) => OnCommitDeleted?.Invoke(sender, result.commitDeleted)
    );
  }

  public bool HasSubscribedCommitDeleted => _commitDeletedSubscription != null;

  #endregion
}
