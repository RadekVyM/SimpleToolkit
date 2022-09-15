#if ANDROID
using AndroidX.Core.View;
using AndroidX.AppCompat.App;
using Android.Views;
using Android.OS;
using AView = Android.Views.View;
using AWindow = Android.Views.Window;
using MWindow = Microsoft.Maui.Controls.Window;
#elif IOS
using UIKit;
#endif

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace SimpleToolkit.Core
{
    public static class WindowExtensions
    {
        // https://developer.android.com/develop/ui/views/layout/edge-to-edge

        public static MauiAppBuilder RenderContentUnderBars(this MauiAppBuilder builder)
        {
#if ANDROID
            WindowHandler.Mapper.AppendToMapping("RenderContentUnderBars", (handler, window) =>
            {
                var activity = handler.PlatformView as AppCompatActivity;

                WindowCompat.SetDecorFitsSystemWindows(activity.Window, false);
            });

            WindowHandler.Mapper.AppendToMapping("Insets", (handler, window) =>
            {
                var activity = handler.PlatformView as AppCompatActivity;

                // If I use just activity.Window.DecorView, set colors of the bars are overridden.
                ViewCompat.SetOnApplyWindowInsetsListener((activity.Window.DecorView as ViewGroup).GetChildAt(0), new SimpleOnApplyWindowInsetsListener());
            });
#elif IOS
            LayoutHandler.Mapper.PrependToMapping("RenderContentUnderBars", (handler, layout) =>
            {
                if (layout is Layout realLayout)
                    realLayout.IgnoreSafeArea = true;
            });
#endif
            return builder;
        }

        public static MauiAppBuilder SetDefaultStatusBarAppearance(this MauiAppBuilder builder, Color color = null, bool lightElements = true)
        {
#if ANDROID
            WindowHandler.Mapper.AppendToMapping("DefaultStatusBarAppearance", (handler, window) =>
            {
                window.SetStatusBarAppearance(color, lightElements);
            });
#endif
            return builder;
        }

        public static MauiAppBuilder SetDefaultNavigationBarAppearance(this MauiAppBuilder builder, Color color = null, bool lightElements = true)
        {
#if ANDROID
            WindowHandler.Mapper.AppendToMapping("DefaultNavigationBarAppearance", (handler, window) =>
            {
                window.SetNavigationBarAppearance(color, lightElements);
            });
#endif
            return builder;
        }

        public static void SetStatusBarAppearance(this IWindow window, Color color = null, bool lightElements = true)
        {
#if ANDROID
            var activity = window.Handler.PlatformView as AppCompatActivity;

            var windowInsetController = ViewCompat.GetWindowInsetsController(activity.Window.DecorView);
            if (windowInsetController is not null)
            {
                windowInsetController.AppearanceLightStatusBars = !lightElements;
            }
            else if (Build.VERSION.SdkInt < BuildVersionCodes.R)
            {
                int uiOptions = (int)activity.Window.DecorView.SystemUiVisibility;

                if (!lightElements)
                    uiOptions |= (int)SystemUiFlags.LightStatusBar;
                else
                    uiOptions &= ~(int)SystemUiFlags.LightStatusBar;

                activity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
            }

            activity.Window.SetStatusBarColor((color ?? Colors.Transparent).ToPlatform());
#endif
        }

        public static void SetNavigationBarAppearance(this IWindow window, Color color = null, bool lightElements = true)
        {
#if ANDROID
            var activity = window.Handler.PlatformView as AppCompatActivity;

            var windowInsetController = ViewCompat.GetWindowInsetsController(activity.Window.DecorView);
            if (windowInsetController is not null)
            {
                windowInsetController.AppearanceLightNavigationBars = !lightElements;
            }
            else if (Build.VERSION.SdkInt < BuildVersionCodes.R)
            {
                int uiOptions = (int)activity.Window.DecorView.SystemUiVisibility;

                if (!lightElements)
                    uiOptions |= (int)SystemUiFlags.LightNavigationBar;
                else
                    uiOptions &= ~(int)SystemUiFlags.LightNavigationBar;

                activity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
            }

            activity.Window.SetNavigationBarColor((color ?? Colors.Transparent).ToPlatform());
#endif
        }

        public static Thickness AddOnSafeAreaChangedListener(this IWindow window, Action<Thickness> listener)
        {
#if ANDROID
            var platform = window.Handler.PlatformView as AppCompatActivity;
#elif IOS
            // Subscribe to DeviceDisplay.Current.MainDisplayInfo.MainDisplayInfoChanged and check orientation changes
            var uiWindow = window.Handler.PlatformView as UIWindow;
            var insets = new Thickness(uiWindow.SafeAreaInsets.Left, uiWindow.SafeAreaInsets.Top, uiWindow.SafeAreaInsets.Right, uiWindow.SafeAreaInsets.Bottom);

            return insets;
#endif

            return new Thickness(0);
        }
    }

#if ANDROID
    internal class SimpleOnApplyWindowInsetsListener : Java.Lang.Object, IOnApplyWindowInsetsListener
    {
        public WindowInsetsCompat OnApplyWindowInsets(AView v, WindowInsetsCompat insets)
        {
            AndroidX.Core.Graphics.Insets barsInsets = insets.GetInsets(WindowInsetsCompat.Type.SystemBars());

            return insets;
        }
    }
#endif
}
