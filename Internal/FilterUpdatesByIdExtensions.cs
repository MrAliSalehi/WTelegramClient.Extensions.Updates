using TL;
using WTelegramClient.Extensions.Updates.Models;

namespace WTelegramClient.Extensions.Updates.Internal;

internal static class FilterUpdatesByIdExtensions
{
    public static async Task FilterUpdatesByIdToPerformAnActionAsync<T>(long id, Func<T, UpdatesBase, Task> actionOnUpdate, UpdatesBase updatesBase) where T : Update, new()
    {
        foreach (var update in updatesBase.UpdateList)
        {
            if (!update.IsValidUpdateType<T>())
                continue;

            if (UpdateConfigurations.RateLimit.Limiter.ShouldHandle(update))
                await update.FilterUpdateByIdAsync(id, actionOnUpdate, updatesBase);
        }
    }

    private static Task FilterUpdateByIdAsync<T>(this Update update, long id, Func<T, UpdatesBase, Task> actionOnUpdate, UpdatesBase updatesBase) where T : Update, new()
    {
        return update switch
        {
            UpdateUserTyping updateUserTyping when updateUserTyping.user_id == id => actionOnUpdate((updateUserTyping as T)!, updatesBase),
            UpdateChatParticipants updateChatParticipants when UpdateHelpers.IsChatIdOrAnyParticipantMatch(id, updateChatParticipants) => actionOnUpdate((updateChatParticipants as T)!, updatesBase),
            UpdateUserStatus updateUserStatus when updateUserStatus.user_id == id => actionOnUpdate((updateUserStatus as T)!, updatesBase),
            UpdateUserPhoto updateUserPhoto when updateUserPhoto.user_id == id => actionOnUpdate((updateUserPhoto as T)!, updatesBase),
            UpdateEncryptedChatTyping updateEncryptedChatTyping when (updateEncryptedChatTyping.chat_id == id) => actionOnUpdate((updateEncryptedChatTyping as T)!, updatesBase),
            UpdateEncryptedMessagesRead updateEncryptedMessagesRead when (updateEncryptedMessagesRead.chat_id == id) => actionOnUpdate((updateEncryptedMessagesRead as T)!, updatesBase),
            UpdateEncryption updateEncryption when (updateEncryption.chat.ID == id) => actionOnUpdate((updateEncryption as T)!, updatesBase),
            UpdateGroupCall updateGroupCall when (updateGroupCall.call.ID == id || updateGroupCall.chat_id == id) => actionOnUpdate((updateGroupCall as T)!, updatesBase),
            UpdateGroupCallParticipants updateGroupCallParticipants when (updateGroupCallParticipants.call.id == id) => actionOnUpdate((updateGroupCallParticipants as T)!, updatesBase),
            UpdateInlineBotCallbackQuery updateInlineBotCallbackQuery when (updateInlineBotCallbackQuery.user_id == id || updateInlineBotCallbackQuery.msg_id.DcId == id || updateInlineBotCallbackQuery.query_id == id) => actionOnUpdate((updateInlineBotCallbackQuery as T)!, updatesBase),
            UpdateMessagePoll updateMessagePoll when (updateMessagePoll.poll_id == id) => actionOnUpdate((updateMessagePoll as T)!, updatesBase),
            UpdateMessagePollVote updateMessagePollVote when (updateMessagePollVote.user_id == id || updateMessagePollVote.poll_id == id) => actionOnUpdate((updateMessagePollVote as T)!, updatesBase),
            UpdateNewEncryptedMessage updateNewEncryptedMessage when (updateNewEncryptedMessage.message.ChatId == id) => actionOnUpdate((updateNewEncryptedMessage as T)!, updatesBase),
            UpdateNewStickerSet updateNewStickerSet when (updateNewStickerSet.stickerset.set.id == id || updateNewStickerSet.stickerset.documents.Any(p => p.ID == id)) => actionOnUpdate((updateNewStickerSet as T)!, updatesBase),
            UpdatePhoneCall updatePhoneCall when (updatePhoneCall.phone_call.ID == id) => actionOnUpdate((updatePhoneCall as T)!, updatesBase),
            UpdatePhoneCallSignalingData updatePhoneCallSignalingData when (updatePhoneCallSignalingData.phone_call_id == id) => actionOnUpdate((updatePhoneCallSignalingData as T)!, updatesBase),
            UpdatePinnedChannelMessages updatePinnedChannelMessages when (updatePinnedChannelMessages.channel_id == id || updatePinnedChannelMessages.messages.Any(p => p == id)) => actionOnUpdate((updatePinnedChannelMessages as T)!, updatesBase),
            UpdatePinnedDialogs updatePinnedDialogs when (updatePinnedDialogs.folder_id == id) => actionOnUpdate((updatePinnedDialogs as T)!, updatesBase),
            UpdateReadChannelDiscussionInbox updateRcDi when (updateRcDi.channel_id == id || updateRcDi.broadcast_id == id || updateRcDi.top_msg_id == id || updateRcDi.read_max_id == id) => actionOnUpdate((updateRcDi as T)!, updatesBase),
            UpdateReadChannelDiscussionOutbox updateRcDo when (updateRcDo.channel_id == id || updateRcDo.read_max_id == id || updateRcDo.top_msg_id == id) => actionOnUpdate((updateRcDo as T)!, updatesBase),
            UpdateReadChannelInbox updateRci when (updateRci.channel_id == id || updateRci.folder_id == id || updateRci.max_id == id) => actionOnUpdate((updateRci as T)!, updatesBase),
            UpdateReadChannelOutbox updateReadChannelOutbox when (updateReadChannelOutbox.channel_id == id || updateReadChannelOutbox.max_id == id) => actionOnUpdate((updateReadChannelOutbox as T)!, updatesBase),
            UpdateReadMessagesContents updateReadMessagesContents when (updateReadMessagesContents.messages.Any(p => p == id)) => actionOnUpdate((updateReadMessagesContents as T)!, updatesBase),
            UpdateStickerSetsOrder updateStickerSetsOrder when (updateStickerSetsOrder.order.Any(p => p == id)) => actionOnUpdate((updateStickerSetsOrder as T)!, updatesBase),
            UpdateTheme updateTheme when (updateTheme.theme.document.ID == id || updateTheme.theme.id == id) => actionOnUpdate((updateTheme as T)!, updatesBase),
            UpdateUserPhone updateUserPhone when (updateUserPhone.user_id == id) => actionOnUpdate((updateUserPhone as T)!, updatesBase),
            UpdateUserPhoto updateUserPhoto when (updateUserPhoto.user_id == id) => actionOnUpdate((updateUserPhoto as T)!, updatesBase),
            UpdateUserStatus updateUserStatus when (updateUserStatus.user_id == id) => actionOnUpdate((updateUserStatus as T)!, updatesBase),
            UpdateUserTyping updateUserTyping when (updateUserTyping.user_id == id) => actionOnUpdate((updateUserTyping as T)!, updatesBase),
            UpdateWebPage updateWebPage when (updateWebPage.webpage.ID == id) => actionOnUpdate((updateWebPage as T)!, updatesBase),
            UpdateWebViewResultSent updateWebViewResultSent when (updateWebViewResultSent.query_id == id) => actionOnUpdate((updateWebViewResultSent as T)!, updatesBase),
            UpdateChannelAvailableMessages updateCam when (updateCam.channel_id == id || updateCam.available_min_id == id) => actionOnUpdate((updateCam as T)!, updatesBase),
            UpdateChannelMessageForwards updateCmF when (updateCmF.id == id || updateCmF.forwards == id || updateCmF.channel_id == id) => actionOnUpdate((updateCmF as T)!, updatesBase),
            UpdateChannelMessageViews updateCmV when (updateCmV.id == id || updateCmV.channel_id == id) => actionOnUpdate((updateCmV as T)!, updatesBase),
            UpdateChannelReadMessagesContents updateCRmC when (updateCRmC.channel_id == id || updateCRmC.messages.Any(p => p == id)) => actionOnUpdate((updateCRmC as T)!, updatesBase),
            UpdateDeleteMessages updateDeleteMessages when (updateDeleteMessages.messages.Any(p => p == id)) => actionOnUpdate((updateDeleteMessages as T)!, updatesBase),
            UpdateBotInlineQuery updateBotInlineQuery when (updateBotInlineQuery.user_id == id || updateBotInlineQuery.query_id == id) => actionOnUpdate((updateBotInlineQuery as T)!, updatesBase),
            UpdateBotInlineSend updateBotInlineSend when (updateBotInlineSend.user_id == id || updateBotInlineSend.msg_id.DcId == id) => actionOnUpdate((updateBotInlineSend as T)!, updatesBase),
            UpdateBotMenuButton updateBotMenuButton when (updateBotMenuButton.bot_id == id) => actionOnUpdate((updateBotMenuButton as T)!, updatesBase),
            UpdateBotPrecheckoutQuery updateBpQ when (updateBpQ.user_id == id || updateBpQ.query_id == id) => actionOnUpdate((updateBpQ as T)!, updatesBase),
            UpdateBotShippingQuery updateBsQ when (updateBsQ.user_id == id || updateBsQ.query_id == id) => actionOnUpdate((updateBsQ as T)!, updatesBase),
            UpdateBotStopped updateBotStopped when (updateBotStopped.user_id == id) => actionOnUpdate((updateBotStopped as T)!, updatesBase),
            UpdateBotWebhookJSONQuery updateBotWebHookJsonQuery when (updateBotWebHookJsonQuery.query_id == id) => actionOnUpdate((updateBotWebHookJsonQuery as T)!, updatesBase),
            UpdateChannel updateChannel when (updateChannel.channel_id == id) => actionOnUpdate((updateChannel as T)!, updatesBase),
            UpdateChannelParticipant updateCp when (updateCp.channel_id == id || updateCp.actor_id == id || updateCp.user_id == id) => actionOnUpdate((updateCp as T)!, updatesBase),
            UpdateChannelTooLong updateChannelTooLong when (updateChannelTooLong.channel_id == id) => actionOnUpdate((updateChannelTooLong as T)!, updatesBase),
            UpdateChannelUserTyping updateCuT when (updateCuT.channel_id == id || updateCuT.from_id.ID == id || updateCuT.top_msg_id == id) => actionOnUpdate((updateCuT as T)!, updatesBase),
            UpdateChat updateChat when (updateChat.chat_id == id) => actionOnUpdate((updateChat as T)!, updatesBase),
            UpdateChatParticipant updateCp when (updateCp.user_id == id || updateCp.user_id == id || updateCp.actor_id == id || updateCp.new_participant.UserId == id || updateCp.prev_participant.UserId == id) => actionOnUpdate((updateCp as T)!, updatesBase),
            UpdateChatParticipants updateChParticipant when (updateChParticipant.participants.ChatId == id || updateChParticipant.participants.Participants.Any(p => p.UserId == id)) => actionOnUpdate((updateChParticipant as T)!, updatesBase),
            UpdateDialogFilter updateDialogFilter when (updateDialogFilter.id == id) => actionOnUpdate((updateDialogFilter as T)!, updatesBase),
            UpdateDialogFilterOrder updateDialogFilterOrder when (updateDialogFilterOrder.order.Any(p => p == id)) => actionOnUpdate((updateDialogFilterOrder as T)!, updatesBase),
            _ => throw new NotSupportedException($"This Type Is Not Supported {update.GetType()} - {typeof(T)}")
        };
    }
}