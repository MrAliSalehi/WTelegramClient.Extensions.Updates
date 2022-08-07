using TL;
using WTelegram;

namespace WTelegramClient.Extensions.Updates.Internal;

internal static class FilterChatTypeExtension
{
    internal static void FilterByChatType<TPeer>(this Client client, Func<Update, TPeer, Task> actionOnUpdate) where TPeer : Peer
    {
        client.OnUpdate += async obj =>
        {
            if (!obj.IsUpdateBase(out var updatesBase))
                return;

            updatesBase.CollectUsersChats(UpdateHelpers.Users, UpdateHelpers.Chats);

            foreach (var update in updatesBase.UpdateList)
            {
                await client.PerformActionBasedOnUpdateType(actionOnUpdate, update);
            }
        };
    }

    private static async Task PerformActionBasedOnUpdateType<TPeer>(this Client client, Func<Update, TPeer, Task> actionOnUpdate, Update update) where TPeer : Peer
    {
        switch (update)
        {
            case UpdateNewChannelMessage updateNewChannelMessage when (updateNewChannelMessage.message.Peer is TPeer peer):
                {
                    await actionOnUpdate(updateNewChannelMessage, peer);
                    break;
                }

            case UpdateNewMessage updateNewMessage when (updateNewMessage.message.Peer is TPeer peer):
                {
                    await actionOnUpdate(updateNewMessage, peer);
                    break;
                }

            case UpdateEditMessage updateEditMessage when (updateEditMessage.message.Peer is TPeer peer):
                {
                    await actionOnUpdate(updateEditMessage, peer);
                    break;
                }
            case UpdateDeleteChannelMessages updateDeleteChannelMessages:
                {
                    await client.DoActionForEachDeletedMessage(actionOnUpdate, updateDeleteChannelMessages);
                    break;
                }
            case UpdateFolderPeers updateFolderPeers:
                {
                    var matchPeer = updateFolderPeers.folder_peers.FirstOrDefault(p => p.peer is TPeer);
                    if (matchPeer is not null)
                        await actionOnUpdate(updateFolderPeers, (matchPeer.peer as TPeer)!);
                    break;
                }

            case UpdateGeoLiveViewed { peer: TPeer peer } updateGeoLiveViewed:
                {
                    await actionOnUpdate(updateGeoLiveViewed, peer);
                    break;
                }
            case UpdateChatUserTyping { from_id: TPeer peer } updateChatUserTyping:
                {
                    await actionOnUpdate(updateChatUserTyping, peer);
                    break;
                }


            case UpdateChannelUserTyping { from_id: TPeer peer } updateChannelUserTyping:
                {
                    await actionOnUpdate(updateChannelUserTyping, peer);
                    break;
                }
            case UpdateMessageReactions { peer: TPeer peer } updateMessageReactions:
                {
                    await actionOnUpdate(updateMessageReactions, peer);
                    break;
                }
            case UpdateNewScheduledMessage updateNewScheduledMessage when (updateNewScheduledMessage.message.Peer is TPeer peer):
                {
                    await actionOnUpdate(updateNewScheduledMessage, peer);
                    break;
                }
            case UpdateNotifySettings { peer: TPeer peer } updateNotifySettings:
                {
                    await actionOnUpdate(updateNotifySettings, peer);

                    break;
                }
            case UpdatePeerBlocked { peer_id: TPeer peer } updatePeerBlocked:
                {
                    await actionOnUpdate(updatePeerBlocked, peer);
                    break;
                }
            case UpdatePeerHistoryTTL { peer: TPeer peer } updatePeerHistoryTtl:
                {
                    await actionOnUpdate(updatePeerHistoryTtl, peer);
                    break;
                }

            case UpdatePeerSettings { peer: TPeer peer } updatePeerSettings:
                {
                    await actionOnUpdate(updatePeerSettings, peer);
                    break;
                }
            case UpdatePendingJoinRequests { peer: TPeer peer } updatePendingJoinRequests:
                {
                    await actionOnUpdate(updatePendingJoinRequests, peer);
                    break;
                }
            case UpdatePinnedMessages { peer: TPeer peer } updatePinnedMessages:
                {
                    await actionOnUpdate(updatePinnedMessages, peer);
                    break;
                }
            case UpdateReadHistoryInbox { peer: TPeer peer } updateReadHistoryInbox:
                {
                    await actionOnUpdate(updateReadHistoryInbox, peer);
                    break;
                }
            case UpdateReadHistoryOutbox { peer: TPeer peer } updateReadHistoryOutbox:
                {
                    await actionOnUpdate(updateReadHistoryOutbox, peer);
                    break;
                }

            case UpdateTranscribedAudio { peer: TPeer peer } updateTranscribedAudio:
                {
                    await actionOnUpdate(updateTranscribedAudio, peer);
                    break;
                }

            case UpdateBotCallbackQuery { peer: TPeer peer } updateBotCallbackQuery:
                {
                    await actionOnUpdate(updateBotCallbackQuery, peer);
                    break;
                }
            case UpdateBotChatInviteRequester { peer: TPeer peer } updateBotChatInviteRequester:
                {
                    await actionOnUpdate(updateBotChatInviteRequester, peer);
                    break;
                }

            case UpdateBotCommands { peer: TPeer peer } updateBotCommands:
                {
                    await actionOnUpdate(updateBotCommands, peer);
                    break;
                }
            case UpdateChatDefaultBannedRights { peer: TPeer peer } updateChatDefaultBannedRights:
                {
                    await actionOnUpdate(updateChatDefaultBannedRights, peer);
                    break;
                }
            case UpdateDeleteScheduledMessages { peer: TPeer peer } updateDeleteScheduledMessages:
                {
                    await actionOnUpdate(updateDeleteScheduledMessages, peer);
                    break;
                }
            case UpdateDialogPinned { peer: TPeer peer } updateDialogPinned:
                {
                    await actionOnUpdate(updateDialogPinned, peer);
                    break;
                }
            case UpdateDialogUnreadMark { peer: TPeer peer } updateDialogUnreadMark:
                {
                    await actionOnUpdate(updateDialogUnreadMark, peer);
                    break;
                }
            case UpdateDraftMessage { peer: TPeer peer } updateDraftMessage:
                {
                    await actionOnUpdate(updateDraftMessage, peer);
                    break;
                }

            //START todo move to filter by id
            case UpdateEncryptedChatTyping updateEncryptedChatTyping: break;
            case UpdateEncryptedMessagesRead updateEncryptedMessagesRead: break;
            case UpdateEncryption updateEncryption: break;
            case UpdateGroupCall updateGroupCall: break;
            case UpdateGroupCallParticipants updateGroupCallParticipants: break;
            case UpdateInlineBotCallbackQuery updateInlineBotCallbackQuery: break;
            case UpdateMessagePoll updateMessagePoll: break;
            case UpdateMessagePollVote updateMessagePollVote: break;
            case UpdateNewEncryptedMessage updateNewEncryptedMessage: break;
            case UpdateNewStickerSet updateNewStickerSet: break;
            case UpdatePhoneCall updatePhoneCall: break;
            case UpdatePhoneCallSignalingData updatePhoneCallSignalingData: break;
            case UpdatePinnedChannelMessages updatePinnedChannelMessages: break;
            case UpdatePinnedDialogs updatePinnedDialogs: break;
            case UpdateReadChannelDiscussionInbox updateReadChannelDiscussionInbox: break;
            case UpdateReadChannelDiscussionOutbox updateReadChannelDiscussionOutbox: break;
            case UpdateReadChannelInbox updateReadChannelInbox: break;
            case UpdateReadChannelOutbox updateReadChannelOutbox: break;
            case UpdateReadMessagesContents updateReadMessagesContents: break;
            case UpdateStickerSetsOrder updateStickerSetsOrder: break;
            case UpdateTheme updateTheme: break;
            case UpdateUserPhone updateUserPhone: break;
            case UpdateUserPhoto updateUserPhoto: break;
            case UpdateUserStatus updateUserStatus: break;

            case UpdateUserTyping updateUserTyping: break;
            case UpdateWebPage updateWebPage: break;
            case UpdateWebViewResultSent updateWebViewResultSent: break;
            case UpdateDeleteMessages updateDeleteMessages: break;
            case UpdateBotInlineQuery updateBotInlineQuery: break;
            case UpdateBotInlineSend updateBotInlineSend: break;
            case UpdateBotMenuButton updateBotMenuButton: break;
            case UpdateBotPrecheckoutQuery updateBotPrecheckoutQuery: break;
            case UpdateBotShippingQuery updateBotShippingQuery: break;
            case UpdateBotStopped updateBotStopped: break;
            case UpdateBotWebhookJSONQuery updateBotWebHookJsonQuery: break;
            case UpdateChannel updateChannel: break;
            case UpdateChannelParticipant updateChannelParticipant: break;
            case UpdateChannelTooLong updateChannelTooLong: break;
            case UpdateChannelUserTyping updateChannelUserTyping1: break;
            case UpdateChat updateChat: break;
            case UpdateDcOptions updateDcOptions: break;
            case UpdateChatParticipant updateChatParticipant: break;
            case UpdateChatParticipants updateChatParticipants: break;
            case UpdateDialogFilter updateDialogFilter: break;
            case UpdateDialogFilterOrder updateDialogFilterOrder: break;

            //END todo move to filter by id


            //START todo wtf?
            case UpdateFavedStickers updateFavedStickers: break;
            case UpdateGroupCallConnection updateGroupCallConnection: break;
            case UpdateLangPack updateLangPack: break;
            case UpdateLangPackTooLong updateLangPackTooLong: break;
            case UpdateLoginToken updateLoginToken: break;
            case UpdateMessageID updateMessageId: break;
            case UpdatePeerLocated updatePeerLocated: break;
            case UpdatePrivacy updatePrivacy: break;
            case UpdatePtsChanged updatePtsChanged: break;
            case UpdateReadFeaturedEmojiStickers updateReadFeaturedEmojiStickers: break;
            case UpdateReadFeaturedStickers updateReadFeaturedStickers: break;
            case UpdateRecentStickers updateRecentStickers: break;
            case UpdateSavedRingtones updateSavedRingtones: break;
            case UpdateSavedGifs updateSavedGifs: break;
            case UpdateServiceNotification updateServiceNotification: break;
            case UpdateStickerSets updateStickerSets: break;
            case UpdateUserName updateUserName: break;
            case UpdateAttachMenuBots updateAttachMenuBots: break;
            case UpdateBotWebhookJSON updateBotWebHookJson: break;
            case UpdateConfig updateConfig: break;
            case UpdateContactsReset updateContactsReset: break;
            case UpdateDialogFilters updateDialogFilters: break;

                //END todo wtf?


        }
    }

    private static async Task DoActionForEachDeletedMessage<TPeer>(this Client client, Func<Update, TPeer, Task> actionOnUpdate,
        UpdateDeleteChannelMessages updateDeleteChannelMessages) where TPeer : Peer
    {
        var channel = await UpdateHelpers.GetChatAsync<Channel>(client, updateDeleteChannelMessages.channel_id);

        var messages = await client.Channels_GetMessages(channel,
            updateDeleteChannelMessages.messages.ToInputMessageId());

        foreach (var message in messages.Messages)
        {
            if (message.IsValidPeerType<TPeer>(out var peer))
                continue;

            await actionOnUpdate(updateDeleteChannelMessages, peer);
        }
    }
}