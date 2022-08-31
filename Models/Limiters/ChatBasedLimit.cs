using TL;
using WTelegram;
using WTelegramClient.Extensions.Updates.Internal;

namespace WTelegramClient.Extensions.Updates.Models.Limiters;

public enum InputType
{
    Include,
    Exclude
}

public class ChatBasedLimit : IUpdateLimit
{
    private readonly ChatBase[] _chats;
    private readonly Client _client;
    private readonly InputType _inputType;

    public ChatBasedLimit(InputType inputType, ChatBase[] chats, Client client)
    {
        _inputType = inputType;
        _chats = chats;
        _client = client;
    }

    public ChatBasedLimit(InputType inputType, ChatBase chat, Client client)
    {
        _inputType = inputType;
        _chats = new[] { chat };
        _client = client;
    }

    bool IUpdateLimit.ShouldHandle(Update update)
    {
        var chatIdentifier = TryFindChatId(update);

        return _inputType switch
        {
            InputType.Include => chatIdentifier is not 0 && _chats.Any(p => p.ID == chatIdentifier),
            InputType.Exclude => chatIdentifier is not 0 && _chats.All(p => p.ID != chatIdentifier),
            _ => throw new InvalidDataException(nameof(InputType))
        };
    }

    private long TryFindChatId(Update update)
    {
        long chatId = 0;
        try
        {
            _client.PerformActionBasedOnUpdateTypeAsync<PeerChannel>((_, peer) =>
            {
                chatId = peer.channel_id;
                return Task.CompletedTask;
            }, update);
        }
        catch
        {
            // ignored
        }
        try
        {
            _client.PerformActionBasedOnUpdateTypeAsync<PeerChat>((_, peer) =>
            {
                chatId = peer.chat_id;
                return Task.CompletedTask;
            }, update);
        }
        catch
        {
            //ignored
        }

        try
        {
            _client.PerformActionBasedOnUpdateTypeAsync<PeerUser>((_, peer) =>
            {
                chatId = peer.user_id;
                return Task.CompletedTask;
            }, update);
        }
        catch
        {
            //ignored
        }

        return chatId;
    }
}