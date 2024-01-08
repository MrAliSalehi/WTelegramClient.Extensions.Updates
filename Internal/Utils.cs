namespace WTelegramClient.Extensions.Updates.Internal;

public static class Utils
{
    internal static void WaitUntilCancelIsRequested(this CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
            Thread.Sleep(TimeSpan.FromSeconds(1));
    }
}