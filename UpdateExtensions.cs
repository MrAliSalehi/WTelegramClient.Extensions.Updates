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

        public static void RegisterUpdateType<T>(this Client client, Func<T, UpdatesBase?, Task> onUpdate) where T : Update
        {
            client.OnUpdate += async obj =>
            {
                if (obj.IsUpdateBase(out var updatesBase))
                {
                    await Task.Run(() =>
                    {
                        updatesBase.FilterUpdatesToPerformAnAction(onUpdate);
                    });
                }
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

        public static async Task RegisterChatTypeAsync<TPeer>(this Client client, Func<Update, TPeer, Task> actionOnUpdate) where TPeer : Peer
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

                FilterUpdatesByIdExtensions.FilterUpdatesByIdToPerformAnAction(id, actionOnUpdate, updatesBase);

                return Task.CompletedTask;
            };
        }
    }
}