using WTelegramClient.Extensions.Updates.Models.Limiters;

namespace WTelegramClient.Extensions.Updates.Models;

public class RateLimit
{
    public IUpdateLimit Limiter { get; set; } = new DefaultLimiter();
}