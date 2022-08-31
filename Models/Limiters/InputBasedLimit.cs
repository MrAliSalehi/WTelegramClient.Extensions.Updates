using TL;

namespace WTelegramClient.Extensions.Updates.Models.Limiters;

public class InputBasedLimit : IUpdateLimit
{
    public bool ShouldHandle(Update update) => false;
}