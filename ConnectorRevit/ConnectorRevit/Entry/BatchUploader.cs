using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DesktopUI2.Models;
using DesktopUI2.ViewModels;
using Speckle.ConnectorRevit.UI;
using Speckle.Core.Api;
using Speckle.Core.Credentials;
using Speckle.Core.Logging;
using Stream = Speckle.Core.Api.Stream;

namespace ConnectorRevit.Entry
{
  internal class BatchUploader
  {
    private static ConnectorBindingsRevit _bindings { get; set; }
    private System.Threading.SynchronizationContext _revitContext = SynchronizationContext.Current;

    public BatchUploader(ConnectorBindingsRevit bindings)
    {

      Task.Run(StartListeningForMessage);
      SendMessageToServer("NEXT", new CancellationToken());
      _bindings = bindings;
    }

    private async void UploadFile(string path)
    {
      try
      {
        if (!File.Exists(path))
        {
          throw new FileNotFoundException();
        }

        var doc = ConnectorBindingsRevit.RevitApp.OpenAndActivateDocument(path);
        string filename = _bindings.GetFileName();
        var fileStream = await GetOrCreateStream(filename);
        var filters = _bindings.GetSelectionFilters();
        var selection = _bindings.GetSelectedObjects();

        fileStream.Filter = filters.First(o => o.Slug == "all");
        fileStream.CommitMessage = "Sent everything from BatchUploader";
        fileStream.BranchName = "main";

        // set settings
        if (fileStream.Settings == null || fileStream.Settings.Count == 0)
        {
          var settings = _bindings.GetSettings();
          fileStream.Settings = settings;
        }
        var Id = await Task.Run(() => _bindings.SendStream(fileStream, new ProgressViewModel()));

        doc.Document.Close(false);

      }
      catch (Exception ex)
      {
        SpeckleLog.Logger
         .ForContext("path", path)
         .Debug(ex, "Swallowing exception in {methodName}: {exceptionMessage}", nameof(SearchStreams), ex.Message);
      }

      SendMessageToServer("NEXT", new CancellationToken());
    }

    #region api
    private async Task<StreamState> GetOrCreateStream(string filename)
    {
      // get default account
      var account = AccountManager.GetDefaultAccount();
      var client = new Client(account);

      var foundStream = await SearchStreams(client, filename);

      if (foundStream != null)
        return new StreamState(account, foundStream);


      // create the stream
      string streamId = await client.StreamCreate(new StreamCreateInput { description = "Somethig useful here", name = filename, isPublic = false });
      var newStream = await client.StreamGet(streamId);

      return new StreamState(account, newStream);


    }

    private async Task<Stream> SearchStreams(Client client, string filename)
    {
      Stream stream = null;
      try
      {
        var streams = await client.StreamSearch(filename).ConfigureAwait(true);
        stream = streams.FirstOrDefault(s => s.name == filename);
      }
      catch (Exception ex)
      {
        SpeckleLog.Logger
          .ForContext("fileName", filename)
          .Debug(ex, "Swallowing exception in {methodName}: {exceptionMessage}", nameof(SearchStreams), ex.Message);
      }
      return stream;
    }
    #endregion

    #region pipes

    internal async void SendMessageToServer(string message, CancellationToken token)
    {
      using (
        NamedPipeServerStream pipeServer = new NamedPipeServerStream(
          "specklebatchuploaderserver",
          PipeDirection.Out,
          100
        )
      )
      {
        await pipeServer.WaitForConnectionAsync(token);

        try
        {
          using (StreamWriter sw = new StreamWriter(pipeServer))
          {
            sw.AutoFlush = true;
            await sw.WriteLineAsync(message);
          }
        }
        catch (IOException e)
        {
          throw new Exception("Send message: {args} failed, pipe is broken.", e);
        }
      }
    }

    internal async void StartListeningForMessage()
    {
      using (
        NamedPipeClientStream pipeClient = new NamedPipeClientStream(
          ".",
          "specklebatchuploaderclient",
          PipeDirection.In
        )
      )
      {
        await pipeClient.ConnectAsync();

        using (StreamReader sr = new StreamReader(pipeClient))
        {
          string line;
          while ((line = sr.ReadLine()) != null)
          {
            if (line.StartsWith("UPLOAD:"))
            {
              var filename = line.Replace("UPLOAD:", "");
              _revitContext.Post(delegate
              {
                UploadFile(filename);
              }, null);
            }
          }
        }
      }
      //restart client
      StartListeningForMessage();
    }

    #endregion
  }
}
