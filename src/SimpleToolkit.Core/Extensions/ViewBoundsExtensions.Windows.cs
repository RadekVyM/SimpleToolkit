#if WINDOWS

using System;

namespace SimpleToolkit.Core.Extensions
{
    public static partial class ViewBoundsExtensions
	{
	    public static Rect GetBoundsOnScreen(this VisualElement view)
        {
            return new Rect();
        }
	}
}

#endif