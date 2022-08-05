#if ANDROID
using Android.Graphics;
using Android.Widget;
#elif IOS || MACCATALYST
using UIKit;
#elif WINDOWS

#endif

using Microsoft.Maui.Platform;

namespace Radek.SimpleShell.Controls
{
    internal static class Extensions
    {
        public static Image ApplyTintToImage(this Image image, Microsoft.Maui.Graphics.Color color)
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

            var wimage = image.Handler.PlatformView as Microsoft.UI.Xaml.Controls.Image;

#endif

            return image;
        }
    }
}
