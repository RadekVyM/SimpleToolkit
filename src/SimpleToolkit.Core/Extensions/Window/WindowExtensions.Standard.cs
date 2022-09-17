#if !(ANDROID || IOS || MACCATALYST || WINDOWS)

namespace SimpleToolkit.Core
{
    public static partial class WindowExtensions
    {
        public static MauiAppBuilder RenderContentUnderBars(this MauiAppBuilder builder)
        {
            return builder;
        }
    }
}

#endif