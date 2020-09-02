﻿using MaterialDesignThemes.Wpf;
using Speckle.Core.Credentials;
using Speckle.Core.Api;
using Speckle.DesktopUI.Accounts;
using Speckle.DesktopUI.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Speckle.DesktopUI.Streams
{
  class StreamCreateViewModel : BindableBase
  {
    public StreamCreateViewModel()
    {
      SelectedSlide = 0;
      _createButtonLoading = false;
      _streamToCreate = new Stream();
      _accountToSendFrom = _acctRepo.GetDefault();

      MessageQueue = new SnackbarMessageQueue();
      ContinueStreamCreatecommand = new RelayCommand<string>(OnContinueStreamCreate);
      ChangeSlideCommand = new RelayCommand<string>(OnChangeSlide);
      CloseDialogCommand = new RelayCommand<string>(OnCloseDialog);
      CreateStreamCommand = new RelayCommand<string>(OnCreateStream);
      CopyStreamCommand = new RelayCommand<string>(OnCopyStreamCommand);
    }

    private StreamsRepository _repo = new StreamsRepository();
    private AccountsRepository _acctRepo = new AccountsRepository();

    private Stream _streamToCreate;
    public Stream StreamToCreate
    {
      get => _streamToCreate;
      set => SetProperty(ref _streamToCreate, value);
    }
    private Account _accountToSendFrom;
    public Account AccountToSendFrom
    {
      get => _accountToSendFrom;
      set => SetProperty(ref _accountToSendFrom, value);
    }

    private int _selectedSlide;
    public int SelectedSlide
    {
      get => _selectedSlide;
      set => SetProperty(ref _selectedSlide, value);
    }
    private SnackbarMessageQueue _messageQueue;
    public SnackbarMessageQueue MessageQueue
    {
      get => _messageQueue;
      set => SetProperty(ref _messageQueue, value);
    }

    private bool _createButtonLoading;
    public bool CreateButtonLoading
    {
      get => _createButtonLoading;
      set => SetProperty(ref _createButtonLoading, value);
    }

    public RelayCommand<string> ContinueStreamCreatecommand { get; set; }
    private void OnContinueStreamCreate(string slideIndex)
    {
      if (StreamToCreate.name == null || StreamToCreate.name.Length < 2)
      {
        MessageQueue.Enqueue("Please choose a name for your stream!");
        return;
      }
      AccountToSendFrom = _acctRepo.GetDefault();
      SelectedSlide = Int32.Parse(slideIndex);

    }
    public RelayCommand<string> ChangeSlideCommand { get; set; }
    public RelayCommand<string> CloseDialogCommand { get; set; }
    private void OnChangeSlide(string slideIndex)
    {
      SelectedSlide = Int32.Parse(slideIndex);
    }

    private void OnCloseDialog(string arg)
    {
      DialogHost.CloseDialogCommand.Execute(null, null);
      SelectedSlide = 0;
      StreamToCreate = new Stream();
    }
    public RelayCommand<string> CreateStreamCommand { get; set; }
    private async void OnCreateStream(string arg)
    {
      CreateButtonLoading = true;
      try
      {
        string streamId = await _repo.CreateStream(StreamToCreate, AccountToSendFrom);
        StreamToCreate = await _repo.GetStream(streamId, AccountToSendFrom);
        SelectedSlide = 3;
      }
      catch (Exception e)
      {
        MessageQueue.Enqueue($"Error: {e.Message}");
      }
      CreateButtonLoading = false;
    }
    public RelayCommand<string> CopyStreamCommand { get; set; }
    private void OnCopyStreamCommand(string arg)
    {
      Clipboard.SetText(StreamToCreate.id);
      MessageQueue.Enqueue("Stream ID copied to clipboard!");
    }
  }
}
