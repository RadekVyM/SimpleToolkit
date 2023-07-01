#if WINDOWS

using Microsoft.UI.Xaml;
using WinPoint = Windows.Foundation.Point;

namespace SimpleToolkit.Core
{
    public static partial class ViewBoundsExtensions
	{
	    public static Rect GetBoundsOnScreen(this VisualElement view)
        {
            if (view?.Handler?.PlatformView is not FrameworkElement platformView)
                return new Rect();

            if (platformView == null)
                return new Rect();

            var rootView = platformView.XamlRoot.Content;
            if (platformView == rootView)
            {
                if (rootView is not FrameworkElement el)
                    return new Rect();

                return new Rect(0, 0, el.ActualWidth, el.ActualHeight);
            }

            var topLeft = platformView.TransformToVisual(rootView).TransformPoint(new WinPoint());
            var topRight = platformView.TransformToVisual(rootView).TransformPoint(new WinPoint(platformView.ActualWidth, 0));
            var bottomLeft = platformView.TransformToVisual(rootView).TransformPoint(new WinPoint(0, platformView.ActualHeight));
            var bottomRight = platformView.TransformToVisual(rootView).TransformPoint(new WinPoint(platformView.ActualWidth, platformView.ActualHeight));

            var x1 = new[] { topLeft.X, topRight.X, bottomLeft.X, bottomRight.X }.Min();
            var x2 = new[] { topLeft.X, topRight.X, bottomLeft.X, bottomRight.X }.Max();
            var y1 = new[] { topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y }.Min();
            var y2 = new[] { topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y }.Max();
            return new Rect(x1, y1, x2 - x1, y2 - y1);
        }
	}
}

#endif