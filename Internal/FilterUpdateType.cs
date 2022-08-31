using TL;
using WTelegramClient.Extensions.Updates.Models;

namespace WTelegramClient.Extensions.Updates.Internal;

internal static class FilterUpdateType
{
    public static async Task FilterUpdatesToPerformAnActionAsync<T>(this UpdatesBase updatesBase, Func<T, UpdatesBase?, Task> onUpdate) where T : Update
    {
        foreach (var update in updatesBase.UpdateList.OfType<T>())
        {
            if (UpdateConfigurations.RateLimit.Limiter.ShouldHandle(update))
                await onUpdate(update, updatesBase);
        }
    }
}