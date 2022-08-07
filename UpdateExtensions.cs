using TL;
using WTelegram;
using WTelegramClient.Extensions.Updates.Internal;

namespace WTelegramClient.Extensions.Updates
{
    public static class UpdateExtensions
    {
        public static void RegisterUpdateType<T>(this Client client, Action<T, UpdatesBase?> actionOnUpdate) where T : Update
        {
            client.OnUpdate += obj =>
            {
                if (obj.IsUpdateBase(out var updatesBase))
                    updatesBase.FilterUpdatesToPerformAnAction(actionOnUpdate);

                return Task.CompletedTask;

            };
        }

        public static void RegisterUpdateType<T1, T2>(this Client client, Action<Update, UpdatesBase?> actionOnUpdate)
            where T1 : Update
            where T2 : Update
        {
            client.OnUpdate += obj =>
            {
                if (!obj.IsUpdateBase(out var updatesBase))
                    return Task.CompletedTask;

                updatesBase.FilterUpdatesToPerformAnAction<T1>(actionOnUpdate);
                updatesBase.FilterUpdatesToPerformAnAction<T2>(actionOnUpdate);

                return Task.CompletedTask;

            };
        }

        public static async Task RegisterChatType<TPeer>(this Client client, Func<Update, TPeer, Task> actionOnUpdate) where TPeer : Peer
        {
            await Task.Run(() =>
            {
                client.FilterByChatType(actionOnUpdate);
            });

        }

        public static void RegisterUpdateWithId<T>(this Client client, long id, Action<T, UpdatesBase> actionOnUpdate)
        where T : Update, new()
        {
            client.OnUpdate += obj =>
            {
                if (!obj.IsUpdateBase(out var updatesBase))
                    return Task.CompletedTask;

                foreach (var update in updatesBase.UpdateList)
                {
                    if (!update.IsValidUpdateType<T>())
                        continue;

                    switch (update)
                    {
                        case UpdateUserTyping updateUserTyping when (updateUserTyping.user_id == id):
                            {
                                actionOnUpdate((updateUserTyping as T)!, updatesBase);
                                break;
                            }

                        case UpdateChatParticipants updateChatParticipants when (UpdateHelpers.IsChatIdOrAnyParticipantMatch(id, updateChatParticipants)):
                            {
                                actionOnUpdate((updateChatParticipants as T)!, updatesBase);
                                break;
                            }

                        case UpdateUserStatus updateUserStatus when (updateUserStatus.user_id == id):
                            {
                                actionOnUpdate((updateUserStatus as T)!, updatesBase);
                                break;
                            }

                        case UpdateUserPhoto updateUserPhoto when (updateUserPhoto.user_id == id):
                            {
                                actionOnUpdate((updateUserPhoto as T)!, updatesBase);
                                break;
                            }
                    }
                }

                return Task.CompletedTask;
            };
        }



        private static void FilterUpdatesToPerformAnAction<T>(this UpdatesBase updatesBase, Action<T, UpdatesBase?> actionOnUpdate) where T : Update
        {
            foreach (var update in updatesBase.UpdateList.OfType<T>())
            {
                actionOnUpdate(update, updatesBase);
            }
        }
    }
}