using TL;
using WTelegram;
using WTelegramClient.Extensions.Updates.Internal;

namespace WTelegramClient.Extensions.Updates
{
    public static class UpdateExtensions
    {
        /// <summary>
        /// Register An Specific Update Type. Other UpdateTypes will be ignored.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="onUpdate">the action to do when ever an update with specified type received.</param>
        /// <typeparam name="T">The Update Type that you want to listen for.</typeparam>
        public static void RegisterUpdateType<T>(this Client client, Func<T, UpdatesBase?, Task> onUpdate) where T : Update
        {
            client.OnUpdate += async obj =>
            {
                if (obj.IsUpdateBase(out var updatesBase))
                {
                    await updatesBase.FilterUpdatesToPerformAnActionAsync(onUpdate);
                }
            };
        }

        /// <summary>
        /// Register 2 Different Update Type.Others will be ignored. 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="actionOnUpdate">the action to do when ever an update with <b>T1</b> or <b>T2</b> received.</param>
        /// <typeparam name="T1">First update Type that you want to listen for.</typeparam>
        /// <typeparam name="T2">Second update Type that you want to listen for.</typeparam>
        public static void RegisterUpdateType<T1, T2>(this Client client, Func<Update, UpdatesBase?, Task> actionOnUpdate) where T1 : Update where T2 : Update
        {
            client.OnUpdate += async obj =>
            {
                if (!obj.IsUpdateBase(out var updatesBase))
                    return;

                await updatesBase.FilterUpdatesToPerformAnActionAsync<T1>(actionOnUpdate);
                await updatesBase.FilterUpdatesToPerformAnActionAsync<T2>(actionOnUpdate);
            };
        }

        /// <summary>
        /// Register a Chat Type [<see cref="PeerUser"/>,<see cref="PeerChannel"/>,<see cref="PeerChat"/>] for receiving updates.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="actionOnUpdate">the action to do whenever an update with Specified ChatType(<b>TPeer</b>) received.</param>
        /// <typeparam name="TPeer">Chat Type That inherits From <see cref="Peer"/>. </typeparam>
        public static void RegisterChatTypeAsync<TPeer>(this Client client, Func<Update, TPeer, Task> actionOnUpdate) where TPeer : Peer
        {
            client.OnUpdate += async obj =>
            {
                if (!obj.IsUpdateBase(out var updatesBase))
                    return;

                updatesBase.CollectUsersChats(UpdateHelpers.Users, UpdateHelpers.Chats);

                foreach (var update in updatesBase.UpdateList)
                {
                    await client.PerformActionBasedOnUpdateTypeAsync(actionOnUpdate, update);
                }
            };
        }

        /// <summary>
        /// Register an update type for a specific ID, this id can be any identifier inside that update . Note that some update types that has no identifier cannot be handled with this method
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id">id of the chat/user/folder_id/channel_id/query_id or anything else.</param>
        /// <param name="actionOnUpdate">the action to do whenever an update with Specified ID received.</param>
        /// <typeparam name="T">The Update Type that you want to listen for.</typeparam>
        public static void RegisterUpdateWithId<T>(this Client client, long id, Func<T, UpdatesBase, Task> actionOnUpdate) where T : Update, new()
        {
            client.OnUpdate += async obj =>
            {
                if (!obj.IsUpdateBase(out var updatesBase))
                    return;

                await FilterUpdatesByIdExtensions.FilterUpdatesByIdToPerformAnActionAsync(id, actionOnUpdate, updatesBase);
            };
        }
    }
}