#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;
using IImage = Microsoft.Maui.IImage;

namespace Radek.SimpleShell.Controls.Handlers
{
    public class BitmapIconHandler : ImageHandler
    {
        public new static IPropertyMapper<IImage, BitmapIconHandler> Mapper = new PropertyMapper<IImage, BitmapIconHandler>(ImageHandler.Mapper)
        {
            [nameof(BitmapIcon.TintColor)] = MapTintColor,
        };

        public new static CommandMapper<IImage, BitmapIconHandler> CommandMapper = new(ImageHandler.CommandMapper)
        {
        };


        public BitmapIconHandler() : base(Mapper)
        {
        }


        private void ApplyTint(Color color)
        {
            if (PlatformView.Image is not null)
            {
                if (color is not null)
                {
                    PlatformView.Image = PlatformView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                    PlatformView.TintColor = color.ToPlatform();
                }
                else
                {
                    PlatformView.Image = PlatformView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                }
            }
        }

        private static void MapTintColor(BitmapIconHandler handler, IImage image)
        {
            if (image is BitmapIcon bitmapIcon)
            {
                handler.ApplyTint(bitmapIcon.TintColor);
            }
        }
    }
}


#endif