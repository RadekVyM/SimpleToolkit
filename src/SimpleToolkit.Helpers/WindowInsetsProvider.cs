namespace SimpleToolkit.Helpers;

public static class WindowInsetsProvider
{
    public static Thickness GetInsets()
    {
#if ANDROID30_0_OR_GREATER
        var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
        var rootView = activity?.Window?.DecorView;

        if (rootView is null)
            return new Thickness();

        var insetsCompat = AndroidX.Core.View.ViewCompat.GetRootWindowInsets(rootView);

        if (insetsCompat is not null)
        {
            var cutoutInsets = insetsCompat.GetInsets(AndroidX.Core.View.WindowInsetsCompat.Type.DisplayCutout());
            var systemBars = insetsCompat.GetInsets(AndroidX.Core.View.WindowInsetsCompat.Type.SystemBars());
            var nDensity = activity?.Resources?.DisplayMetrics?.Density;

            if (cutoutInsets is null || systemBars is null || nDensity is not float density)
                return new Thickness();

            return new Thickness(
                Math.Max(systemBars.Left, cutoutInsets.Left) / density,
                Math.Max(systemBars.Top, cutoutInsets.Top) / density,
                Math.Max(systemBars.Right, cutoutInsets.Right) / density,
                Math.Max(systemBars.Bottom, cutoutInsets.Bottom) / density);
        }
#elif IOS || MACCATALYST
        var window = UIKit.UIApplication.SharedApplication.ConnectedScenes
            .OfType<UIKit.UIWindowScene>()
            .SelectMany(s => s.Windows)
            .FirstOrDefault(w => w.IsKeyWindow);

        if (window is not null)
        {
            var insets = window.SafeAreaInsets;

            return new Thickness(
                (double)insets.Left,
                (double)insets.Top,
                (double)insets.Right,
                (double)insets.Bottom);
        }
#endif
        return new Thickness();
    }
}