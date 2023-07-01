#if !(ANDROID || WINDOWS || IOS || MACCATALYST)

using System;

namespace SimpleToolkit.Core
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