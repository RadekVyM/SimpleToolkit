#if ANDROID
using Android.Graphics;
using Android.Widget;
#elif IOS || MACCATALYST
using UIKit;
#elif WINDOWS

#endif

using Microsoft.Maui.Platform;

#if ANDROID || IOS || MACCATALYST || WINDOWS
using Radek.SimpleShell.Controls.Handlers;
#endif
using System.Runtime.CompilerServices;

namespace Radek.SimpleShell.Controls
{
    public static class Extensions
    {
        public static MauiAppBuilder ConfigureSimpleShellControls(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
                #if ANDROID || IOS || MACCATALYST || WINDOWS
                handlers.AddHandler(typeof(BitmapIcon), typeof(BitmapIconHandler));
                #endif
            });

            return builder;
        }

        internal static Image ApplyTintToImage(this Image image, Microsoft.Maui.Graphics.Color color)
        {
#if ANDROID

            var imageView = image.Handler.PlatformView as ImageView;
            imageView.ClearColorFilter();
            if (color is not null)
                imageView.SetColorFilter(color.ToPlatform(), PorterDuff.Mode.SrcIn ?? throw new InvalidOperationException("PorterDuff.Mode.SrcIn should not be null at runtime"));

#elif IOS || MACCATALYST

            var imageView = image.Handler.PlatformView as UIImageView;
            if (imageView.Image is not null)
            {
                if (color is not null)
                {
                    imageView.Image = imageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                    imageView.TintColor = color.ToPlatform();
                }
                else
                {
                    imageView.Image = imageView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                }
            }

#elif WINDOWS

            //var wimage = image.Handler.PlatformView as Microsoft.UI.Xaml.Controls.Image;

            //wimage.Stretch = Microsoft.UI.Xaml.Media.Stretch.Fill;

            //var v = new Microsoft.UI.Xaml.Controls.BitmapIcon();

#endif

            return image;
        }

#if !WEBVIEW2_MAUI

        internal static async void FireAndForget(
            this Task task,
            Action<Exception> errorCallback = null
            )
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                errorCallback?.Invoke(ex);
#if DEBUG
                throw;
#endif
            }
        }

        internal static void FireAndForget<T>(this Task task, T? viewHandler, [CallerMemberName] string? callerName = null)
            where T : IElementHandler
        {
            task.FireAndForget();
        }
#endif
    }
}
