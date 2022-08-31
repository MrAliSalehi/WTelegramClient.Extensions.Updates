using TL;
using WTelegram;

namespace WTelegramClient.Extensions.Updates.Internal;

internal static class FilterChatTypeExtension
{
    internal static Task PerformActionBasedOnUpdateTypeAsync<TPeer>(this Client client, Func<Update, TPeer, Task> actionOnUpdate, Update update) where TPeer : Peer
    {
        var todo = Task.CompletedTask;

        switch (update)
        {
            case UpdateNewChannelMessage updateNewChannelMessage when (updateNewChannelMessage.message.Peer is TPeer peer):
            {
                todo = actionOnUpdate(updateNewChannelMessage, peer);
                break;
            }

            case UpdateNewMessage updateNewMessage when (updateNewMessage.message.Peer is TPeer peer):
            {
                todo = actionOnUpdate(updateNewMessage, peer);
                break;
            }

            case UpdateEditMessage updateEditMessage when (updateEditMessage.message.Peer is TPeer peer):
            {
                todo = actionOnUpdate(updateEditMessage, peer);
                break;
            }
            case UpdateDeleteChannelMessages updateDeleteChannelMessages:
            {
                todo = client.DoActionForEachDeletedMessageAsync(actionOnUpdate, updateDeleteChannelMessages);
                break;
            }
            case UpdateFolderPeers updateFolderPeers:
            {
                var matchPeer = updateFolderPeers.folder_peers.FirstOrDefault(p => p.peer is TPeer);
                if (matchPeer is not null)
                    todo = actionOnUpdate(updateFolderPeers, (matchPeer.peer as TPeer)!);
                break;
            }

            case UpdateGeoLiveViewed { peer: TPeer peer } updateGeoLiveViewed:
            {
                todo = actionOnUpdate(updateGeoLiveViewed, peer);
                break;
            }
            case UpdateChatUserTyping { from_id: TPeer peer } updateChatUserTyping:
            {
                todo = actionOnUpdate(updateChatUserTyping, peer);
                break;
            }
            case UpdateChannelUserTyping { from_id: TPeer peer } updateChannelUserTyping:
            {
                todo = actionOnUpdate(updateChannelUserTyping, peer);
                break;
            }
            case UpdateMessageReactions { peer: TPeer peer } updateMessageReactions:
            {
                todo = actionOnUpdate(updateMessageReactions, peer);
                break;
            }
            case UpdateNewScheduledMessage updateNewScheduledMessage when (updateNewScheduledMessage.message.Peer is TPeer peer):
            {
                todo = actionOnUpdate(updateNewScheduledMessage, peer);
                break;
            }
            case UpdateNotifySettings { peer: TPeer peer } updateNotifySettings:
            {
                todo = actionOnUpdate(updateNotifySettings, peer);

                break;
            }
            case UpdatePeerBlocked { peer_id: TPeer peer } updatePeerBlocked:
            {
                todo = actionOnUpdate(updatePeerBlocked, peer);
                break;
            }
            case UpdatePeerHistoryTTL { peer: TPeer peer } updatePeerHistoryTtl:
            {
                todo = actionOnUpdate(updatePeerHistoryTtl, peer);
                break;
            }

            case UpdatePeerSettings { peer: TPeer peer } updatePeerSettings:
            {
                todo = actionOnUpdate(updatePeerSettings, peer);
                break;
            }
            case UpdatePendingJoinRequests { peer: TPeer peer } updatePendingJoinRequests:
            {
                todo = actionOnUpdate(updatePendingJoinRequests, peer);
                break;
            }
            case UpdatePinnedMessages { peer: TPeer peer } updatePinnedMessages:
            {
                todo = actionOnUpdate(updatePinnedMessages, peer);
                break;
            }
            case UpdateReadHistoryInbox { peer: TPeer peer } updateReadHistoryInbox:
            {
                todo = actionOnUpdate(updateReadHistoryInbox, peer);
                break;
            }
            case UpdateReadHistoryOutbox { peer: TPeer peer } updateReadHistoryOutbox:
            {
                todo = actionOnUpdate(updateReadHistoryOutbox, peer);
                break;
            }

            case UpdateTranscribedAudio { peer: TPeer peer } updateTranscribedAudio:
            {
                todo = actionOnUpdate(updateTranscribedAudio, peer);
                break;
            }

            case UpdateBotCallbackQuery { peer: TPeer peer } updateBotCallbackQuery:
            {
                todo = actionOnUpdate(updateBotCallbackQuery, peer);
                break;
            }
            case UpdateBotChatInviteRequester { peer: TPeer peer } updateBotChatInviteRequester:
            {
                todo = actionOnUpdate(updateBotChatInviteRequester, peer);
                break;
            }

            case UpdateBotCommands { peer: TPeer peer } updateBotCommands:
            {
                todo = actionOnUpdate(updateBotCommands, peer);
                break;
            }
            case UpdateChatDefaultBannedRights { peer: TPeer peer } updateChatDefaultBannedRights:
            {
                todo = actionOnUpdate(updateChatDefaultBannedRights, peer);
                break;
            }
            case UpdateDeleteScheduledMessages { peer: TPeer peer } updateDeleteScheduledMessages:
            {
                todo = actionOnUpdate(updateDeleteScheduledMessages, peer);
                break;
            }
            case UpdateDialogPinned { peer: TPeer peer } updateDialogPinned:
            {
                todo = actionOnUpdate(updateDialogPinned, peer);
                break;
            }
            case UpdateDialogUnreadMark { peer: TPeer peer } updateDialogUnreadMark:
            {
                todo = actionOnUpdate(updateDialogUnreadMark, peer);
                break;
            }
            case UpdateDraftMessage { peer: TPeer peer } updateDraftMessage:
                todo = actionOnUpdate(updateDraftMessage, peer);
                break;
            default:
                throw new NotSupportedException();
        }

        return todo;
    }

    private static async Task DoActionForEachDeletedMessageAsync<TPeer>(this Client client, Func<Update, TPeer, Task> actionOnUpdate,
        UpdateDeleteChannelMessages updateDeleteChannelMessages) where TPeer : Peer
    {
        var channel = await UpdateHelpers.GetChatAsync<Channel>(client, updateDeleteChannelMessages.channel_id);

        var messages = await client.Channels_GetMessages(channel,
            updateDeleteChannelMessages.messages.ToInputMessageId());

        foreach (var message in messages.Messages)
        {
            if (!message.IsValidPeerType<TPeer>(out var peer))
                continue;

            await actionOnUpdate(updateDeleteChannelMessages, peer);
        }
    }
}