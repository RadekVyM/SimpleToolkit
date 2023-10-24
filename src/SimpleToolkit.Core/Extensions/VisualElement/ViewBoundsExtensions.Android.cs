#if ANDROID

using Microsoft.Maui.Platform;
using AView = Android.Views.View;

namespace SimpleToolkit.Core;

public static partial class ViewBoundsExtensions
	{
    public static Rect GetBoundsOnScreen(this VisualElement view)
    {
        if (view?.Handler?.PlatformView is not AView platformView)
            return new Rect();

        int[] location = new int[2];
        platformView.GetLocationOnScreen(location);
        var context = platformView.Context;

        return new Rect(
            context.FromPixels(location[0]),
            context.FromPixels(location[1]),
            context.FromPixels(platformView.MeasuredWidth),
            context.FromPixels(platformView.MeasuredHeight));
    }
}

#endif