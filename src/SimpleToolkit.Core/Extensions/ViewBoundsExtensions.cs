using System;

namespace SimpleToolkit.Core.Extensions
{
	public static partial class ViewBoundsExtensions
	{
		public static Rect GetBounds(this VisualElement view, VisualElement relativeTo = null)
		{
            var viewWindowBounds = view.GetBoundsOnScreen();
            var relativeToBounds = relativeTo?.GetBoundsOnScreen() ?? new Rect();

            return new Rect(
                new Point(viewWindowBounds.X - relativeToBounds.X, viewWindowBounds.Y - relativeToBounds.Y),
                new Size(viewWindowBounds.Width, viewWindowBounds.Height)
            );
        }
	}
}
