#if IOS || MACCATALYST

using System;
using UIKit;

namespace SimpleToolkit.Core
{
    public static partial class ViewBoundsExtensions
    {
        public static Rect GetBoundsOnScreen(this VisualElement view)
        {
            if (view.Handler.PlatformView is not UIView platformView)
                return new Rect();

            var superview = platformView;

            while (superview.Superview is not null)
                superview = superview.Superview;

            var convertPoint = platformView.ConvertRectToView(platformView.Bounds, superview);

            return new Rect(
                convertPoint.X,
                convertPoint.Y,
                convertPoint.Width,
                convertPoint.Height);
        }
    }
}

#endif