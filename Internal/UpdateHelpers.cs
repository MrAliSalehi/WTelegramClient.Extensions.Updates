using TL;
using WTelegram;

namespace WTelegramClient.Extensions.Updates.Internal;

internal static class UpdateHelpers
{
    internal static readonly Dictionary<long, ChatBase> Chats = new();

    internal static readonly Dictionary<long, User> Users = new();

    public static bool IsChatIdOrAnyParticipantMatch(long id, UpdateChatParticipants updateChatParticipants)
    {
        return updateChatParticipants.participants.ChatId == id || updateChatParticipants.participants.Participants.Any();
    }

    internal static InputMessage[] ToInputMessageId(this IReadOnlyList<int> ids)
    {
        var outputArray = new InputMessage[ids.Count];

        for (var indexer = 0; indexer < ids.Count; indexer++)
        {
            outputArray[indexer] = ids[indexer];

        }
        return outputArray;
    }

    internal static bool IsValidType<TPeer>(this MessageBase message, out TPeer peer) where TPeer : Peer
    {
        if (message.Peer is TPeer validPeer)
        {
            peer = validPeer;
            return true;
        }

        peer = default!;
        return false;
    }

    internal static bool IsUpdateBase(this IObject obj, out UpdatesBase updateBase)
    {
        if (obj is UpdatesBase updates)
        {
            updateBase = updates;
            return true;
        }

        updateBase = default!;
        return false;
    }

    internal static async ValueTask<TChatType?> GetChatAsync<TChatType>(Client client, long channelId) where TChatType : class, new()
    {
        var isAlreadyExists = UpdateHelpers.Chats.TryGetValue(channelId, out var chatBase);
        TChatType? channel;

        if (isAlreadyExists)
            channel = chatBase as TChatType;
        else
        {
            var chats = await client.Messages_GetAllChats();

            var canFindDialogs = chats.chats.TryGetValue(channelId, out var chat);
            if (!canFindDialogs)
                throw new ArgumentNullException(
                    $"Cant Find The Required Chat : {channelId}");
            channel = chat as TChatType;
        }

        return channel;
    }
}