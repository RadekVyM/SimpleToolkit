#if ANDROID
using Android.Graphics;
using Android.Widget;
#elif IOS || MACCATALYST
using UIKit;
#elif WINDOWS

#endif

using Microsoft.Maui.Platform;

#if ANDROID || IOS || MACCATALYST || WINDOWS
using Radek.SimpleShell.Controls.Handlers;
#endif
using System.Runtime.CompilerServices;

namespace Radek.SimpleShell.Controls
{
    public static class Extensions
    {
        public static MauiAppBuilder ConfigureSimpleShellControls(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
#if ANDROID || IOS || MACCATALYST || WINDOWS
                handlers.AddHandler(typeof(Icon), typeof(IconHandler));
                handlers.AddHandler(typeof(Popover), typeof(PopoverHandler));
#endif
            });

            return builder;
        }

        public static void ShowAttachedPopover(this View parentView)
        {
            var popover = Popover.GetAttachedPopover(parentView);
            popover.Show(parentView);
        }

        public static void HideAttachedPopover(this View parentView)
        {
            var popover = Popover.GetAttachedPopover(parentView);
            popover.Hide();
        }

#if !WEBVIEW2_MAUI

        internal static async void FireAndForget(this Task task, Action<Exception> errorCallback = null)
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

        internal static void FireAndForget<T>(this Task task, T? viewHandler, [CallerMemberName] string? callerName = null)
            where T : IElementHandler
        {
            task.FireAndForget();
        }
#endif
    }
}
