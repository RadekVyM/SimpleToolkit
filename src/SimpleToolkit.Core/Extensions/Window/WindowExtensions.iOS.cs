#if IOS || MACCATALYST

using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace SimpleToolkit.Core
{
    public static partial class WindowExtensions
    {
        public static MauiAppBuilder RenderContentUnderBars(this MauiAppBuilder builder)
        {
            LayoutHandler.Mapper.PrependToMapping("RenderContentUnderBars", (handler, layout) =>
            {
                if (layout is Layout realLayout)
                    realLayout.IgnoreSafeArea = true;
            });

            WindowHandler.Mapper.AppendToMapping("Insets", (handler, window) =>
            {
                if (window is not Element elementWindow)
                    return;

                if (!safeAreas.ContainsKey(elementWindow.Id))
                    safeAreas[elementWindow.Id] = GetInsets(window);

                if (window is not Window realWindow)
                    return;

                DeviceDisplay.Current.MainDisplayInfoChanged += DeviceDisplayMainDisplayInfoChanged;
                realWindow.Destroying += WindowExtensionsDestroying;
            });

            return builder;
        }

        private static void WindowExtensionsDestroying(object sender, EventArgs e)
        {
            var window = sender as Window;

            window.Destroying -= WindowExtensionsDestroying;
            DeviceDisplay.Current.MainDisplayInfoChanged -= DeviceDisplayMainDisplayInfoChanged;
        }

        private static void DeviceDisplayMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            foreach (var key in safeAreas.Keys)
            {
                var window = Application.Current.Windows.FirstOrDefault(w => w.Id == key);

                InvokeListenersIfChanged(key, GetInsets(window));
            }
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
        }

        public static void SetNavigationBarAppearance(this IWindow window, Color color = null, bool lightElements = true)
        {
        }

        private static Thickness GetInsets(IWindow window)
        {
            if (window is null)
                return new Thickness(0);

            var uiWindow = window.Handler.PlatformView as UIWindow;
            return new Thickness(uiWindow.SafeAreaInsets.Left, uiWindow.SafeAreaInsets.Top, uiWindow.SafeAreaInsets.Right, uiWindow.SafeAreaInsets.Bottom);
        }
    }
}

#endif