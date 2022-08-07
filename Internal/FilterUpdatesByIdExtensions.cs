using TL;

namespace WTelegramClient.Extensions.Updates.Internal;

internal static class FilterUpdatesByIdExtensions
{
    public static void FilterUpdatesByIdToPerformAnAction<T>(long id, Action<T, UpdatesBase> actionOnUpdate, UpdatesBase updatesBase)
        where T : Update, new()
    {
        foreach (var update in updatesBase.UpdateList)
        {
            if (!update.IsValidUpdateType<T>())
                continue;

            update.FilterUpdateById(id, actionOnUpdate, updatesBase);
        }
    }

    private static void FilterUpdateById<T>(this Update update, long id, Action<T, UpdatesBase> actionOnUpdate, UpdatesBase updatesBase)
        where T : Update, new()
    {
        switch (update)
        {
            case UpdateUserTyping updateUserTyping when updateUserTyping.user_id == id:
                actionOnUpdate((updateUserTyping as T)!, updatesBase);
                break;

            case UpdateChatParticipants updateChatParticipants when UpdateHelpers.IsChatIdOrAnyParticipantMatch(id, updateChatParticipants):
                actionOnUpdate((updateChatParticipants as T)!, updatesBase);
                break;

            case UpdateUserStatus updateUserStatus when updateUserStatus.user_id == id:
                actionOnUpdate((updateUserStatus as T)!, updatesBase);
                break;

            case UpdateUserPhoto updateUserPhoto when updateUserPhoto.user_id == id:
                actionOnUpdate((updateUserPhoto as T)!, updatesBase);
                break;

            //START todo move to filter by id
            case UpdateEncryptedChatTyping updateEncryptedChatTyping when (updateEncryptedChatTyping.chat_id == id):
                actionOnUpdate((updateEncryptedChatTyping as T)!, updatesBase);
                break;

            case UpdateEncryptedMessagesRead updateEncryptedMessagesRead when (updateEncryptedMessagesRead.chat_id == id):
                actionOnUpdate((updateEncryptedMessagesRead as T)!, updatesBase);
                break;

            case UpdateEncryption updateEncryption when (updateEncryption.chat.ID == id):
                actionOnUpdate((updateEncryption as T)!, updatesBase);
                break;

            case UpdateGroupCall updateGroupCall when (updateGroupCall.call.ID == id || updateGroupCall.chat_id == id):
                actionOnUpdate((updateGroupCall as T)!, updatesBase);
                break;

            case UpdateGroupCallParticipants updateGroupCallParticipants when (updateGroupCallParticipants.call.id == id):
                actionOnUpdate((updateGroupCallParticipants as T)!, updatesBase);
                break;

            case UpdateInlineBotCallbackQuery updateInlineBotCallbackQuery
                when (updateInlineBotCallbackQuery.user_id == id || updateInlineBotCallbackQuery.msg_id.DcId == id || updateInlineBotCallbackQuery.query_id == id):
                actionOnUpdate((updateInlineBotCallbackQuery as T)!, updatesBase);
                break;

            case UpdateMessagePoll updateMessagePoll when (updateMessagePoll.poll_id == id):
                actionOnUpdate((updateMessagePoll as T)!, updatesBase);
                break;

            case UpdateMessagePollVote updateMessagePollVote when (updateMessagePollVote.user_id == id || updateMessagePollVote.poll_id == id):
                actionOnUpdate((updateMessagePollVote as T)!, updatesBase);
                break;

            case UpdateNewEncryptedMessage updateNewEncryptedMessage when (updateNewEncryptedMessage.message.ChatId == id):
                actionOnUpdate((updateNewEncryptedMessage as T)!, updatesBase);
                break;

            case UpdateNewStickerSet updateNewStickerSet when (updateNewStickerSet.stickerset.set.id == id || updateNewStickerSet.stickerset.documents.Any(p => p.ID == id)):
                actionOnUpdate((updateNewStickerSet as T)!, updatesBase);
                break;

            case UpdatePhoneCall updatePhoneCall when (updatePhoneCall.phone_call.ID == id):
                actionOnUpdate((updatePhoneCall as T)!, updatesBase);
                break;

            case UpdatePhoneCallSignalingData updatePhoneCallSignalingData when (updatePhoneCallSignalingData.phone_call_id == id):
                actionOnUpdate((updatePhoneCallSignalingData as T)!, updatesBase);
                break;

            case UpdatePinnedChannelMessages updatePinnedChannelMessages when (updatePinnedChannelMessages.channel_id == id || updatePinnedChannelMessages.messages.Any(p => p == id)):
                actionOnUpdate((updatePinnedChannelMessages as T)!, updatesBase);
                break;

            case UpdatePinnedDialogs updatePinnedDialogs when (updatePinnedDialogs.folder_id == id):
                actionOnUpdate((updatePinnedDialogs as T)!, updatesBase);
                break;

            case UpdateReadChannelDiscussionInbox updateRcDi when (updateRcDi.channel_id == id || updateRcDi.broadcast_id == id || updateRcDi.top_msg_id == id || updateRcDi.read_max_id == id):
                actionOnUpdate((updateRcDi as T)!, updatesBase);
                break;

            case UpdateReadChannelDiscussionOutbox updateRcDo when (updateRcDo.channel_id == id || updateRcDo.read_max_id == id || updateRcDo.top_msg_id == id):
                actionOnUpdate((updateRcDo as T)!, updatesBase);
                break;

            case UpdateReadChannelInbox updateRci when (updateRci.channel_id == id || updateRci.folder_id == id || updateRci.max_id == id):
                actionOnUpdate((updateRci as T)!, updatesBase);
                break;

            case UpdateReadChannelOutbox updateReadChannelOutbox when (updateReadChannelOutbox.channel_id == id || updateReadChannelOutbox.max_id == id):
                actionOnUpdate((updateReadChannelOutbox as T)!, updatesBase);
                break;

            case UpdateReadMessagesContents updateReadMessagesContents when (updateReadMessagesContents.messages.Any(p => p == id)):
                actionOnUpdate((updateReadMessagesContents as T)!, updatesBase);
                break;

            case UpdateStickerSetsOrder updateStickerSetsOrder when (updateStickerSetsOrder.order.Any(p => p == id)):
                actionOnUpdate((updateStickerSetsOrder as T)!, updatesBase);
                break;

            case UpdateTheme updateTheme when (updateTheme.theme.document.ID == id || updateTheme.theme.id == id):
                actionOnUpdate((updateTheme as T)!, updatesBase);
                break;

            case UpdateUserPhone updateUserPhone when (updateUserPhone.user_id == id):
                actionOnUpdate((updateUserPhone as T)!, updatesBase);
                break;

            case UpdateUserPhoto updateUserPhoto when (updateUserPhoto.user_id == id):
                actionOnUpdate((updateUserPhoto as T)!, updatesBase);
                break;

            case UpdateUserStatus updateUserStatus when (updateUserStatus.user_id == id):
                actionOnUpdate((updateUserStatus as T)!, updatesBase);
                break;

            case UpdateUserTyping updateUserTyping when (updateUserTyping.user_id == id):
                actionOnUpdate((updateUserTyping as T)!, updatesBase);
                break;

            case UpdateWebPage updateWebPage when (updateWebPage.webpage.ID == id):
                actionOnUpdate((updateWebPage as T)!, updatesBase);
                break;

            case UpdateWebViewResultSent updateWebViewResultSent when (updateWebViewResultSent.query_id == id):
                actionOnUpdate((updateWebViewResultSent as T)!, updatesBase);
                break;

            case UpdateChannelAvailableMessages updateCam when (updateCam.channel_id == id || updateCam.available_min_id == id):
                actionOnUpdate((updateCam as T)!, updatesBase);
                break;

            case UpdateChannelMessageForwards updateCmF when (updateCmF.id == id || updateCmF.forwards == id || updateCmF.channel_id == id):
                actionOnUpdate((updateCmF as T)!, updatesBase);
                break;
            case UpdateChannelMessageViews updateCmV when (updateCmV.id == id || updateCmV.channel_id == id):
                actionOnUpdate((updateCmV as T)!, updatesBase);
                break;
            case UpdateChannelReadMessagesContents updateCRmC when (updateCRmC.channel_id == id || updateCRmC.messages.Any(p => p == id)):
                actionOnUpdate((updateCRmC as T)!, updatesBase);
                break;
            case UpdateDeleteMessages updateDeleteMessages when (updateDeleteMessages.messages.Any(p => p == id)):
                actionOnUpdate((updateDeleteMessages as T)!, updatesBase);
                break;
            case UpdateBotInlineQuery updateBotInlineQuery when (updateBotInlineQuery.user_id == id || updateBotInlineQuery.query_id == id):
                actionOnUpdate((updateBotInlineQuery as T)!, updatesBase);
                break;
            case UpdateBotInlineSend updateBotInlineSend when (updateBotInlineSend.user_id == id || updateBotInlineSend.msg_id.DcId == id):
                actionOnUpdate((updateBotInlineSend as T)!, updatesBase);
                break;
            case UpdateBotMenuButton updateBotMenuButton when (updateBotMenuButton.bot_id == id):
                actionOnUpdate((updateBotMenuButton as T)!, updatesBase);
                break;
            case UpdateBotPrecheckoutQuery updateBpQ when (updateBpQ.user_id == id || updateBpQ.query_id == id):
                actionOnUpdate((updateBpQ as T)!, updatesBase);
                break;
            case UpdateBotShippingQuery updateBsQ when (updateBsQ.user_id == id || updateBsQ.query_id == id):
                actionOnUpdate((updateBsQ as T)!, updatesBase);
                break;
            case UpdateBotStopped updateBotStopped when (updateBotStopped.user_id == id):
                actionOnUpdate((updateBotStopped as T)!, updatesBase);
                break;
            case UpdateBotWebhookJSONQuery updateBotWebHookJsonQuery when (updateBotWebHookJsonQuery.query_id == id):
                actionOnUpdate((updateBotWebHookJsonQuery as T)!, updatesBase);
                break;
            case UpdateChannel updateChannel when (updateChannel.channel_id == id):
                actionOnUpdate((updateChannel as T)!, updatesBase);
                break;
            case UpdateChannelParticipant updateCp when (updateCp.channel_id == id || updateCp.actor_id == id || updateCp.user_id == id):
                actionOnUpdate((updateCp as T)!, updatesBase);
                break;
            case UpdateChannelTooLong updateChannelTooLong when (updateChannelTooLong.channel_id == id):
                actionOnUpdate((updateChannelTooLong as T)!, updatesBase);
                break;
            case UpdateChannelUserTyping updateCuT when (updateCuT.channel_id == id || updateCuT.from_id.ID == id || updateCuT.top_msg_id == id):
                actionOnUpdate((updateCuT as T)!, updatesBase);
                break;
            case UpdateChat updateChat when (updateChat.chat_id == id):
                actionOnUpdate((updateChat as T)!, updatesBase);
                break;
            case UpdateChatParticipant updateCp when (updateCp.user_id == id || updateCp.user_id == id || updateCp.actor_id == id ||
                                                      updateCp.new_participant.UserId == id || updateCp.prev_participant.UserId == id):
                actionOnUpdate((updateCp as T)!, updatesBase);
                break;
            case UpdateChatParticipants updateChParticipant when (updateChParticipant.participants.ChatId == id ||
                                                                  updateChParticipant.participants.Participants.Any(p => p.UserId == id)):
                actionOnUpdate((updateChParticipant as T)!, updatesBase);
                break;
            case UpdateDialogFilter updateDialogFilter when (updateDialogFilter.id == id):
                actionOnUpdate((updateDialogFilter as T)!, updatesBase);
                break;
            case UpdateDialogFilterOrder updateDialogFilterOrder when (updateDialogFilterOrder.order.Any(p => p == id)):
                actionOnUpdate((updateDialogFilterOrder as T)!, updatesBase);
                break;

        }
    }
}