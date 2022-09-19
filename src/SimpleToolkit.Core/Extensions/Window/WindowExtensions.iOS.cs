#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using UIKit;

namespace SimpleToolkit.Core
{
    public static partial class WindowExtensions
    {
        /// <summary>
        /// Forces application content to be displayed behind system bars (status and navigation bars) on Android and iOS.
        /// </summary>
        /// <param name="builder">The builder for your .NET MAUI application and services.</param>
        /// <returns>The builder.</returns>
        public static MauiAppBuilder DisplayContentBehindBars(this MauiAppBuilder builder)
        {
            LayoutHandler.Mapper.PrependToMapping("DisplayContentBehindBars", (handler, layout) =>
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