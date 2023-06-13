#if ANDROID

using System;
using Microsoft.Maui.Platform;
using AView = Android.Views.View;

namespace SimpleToolkit.Core
{
    public static partial class ViewBoundsExtensions
	{
        public static Rect GetBoundsOnScreen(this VisualElement view)
        {
            if (view.Handler.PlatformView is not AView platformView)
                return new Rect();

            int[] location = new int[2];
            platformView.GetLocationOnScreen(location);

            return new Rect(
                platformView.Context.FromPixels(location[0]),
                platformView.Context.FromPixels(location[1]),
                platformView.Context.FromPixels(platformView.MeasuredWidth),
                platformView.Context.FromPixels(platformView.MeasuredHeight));
        }
    }
}

#endif