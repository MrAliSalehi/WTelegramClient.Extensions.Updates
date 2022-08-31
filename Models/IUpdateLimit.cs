using TL;

namespace WTelegramClient.Extensions.Updates.Models;

public interface IUpdateLimit
{
    bool ShouldHandle(Update update);
}