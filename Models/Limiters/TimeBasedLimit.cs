using TL;

namespace WTelegramClient.Extensions.Updates.Models.Limiters;

public class TimeBasedLimit : IUpdateLimit
{
    public bool ShouldHandle(Update update) => false;
}