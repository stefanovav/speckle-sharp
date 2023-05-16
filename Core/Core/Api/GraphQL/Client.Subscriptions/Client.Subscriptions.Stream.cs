using System;
using GraphQL;
using Speckle.Core.Api.SubscriptionModels;

namespace Speckle.Core.Api;

public partial class Client
{
  #region UserStreamAdded

  public delegate void UserStreamAddedHandler(object sender, StreamInfo e);

  public event UserStreamAddedHandler OnUserStreamAdded;
  private IDisposable _userStreamAddedSubscription;

  /// <summary>
  /// Subscribe to events of streams added for the current user
  /// </summary>
  /// <returns></returns>
  public void SubscribeUserStreamAdded()
  {
    var request = new GraphQLRequest { Query = @"subscription { userStreamAdded }" };

    _userStreamAddedSubscription = SubscribeTo<UserStreamAddedResult>(
      request,
      (sender, result) => OnUserStreamAdded?.Invoke(sender, result.userStreamAdded)
    );
  }

  public bool HasSubscribedUserStreamAdded => _userStreamAddedSubscription != null;

  #endregion

  #region StreamUpdated

  public delegate void StreamUpdatedHandler(object sender, StreamInfo e);

  public event StreamUpdatedHandler OnStreamUpdated;
  private IDisposable _streamUpdatedSubscription;

  /// <summary>
  /// Subscribe to events of streams updated for a specific streamId
  /// </summary>
  /// <param name="id">streamId</param>
  public void SubscribeStreamUpdated(string id)
  {
    var request = new GraphQLRequest { Query = $@"subscription {{ streamUpdated( streamId: ""{id}"") }}" };
    _streamUpdatedSubscription = SubscribeTo<StreamUpdatedResult>(
      request,
      (sender, result) => OnStreamUpdated?.Invoke(sender, result.streamUpdated)
    );
  }

  public bool HasSubscribedStreamUpdated => _streamUpdatedSubscription != null;

  #endregion

  #region StreamRemoved

  public delegate void UserStreamRemovedHandler(object sender, StreamInfo e);

  public event UserStreamRemovedHandler OnUserStreamRemoved;
  private IDisposable _userStreamRemovedSubscription;

  /// <summary>
  /// Subscribe to events of streams removed for the current user
  /// </summary>
  /// <param name="id"></param>
  public void SubscribeUserStreamRemoved()
  {
    var request = new GraphQLRequest { Query = @"subscription { userStreamRemoved }" };

    _userStreamRemovedSubscription = SubscribeTo<UserStreamRemovedResult>(
      request,
      (sender, result) => OnUserStreamRemoved?.Invoke(sender, result.userStreamRemoved)
    );
  }

  public bool HasSubscribedUserStreamRemoved => _userStreamRemovedSubscription != null;

  #endregion

  #region CommentActivity

  public delegate void CommentActivityHandler(object sender, CommentItem e);

  public event CommentActivityHandler OnCommentActivity;
  private IDisposable _commentActivitySubscription;

  /// <summary>
  /// Subscribe to new comment events
  /// </summary>
  ///
  public void SubscribeCommentActivity(string streamId)
  {
    var request = new GraphQLRequest
    {
      Query =
        $@"subscription {{ commentActivity( streamId: ""{streamId}"") {{ type comment {{ id authorId archived screenshot rawText }} }} }}"
    };
    _commentActivitySubscription = SubscribeTo<CommentActivityResponse>(
      request,
      (sender, result) => OnCommentActivity?.Invoke(sender, result.commentActivity.comment)
    );
  }

  public bool HasSubscribedCommentActivity => _commentActivitySubscription != null;

  #endregion
}
