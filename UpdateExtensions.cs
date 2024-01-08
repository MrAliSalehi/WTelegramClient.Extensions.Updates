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
            => client.OnUpdate += async updatesBase => await updatesBase.FilterUpdatesToPerformAnActionAsync(onUpdate);

        /// <summary>
        /// same as the <see cref="RegisterUpdateType{T}"/> except that this method WILL block the current thread until the cancellation token is requested
        /// </summary>
        public static void RegisterUpdateTypeBlocking<T>(this Client client, Func<T, UpdatesBase?, Task> onUpdate, CancellationToken ct = default) where T : Update
        {
            RegisterUpdateType(client, onUpdate);
            ct.WaitUntilCancelIsRequested();
        }

        /// <summary>
        /// Register 2 Different Update Type.Others will be ignored. 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="actionOnUpdate">the action to do when ever an update with <b>T1</b> or <b>T2</b> received.</param>
        /// <typeparam name="T1">First update Type that you want to listen for.</typeparam>
        /// <typeparam name="T2">Second update Type that you want to listen for.</typeparam>
        public static void RegisterUpdateType<T1, T2>(this Client client, Func<Update, UpdatesBase?, Task> actionOnUpdate) where T1 : Update where T2 : Update
            => client.OnUpdate += async updatesBase =>
            {
                await updatesBase.FilterUpdatesToPerformAnActionAsync<T1>(actionOnUpdate);
                await updatesBase.FilterUpdatesToPerformAnActionAsync<T2>(actionOnUpdate);
            };

        /// <summary>
        /// same as the <see cref="RegisterUpdateType{T1,T2}"/> except that this method WILL block the current thread until the cancellation token is requested
        /// </summary>
        public static void RegisterUpdateTypeBlocking<T1, T2>(this Client client, Func<Update, UpdatesBase?, Task> onUpdate, CancellationToken ct = default) where T1 : Update where T2 : Update
        {
            RegisterUpdateType<T1, T2>(client, onUpdate);
            ct.WaitUntilCancelIsRequested();
        }

        /// <summary>
        /// Register a Chat Type [<see cref="PeerUser"/>, <see cref="PeerChannel"/>, <see cref="PeerChat"/>] for receiving updates.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="actionOnUpdate">the action to do whenever an update with Specified ChatType(<b>TPeer</b>) received.</param>
        /// <typeparam name="TPeer">Chat Type That inherits From <see cref="Peer"/>. </typeparam>
        public static void RegisterChatType<TPeer>(this Client client, Func<Update, TPeer, Task> actionOnUpdate) where TPeer : Peer
            => client.OnUpdate += async updatesBase =>
            {
                updatesBase.CollectUsersChats(UpdateHelpers.Users, UpdateHelpers.Chats);

                foreach (var update in updatesBase.UpdateList)
                    await client.PerformActionBasedOnUpdateTypeAsync(actionOnUpdate, update);
            };

        /// <summary>
        /// same as the <see cref="RegisterChatType{TPeer}"/> except that this method WILL block the current thread until the cancellation token is requested
        /// </summary>
        public static void RegisterChatTypeBlocking<TPeer>(this Client client, Func<Update, TPeer, Task> actionOnUpdate, CancellationToken ct = default) where TPeer : Peer
        {
            RegisterChatType(client, actionOnUpdate);
            ct.WaitUntilCancelIsRequested();
        }
        /// <summary>
        /// Register an update type for a specific ID, this id can be any identifier inside that update . Note that some update types that has no identifier cannot be handled with this method
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id">id of the chat/user/folder_id/channel_id/query_id or anything else.</param>
        /// <param name="actionOnUpdate">the action to do whenever an update with Specified ID received.</param>
        /// <typeparam name="T">The Update Type that you want to listen for.</typeparam>
        public static void RegisterUpdateWithId<T>(this Client client, long id, Func<T, UpdatesBase, Task> actionOnUpdate) where T : Update, new()
            => client.OnUpdate += async updatesBase => { await FilterUpdatesByIdExtensions.FilterUpdatesByIdToPerformAnActionAsync(id, actionOnUpdate, updatesBase); };

        /// <summary>
        /// same as the <see cref="RegisterUpdateWithId{T}"/> except that this method WILL block the current thread until the cancellation token is requested
        /// </summary>
        public static void RegisterUpdateWithIdBlocking<T>(this Client client, long id, Func<T, UpdatesBase, Task> actionOnUpdate, CancellationToken ct = default) where T : Update, new()
        {
            RegisterUpdateWithId(client, id, actionOnUpdate);
            ct.WaitUntilCancelIsRequested();
        }
    }
}