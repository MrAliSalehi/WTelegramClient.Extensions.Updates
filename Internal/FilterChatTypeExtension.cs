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

            case UpdateDeleteChannelMessages updateDeleteChannelMessages:
                {
                    await client.DoActionForEachDeletedMessage(actionOnUpdate, updateDeleteChannelMessages);
                    break;
                }


                //TODO : move to another part where you can filter by id ..



                //todo : tf i should do with these 2 : ????
                //case UpdateUserName updateUserName:
                //    {
                //        break;
                //    }

                //case UpdateDeleteMessages updateDeleteMessages:
                //    {
                //        break;
                //    }
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
            if (message.IsValidType<TPeer>(out var peer))
                continue;

            await actionOnUpdate(updateDeleteChannelMessages, peer);
        }
    }
}