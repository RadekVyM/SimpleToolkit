#if WINDOWS

namespace SimpleToolkit.Core
{
    public static partial class WindowExtensions
    {
        /// <summary>
        /// Forces application content to be displayed behind system bars (status and navigation bars) on Android and iOS.
        /// </summary>
        /// <param name="builder">The builder for your .NET MAUI application and services.</param>
        /// <returns>The builder.</returns>
        public static MauiAppBuilder DisplayContentBehindBars(this MauiAppBuilder builder)
        {
            return builder;
        }
    }
}

#endif