#if ANDROID

using System;
using FrameLayout = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
using AView = Android.Views.View;

namespace SimpleToolkit.SimpleShell.Extensions
{
    internal static class PlatformViewExtensions
    {
        public static IList<AView> GetChildViews(this FrameLayout layout)
        {
            var count = layout.ChildCount;
            IList<AView> children = new List<AView>();

            for (int i = 0; i < count; i++)
                children.Add(layout.GetChildAt(i));

            return children;
        }

        public static bool Contains(this FrameLayout layout, AView view)
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
}

#endif