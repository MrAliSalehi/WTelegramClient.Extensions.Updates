namespace WTelegramClient.Extensions.Updates.Internal;

internal static class FilterUpdatesByIdExtensions
{
    public static async Task FilterUpdatesByIdToPerformAnActionAsync<T>(long id, Func<T, UpdatesBase, Task> actionOnUpdate, UpdatesBase updatesBase) where T : Update, new()
    {
        foreach (var update in updatesBase.UpdateList)
        {
            if (!update.IsValidUpdateType<T>())
                continue;

            await update.FilterUpdateByIdAsync(id, actionOnUpdate, updatesBase);
        }
    }
    private static bool When<T>(T update, long id) where T : Update, new() => update switch
    {
        UpdateUserTyping updateUserTyping when updateUserTyping.user_id == id                                                                                                                                      => true,
        UpdateUserPhone updateUserPhone when (updateUserPhone.user_id == id)                                                                                                                                       => true,
        UpdateUserStatus updateUserStatus when (updateUserStatus.user_id == id)                                                                                                                                    => true,
        UpdateUserTyping updateUserTyping when (updateUserTyping.user_id == id)                                                                                                                                    => true,
        UpdateUserStatus updateUserStatus when updateUserStatus.user_id == id                                                                                                                                      => true,
        UpdateEncryptedChatTyping updateEncryptedChatTyping when updateEncryptedChatTyping.chat_id == id                                                                                                           => true,
        UpdateEncryptedMessagesRead updateEncryptedMessagesRead when updateEncryptedMessagesRead.chat_id == id                                                                                                     => true,
        UpdateEncryption updateEncryption when updateEncryption.chat.ID == id                                                                                                                                      => true,
        UpdateGroupCall updateGroupCall when updateGroupCall.call.ID == id || updateGroupCall.chat_id == id                                                                                                        => true,
        UpdateGroupCallParticipants updateGroupCallParticipants when updateGroupCallParticipants.call.id == id                                                                                                     => true,
        UpdateInlineBotCallbackQuery updateInlineBotCallbackQuery when updateInlineBotCallbackQuery.user_id == id || updateInlineBotCallbackQuery.msg_id.DcId == id || updateInlineBotCallbackQuery.query_id == id => true,
        UpdateMessagePoll updateMessagePoll when updateMessagePoll.poll_id == id                                                                                                                                   => true,
        UpdateMessagePollVote updateMessagePollVote when updateMessagePollVote.poll_id == id || updateMessagePollVote.poll_id == id                                                                                => true,
        UpdateNewEncryptedMessage updateNewEncryptedMessage when updateNewEncryptedMessage.message.ChatId == id                                                                                                    => true,
        UpdateNewStickerSet updateNewStickerSet when updateNewStickerSet.stickerset.set.id == id || updateNewStickerSet.stickerset.documents.Any(p => p.ID == id)                                                  => true,
        UpdatePhoneCall updatePhoneCall when updatePhoneCall.phone_call.ID == id                                                                                                                                   => true,
        UpdatePhoneCallSignalingData updatePhoneCallSignalingData when updatePhoneCallSignalingData.phone_call_id == id                                                                                            => true,
        UpdatePinnedChannelMessages updatePinnedChannelMessages when updatePinnedChannelMessages.channel_id == id || updatePinnedChannelMessages.messages.Any(p => p == id)                                        => true,
        UpdatePinnedDialogs updatePinnedDialogs when (updatePinnedDialogs.folder_id == id)                                                                                                                         => true,
        UpdateReadMessagesContents updateReadMessagesContents when (updateReadMessagesContents.messages.Any(p => p == id))                                                                                         => true,
        UpdateStickerSetsOrder updateStickerSetsOrder when (updateStickerSetsOrder.order.Any(p => p == id))                                                                                                        => true,
        UpdateTheme updateTheme when (updateTheme.theme.document.ID == id || updateTheme.theme.id == id)                                                                                                           => true,
        UpdateWebPage updateWebPage when (updateWebPage.webpage.ID == id)                                                                                                                                          => true,
        UpdateWebViewResultSent updateWebViewResultSent when (updateWebViewResultSent.query_id == id)                                                                                                              => true,
        UpdateDeleteMessages updateDeleteMessages when (updateDeleteMessages.messages.Any(p => p == id))                                                                                                           => true,
        UpdateChat updateChat when (updateChat.chat_id == id)                                                                                                                                                      => true,
        UpdateChatParticipant updateCp when (updateCp.user_id == id || updateCp.user_id == id || updateCp.actor_id == id || updateCp.new_participant.UserId == id || updateCp.prev_participant.UserId == id)       => true,
        UpdateChatParticipants updateChatParticipants when UpdateHelpers.IsChatIdOrAnyParticipantMatch(id, updateChatParticipants)                                                                                 => true,
        UpdateChatParticipants updateChParticipant when updateChParticipant.participants.ChatId == id || updateChParticipant.participants.Participants.Any(p => p.UserId == id)                                    => true,
        UpdateDialogFilter updateDialogFilter when updateDialogFilter.id == id                                                                                                                                     => true,
        UpdateDialogFilterOrder updateDialogFilterOrder when updateDialogFilterOrder.order.Any(p => p == id)                                                                                                       => true,
        _
            => MaybeChannel(update, id)
    };
    private static bool MaybeChannel<T>(T update, long id) where T : Update, new()
    {
        return update switch
        {
            UpdateChannelAvailableMessages updateCam when (updateCam.channel_id == id || updateCam.available_min_id == id)                                                                 => true,
            UpdateChannelMessageForwards updateCmF when (updateCmF.id == id || updateCmF.forwards == id || updateCmF.channel_id == id)                                                     => true,
            UpdateChannelMessageViews updateCmV when (updateCmV.id == id || updateCmV.channel_id == id)                                                                                    => true,
            UpdateChannelReadMessagesContents updateCRmC when (updateCRmC.channel_id == id || updateCRmC.messages.Any(p => p == id))                                                       => true,
            UpdateChannel updateChannel when (updateChannel.channel_id == id)                                                                                                              => true,
            UpdateChannelParticipant updateCp when (updateCp.channel_id == id || updateCp.actor_id == id || updateCp.user_id == id)                                                        => true,
            UpdateChannelTooLong updateChannelTooLong when (updateChannelTooLong.channel_id == id)                                                                                         => true,
            UpdateChannelUserTyping updateCuT when (updateCuT.channel_id == id || updateCuT.from_id.ID == id || updateCuT.top_msg_id == id)                                                => true,
            UpdateReadChannelDiscussionInbox updateRcDi when (updateRcDi.channel_id == id || updateRcDi.broadcast_id == id || updateRcDi.top_msg_id == id || updateRcDi.read_max_id == id) => true,
            UpdateReadChannelDiscussionOutbox updateRcDo when (updateRcDo.channel_id == id || updateRcDo.read_max_id == id || updateRcDo.top_msg_id == id)                                 => true,
            UpdateReadChannelInbox updateRci when (updateRci.channel_id == id || updateRci.folder_id == id || updateRci.max_id == id)                                                      => true,
            UpdateReadChannelOutbox updateReadChannelOutbox when (updateReadChannelOutbox.channel_id == id || updateReadChannelOutbox.max_id == id)                                        => true,
            _                                                                                                                                                                              => MaybeBot(update, id),
        };
    }
    private static bool MaybeBot<T>(T update, long id) where T : Update, new() => update switch
    {
        UpdateBotInlineQuery updateBotInlineQuery when (updateBotInlineQuery.user_id == id || updateBotInlineQuery.query_id == id) => true,
        UpdateBotInlineSend updateBotInlineSend when (updateBotInlineSend.user_id == id || updateBotInlineSend.msg_id.DcId == id)  => true,
        UpdateBotMenuButton updateBotMenuButton when (updateBotMenuButton.bot_id == id)                                            => true,
        UpdateBotPrecheckoutQuery updateBpQ when (updateBpQ.user_id == id || updateBpQ.query_id == id)                             => true,
        UpdateBotShippingQuery updateBsQ when (updateBsQ.user_id == id || updateBsQ.query_id == id)                                => true,
        UpdateBotStopped updateBotStopped when (updateBotStopped.user_id == id)                                                    => true,
        UpdateBotWebhookJSONQuery updateBotWebHookJsonQuery when (updateBotWebHookJsonQuery.query_id == id)                        => true,
        _                                                                                                                          => false
    };
    private static Task FilterUpdateByIdAsync<T>(this Update update, long id, Func<T, UpdatesBase, Task> actionOnUpdate, UpdatesBase updatesBase) where T : Update, new()
    {
        return update switch
        {
            UpdateUserTyping updateUserTyping when When(updateUserTyping, id)                                     => actionOnUpdate((updateUserTyping as T)!, updatesBase),
            UpdateChatParticipants updateChatParticipants when When(updateChatParticipants, id)                   => actionOnUpdate((updateChatParticipants as T)!, updatesBase),
            UpdateUserStatus updateUserStatus when When(updateUserStatus, id)                                     => actionOnUpdate((updateUserStatus as T)!, updatesBase),
            UpdateEncryptedChatTyping updateEncryptedChatTyping when When(updateEncryptedChatTyping, id)          => actionOnUpdate((updateEncryptedChatTyping as T)!, updatesBase),
            UpdateEncryptedMessagesRead updateEncryptedMessagesRead when When(updateEncryptedMessagesRead, id)    => actionOnUpdate((updateEncryptedMessagesRead as T)!, updatesBase),
            UpdateEncryption updateEncryption when When(updateEncryption, id)                                     => actionOnUpdate((updateEncryption as T)!, updatesBase),
            UpdateGroupCall updateGroupCall when When(updateGroupCall, id)                                        => actionOnUpdate((updateGroupCall as T)!, updatesBase),
            UpdateGroupCallParticipants updateGroupCallParticipants when When(updateGroupCallParticipants, id)    => actionOnUpdate((updateGroupCallParticipants as T)!, updatesBase),
            UpdateInlineBotCallbackQuery updateInlineBotCallbackQuery when When(updateInlineBotCallbackQuery, id) => actionOnUpdate((updateInlineBotCallbackQuery as T)!, updatesBase),
            UpdateMessagePoll updateMessagePoll when When(updateMessagePoll, id)                                  => actionOnUpdate((updateMessagePoll as T)!, updatesBase),
            UpdateMessagePollVote updateMessagePollVote when When(updateMessagePollVote, id)                      => actionOnUpdate((updateMessagePollVote as T)!, updatesBase),
            UpdateNewEncryptedMessage updateNewEncryptedMessage when When(updateNewEncryptedMessage, id)          => actionOnUpdate((updateNewEncryptedMessage as T)!, updatesBase),
            UpdateNewStickerSet updateNewStickerSet when When(updateNewStickerSet, id)                            => actionOnUpdate((updateNewStickerSet as T)!, updatesBase),
            UpdatePhoneCall updatePhoneCall when When(updatePhoneCall, id)                                        => actionOnUpdate((updatePhoneCall as T)!, updatesBase),
            UpdatePhoneCallSignalingData updatePhoneCallSignalingData when When(updatePhoneCallSignalingData, id) => actionOnUpdate((updatePhoneCallSignalingData as T)!, updatesBase),
            UpdatePinnedChannelMessages updatePinnedChannelMessages when When(updatePinnedChannelMessages, id)    => actionOnUpdate((updatePinnedChannelMessages as T)!, updatesBase),
            UpdatePinnedDialogs updatePinnedDialogs when When(updatePinnedDialogs, id)                            => actionOnUpdate((updatePinnedDialogs as T)!, updatesBase),
            UpdateReadChannelDiscussionInbox updateRcDi when When(updateRcDi, id)                                 => actionOnUpdate((updateRcDi as T)!, updatesBase),
            UpdateReadChannelDiscussionOutbox updateRcDo when When(updateRcDo, id)                                => actionOnUpdate((updateRcDo as T)!, updatesBase),
            UpdateReadChannelInbox updateRci when When(updateRci, id)                                             => actionOnUpdate((updateRci as T)!, updatesBase),
            UpdateReadChannelOutbox updateReadChannelOutbox when When(updateReadChannelOutbox, id)                => actionOnUpdate((updateReadChannelOutbox as T)!, updatesBase),
            UpdateReadMessagesContents updateReadMessagesContents when When(updateReadMessagesContents, id)       => actionOnUpdate((updateReadMessagesContents as T)!, updatesBase),
            UpdateStickerSetsOrder updateStickerSetsOrder when When(updateStickerSetsOrder, id)                   => actionOnUpdate((updateStickerSetsOrder as T)!, updatesBase),
            UpdateTheme updateTheme when When(updateTheme, id)                                                    => actionOnUpdate((updateTheme as T)!, updatesBase),
            UpdateUserPhone updateUserPhone when When(updateUserPhone, id)                                        => actionOnUpdate((updateUserPhone as T)!, updatesBase),
            UpdateUserStatus updateUserStatus when When(updateUserStatus, id)                                     => actionOnUpdate((updateUserStatus as T)!, updatesBase),
            UpdateUserTyping updateUserTyping when When(updateUserTyping, id)                                     => actionOnUpdate((updateUserTyping as T)!, updatesBase),
            UpdateWebPage updateWebPage when When(updateWebPage, id)                                              => actionOnUpdate((updateWebPage as T)!, updatesBase),
            UpdateWebViewResultSent updateWebViewResultSent when When(updateWebViewResultSent, id)                => actionOnUpdate((updateWebViewResultSent as T)!, updatesBase),
            UpdateChannelAvailableMessages updateCam when When(updateCam, id)                                     => actionOnUpdate((updateCam as T)!, updatesBase),
            UpdateChannelMessageForwards updateCmF when When(updateCmF, id)                                       => actionOnUpdate((updateCmF as T)!, updatesBase),
            UpdateChannelMessageViews updateCmV when When(updateCmV, id)                                          => actionOnUpdate((updateCmV as T)!, updatesBase),
            UpdateChannelReadMessagesContents updateCRmC when When(updateCRmC, id)                                => actionOnUpdate((updateCRmC as T)!, updatesBase),
            UpdateDeleteMessages updateDeleteMessages when When(updateDeleteMessages, id)                         => actionOnUpdate((updateDeleteMessages as T)!, updatesBase),
            UpdateBotInlineQuery updateBotInlineQuery when When(updateBotInlineQuery, id)                         => actionOnUpdate((updateBotInlineQuery as T)!, updatesBase),
            UpdateBotInlineSend updateBotInlineSend when When(updateBotInlineSend, id)                            => actionOnUpdate((updateBotInlineSend as T)!, updatesBase),
            UpdateBotMenuButton updateBotMenuButton when When(updateBotMenuButton, id)                            => actionOnUpdate((updateBotMenuButton as T)!, updatesBase),
            UpdateBotPrecheckoutQuery updateBpQ when When(updateBpQ, id)                                          => actionOnUpdate((updateBpQ as T)!, updatesBase),
            UpdateBotShippingQuery updateBsQ when When(updateBsQ, id)                                             => actionOnUpdate((updateBsQ as T)!, updatesBase),
            UpdateBotStopped updateBotStopped when When(updateBotStopped, id)                                     => actionOnUpdate((updateBotStopped as T)!, updatesBase),
            UpdateBotWebhookJSONQuery updateBotWebHookJsonQuery when When(updateBotWebHookJsonQuery, id)          => actionOnUpdate((updateBotWebHookJsonQuery as T)!, updatesBase),
            UpdateChannel updateChannel when When(updateChannel, id)                                              => actionOnUpdate((updateChannel as T)!, updatesBase),
            UpdateChannelParticipant updateCp when When(updateCp, id)                                             => actionOnUpdate((updateCp as T)!, updatesBase),
            UpdateChannelTooLong updateChannelTooLong when When(updateChannelTooLong, id)                         => actionOnUpdate((updateChannelTooLong as T)!, updatesBase),
            UpdateChannelUserTyping updateCuT when When(updateCuT, id)                                            => actionOnUpdate((updateCuT as T)!, updatesBase),
            UpdateChat updateChat when When(updateChat, id)                                                       => actionOnUpdate((updateChat as T)!, updatesBase),
            UpdateChatParticipant updateCp when When(updateCp, id)                                                => actionOnUpdate((updateCp as T)!, updatesBase),
            UpdateChatParticipants updateChParticipant when When(updateChParticipant, id)                         => actionOnUpdate((updateChParticipant as T)!, updatesBase),
            UpdateDialogFilter updateDialogFilter when When(updateDialogFilter, id)                               => actionOnUpdate((updateDialogFilter as T)!, updatesBase),
            UpdateDialogFilterOrder updateDialogFilterOrder when When(updateDialogFilterOrder, id)                => actionOnUpdate((updateDialogFilterOrder as T)!, updatesBase),
            _                                                                                                     => throw new NotSupportedException($"This Type Is Not Supported {update.GetType()} - {typeof(T)}")
        };
    }
}