﻿using TL;

namespace WTelegramClient.Extensions.Updates.Internal;

internal static class FilterUpdateType
{
    public static void FilterUpdatesToPerformAnAction<T>(this UpdatesBase updatesBase, Action<T, UpdatesBase?> actionOnUpdate) where T : Update
    {
        foreach (var update in updatesBase.UpdateList.OfType<T>())
        {
            actionOnUpdate(update, updatesBase);
        }
    }

    public static void FilterUpdatesToPerformAnAction<T>(this UpdatesBase updatesBase, Func<T, UpdatesBase?, Task> onUpdate) where T : Update
    {
        foreach (var update in updatesBase.UpdateList.OfType<T>())
        {
            onUpdate(update, updatesBase);
        }
    }
}