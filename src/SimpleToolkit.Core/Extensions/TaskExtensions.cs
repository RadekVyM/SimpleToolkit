using System.Runtime.CompilerServices;

namespace SimpleToolkit.Core;

internal static class TaskExtensions
{
#if !WEBVIEW2_MAUI
    public static async void FireAndForget(this Task task, Action<Exception> errorCallback = null)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            errorCallback?.Invoke(ex);
#if DEBUG
            throw;
#endif
        }
    }

    public static void FireAndForget<T>(this Task task, T viewHandler, [CallerMemberName] string callerName = null)
        where T : IElementHandler
    {
        task.FireAndForget();
    }
#endif
}