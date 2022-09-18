#if WINDOWS

namespace SimpleToolkit.Core
{
    public static partial class WindowExtensions
    {
        public static MauiAppBuilder DisplayContentBehindBars(this MauiAppBuilder builder)
        {
            return builder;
        }
    }
}

#endif