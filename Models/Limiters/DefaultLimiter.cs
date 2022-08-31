using TL;

namespace WTelegramClient.Extensions.Updates.Models.Limiters;

public class DefaultLimiter : IUpdateLimit
{
    public bool ShouldHandle(Update update) => true;
}