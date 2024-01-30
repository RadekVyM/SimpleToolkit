#if ANDROID

using AView = Android.Views.View;
using Android.Views;

namespace SimpleToolkit.SimpleShell.Extensions;

internal static class PlatformViewExtensions
{
    public static IList<AView> GetChildViews(this ViewGroup layout)
    {
        var count = layout.ChildCount;
        IList<AView> children = new List<AView>();

        for (int i = 0; i < count; i++)
            children.Add(layout.GetChildAt(i));

        return children;
    }

    public static bool Contains(this ViewGroup layout, AView view)
    {
        var count = layout.ChildCount;

        for (int i = 0; i < count; i++)
        {
            if (layout.GetChildAt(i) == view)
                return true;
        }

        return false;
    }
}

#endif