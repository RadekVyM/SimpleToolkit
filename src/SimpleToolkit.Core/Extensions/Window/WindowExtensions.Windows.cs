#if WINDOWS

using Microsoft.Maui.Handlers;

namespace SimpleToolkit.Core
{
    public static partial class WindowExtensions
    {
        public static MauiAppBuilder RenderContentUnderBars(this MauiAppBuilder builder)
        {
            return builder;
        }

        public static MauiAppBuilder SetDefaultStatusBarAppearance(this MauiAppBuilder builder, Color color = null, bool lightElements = true)
        {
            return builder;
        }

        public static MauiAppBuilder SetDefaultNavigationBarAppearance(this MauiAppBuilder builder, Color color = null, bool lightElements = true)
        {
            return builder;
        }

        public static void SetStatusBarAppearance(this IWindow window, Color color = null, bool lightElements = true)
        {
            var winUIWindow = window.Handler.PlatformView as MauiWinUIWindow;
        }

        public static void SetNavigationBarAppearance(this IWindow window, Color color = null, bool lightElements = true)
        {
        }
    }
}

#endif