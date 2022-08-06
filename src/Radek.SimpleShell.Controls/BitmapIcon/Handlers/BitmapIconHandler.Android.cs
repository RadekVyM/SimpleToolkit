#if ANDROID

using Android.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Color = Microsoft.Maui.Graphics.Color;
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
            PlatformView.ClearColorFilter();
            if (color is not null)
                PlatformView.SetColorFilter(color.ToPlatform(), PorterDuff.Mode.SrcIn ?? throw new InvalidOperationException("PorterDuff.Mode.SrcIn should not be null at runtime"));
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