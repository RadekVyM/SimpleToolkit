#if ANDROID

using AndroidX.Core.View;
using AndroidX.AppCompat.App;
using Android.Views;
using Android.OS;
using AView = Android.Views.View;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace SimpleToolkit.Core;

// https://developer.android.com/develop/ui/views/layout/edge-to-edge

public static partial class WindowExtensions
{
    private static Color defaultStatusBarColor = Colors.Transparent;
    private static Color defaultNavigationBarColor = Colors.Black;
    private static bool defaultStatusBarLightElements = false;
    private static bool defaultNavigationBarLightElements = true;

    /// <summary>
    /// Forces application content to be displayed behind system bars (status and navigation bars) on Android and iOS.
    /// </summary>
    /// <param name="builder">The builder for your .NET MAUI application and services.</param>
    /// <returns>The builder.</returns>
    public static MauiAppBuilder DisplayContentBehindBars(this MauiAppBuilder builder)
    {
        WindowHandler.Mapper.AppendToMapping("DisplayContentBehindBars", (handler, window) =>
        {
            var activity = handler.PlatformView as AppCompatActivity;

            WindowCompat.SetDecorFitsSystemWindows(activity.Window, false);

            window.SetStatusBarAppearance();
        });

        WindowHandler.Mapper.AppendToMapping("Insets", (handler, window) =>
        {
            var activity = handler.PlatformView as AppCompatActivity;

            if (window is not Element elmentWindow)
                return;

            // If I use just activity.Window.DecorView, set colors of the bars are overridden.
            ViewCompat.SetOnApplyWindowInsetsListener(
                (activity.Window.DecorView as ViewGroup).GetChildAt(0), 
                new SimpleOnApplyWindowInsetsListener(elmentWindow.Id, InvokeListenersIfChanged));
        });

        return builder;
    }

    /// <summary>
    /// Changes the status bar appearance of all windows.
    /// </summary>
    /// <param name="builder">The builder for your .NET MAUI application and services.</param>
    /// <param name="color">New background color of the status bar.</param>
    /// <param name="lightElements">Whether text and icons should be light or dark.</param>
    /// <returns>The builder.</returns>
    public static MauiAppBuilder SetDefaultStatusBarAppearance(this MauiAppBuilder builder, Color color = null, bool lightElements = true)
    {
        defaultStatusBarColor = color;
        defaultStatusBarLightElements = lightElements;

        WindowHandler.Mapper.AppendToMapping("DefaultStatusBarAppearance", (handler, window) =>
        {
            window.SetStatusBarAppearance(color, lightElements);
        });

        return builder;
    }

    /// <summary>
    /// Changes the navigation bar appearance of all windows.
    /// </summary>
    /// <param name="builder">The builder for your .NET MAUI application and services.</param>
    /// <param name="color">New background color of the navigation bar.</param>
    /// <param name="lightElements">Whether text and icons should be light or dark.</param>
    /// <returns>The builder.</returns>
    public static MauiAppBuilder SetDefaultNavigationBarAppearance(this MauiAppBuilder builder, Color color = null, bool lightElements = true)
    {
        defaultNavigationBarColor = color;
        defaultNavigationBarLightElements = lightElements;

        WindowHandler.Mapper.AppendToMapping("DefaultNavigationBarAppearance", (handler, window) =>
        {
            window.SetNavigationBarAppearance(color, lightElements);
        });

        return builder;
    }

    /// <summary>
    /// Changes the status bar appearance of a window.
    /// </summary>
    /// <param name="window">The window.</param>
    /// <param name="color">New background color of the status bar.</param>
    /// <param name="lightElements">Whether text and icons should be light or dark.</param>
    public static void SetStatusBarAppearance(this IWindow window, Color color = null, bool? lightElements = null)
    {
        var activity = window.Handler.PlatformView as AppCompatActivity;

        var windowInsetController = WindowCompat.GetInsetsController(activity.Window, activity.Window.DecorView);
        if (windowInsetController is not null)
        {
            windowInsetController.AppearanceLightStatusBars = !(lightElements ?? defaultStatusBarLightElements);
        }
        else if (Build.VERSION.SdkInt < BuildVersionCodes.R)
        {
            int uiOptions = (int)activity.Window.DecorView.SystemUiVisibility;

            if (!(lightElements ?? defaultStatusBarLightElements))
                uiOptions |= (int)SystemUiFlags.LightStatusBar;
            else
                uiOptions &= ~(int)SystemUiFlags.LightStatusBar;

            activity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
        }

        activity.Window.SetStatusBarColor((color ?? defaultStatusBarColor).ToPlatform());
    }

    /// <summary>
    /// Changes the navigation bar appearance of a window.
    /// </summary>
    /// <param name="window">The window.</param>
    /// <param name="color">New background color of the navigation bar.</param>
    /// <param name="lightElements">Whether text and icons should be light or dark.</param>
    public static void SetNavigationBarAppearance(this IWindow window, Color color = null, bool? lightElements = null)
    {
        var activity = window.Handler.PlatformView as AppCompatActivity;

        var windowInsetController = ViewCompat.GetWindowInsetsController(activity.Window.DecorView);
        if (windowInsetController is not null)
        {
            windowInsetController.AppearanceLightNavigationBars = !(lightElements ?? defaultNavigationBarLightElements);
        }
        else if (Build.VERSION.SdkInt < BuildVersionCodes.R)
        {
            int uiOptions = (int)activity.Window.DecorView.SystemUiVisibility;

            if (!(lightElements ?? defaultNavigationBarLightElements))
                uiOptions |= (int)SystemUiFlags.LightNavigationBar;
            else
                uiOptions &= ~(int)SystemUiFlags.LightNavigationBar;

            activity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
        }

        activity.Window.SetNavigationBarColor((color ?? defaultNavigationBarColor).ToPlatform());
    }
}

internal class SimpleOnApplyWindowInsetsListener : Java.Lang.Object, IOnApplyWindowInsetsListener
{
    private readonly Guid windowId;
    private readonly Action<Guid, Thickness> action;

    public SimpleOnApplyWindowInsetsListener(Guid windowId, Action<Guid, Thickness> action)
    {
        this.windowId = windowId;
        this.action = action;
    }

    public WindowInsetsCompat OnApplyWindowInsets(AView v, WindowInsetsCompat insets)
    {
        var barsInsets = insets.GetInsets(WindowInsetsCompat.Type.SystemBars());
        var density = DeviceDisplay.MainDisplayInfo.Density;

        action?.Invoke(windowId, new Thickness(barsInsets.Left / density, barsInsets.Top / density, barsInsets.Right / density, barsInsets.Bottom / density));

        return ViewCompat.OnApplyWindowInsets(v, insets.Inset(barsInsets));
    }
}

#endif