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
                {
                    actionOnUpdate((updateUserTyping as T)!, updatesBase);
                    break;
                }

            case UpdateChatParticipants updateChatParticipants
                when UpdateHelpers.IsChatIdOrAnyParticipantMatch(id, updateChatParticipants):
                {
                    actionOnUpdate((updateChatParticipants as T)!, updatesBase);
                    break;
                }

            case UpdateUserStatus updateUserStatus when updateUserStatus.user_id == id:
                {
                    actionOnUpdate((updateUserStatus as T)!, updatesBase);
                    break;
                }

            case UpdateUserPhoto updateUserPhoto when updateUserPhoto.user_id == id:
                {
                    actionOnUpdate((updateUserPhoto as T)!, updatesBase);
                    break;
                }
        }
    }
}