#if WINDOWS

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Media.Imaging;
using WBitmapIcon = Microsoft.UI.Xaml.Controls.BitmapIcon;
using WImageSource = Microsoft.UI.Xaml.Media.ImageSource;

namespace Radek.SimpleShell.Controls.Handlers
{
    public class BitmapIconHandler : ViewHandler<BitmapIcon, WBitmapIcon>, IElementHandler
    {
        public static IPropertyMapper<BitmapIcon, BitmapIconHandler> Mapper = new PropertyMapper<BitmapIcon, BitmapIconHandler>(ViewHandler.ViewMapper)
        {
            [nameof(BitmapIcon.Source)] = MapSource,
            [nameof(BitmapIcon.TintColor)] = MapTintColor,
        };

        public static CommandMapper<BitmapIcon, BitmapIconHandler> CommandMapper = new(ViewHandler.ViewCommandMapper)
        {
        };

        ImageSourcePartLoader? _imageSourcePartLoader;
        public ImageSourcePartLoader SourceLoader =>
            _imageSourcePartLoader ??= new ImageSourcePartLoader(this, () => VirtualView, OnSetImageSource);

        public BitmapIconHandler() : base(Mapper)
        {
        }

        public BitmapIconHandler(IPropertyMapper mapper) : base(mapper ?? Mapper)
        {
        }

        protected override WBitmapIcon CreatePlatformView() => new WBitmapIcon();

        protected override void DisconnectHandler(WBitmapIcon platformView)
        {
            base.DisconnectHandler(platformView);
            SourceLoader.Reset();
        }

        public override bool NeedsContainer =>
            VirtualView?.Background != null ||
            base.NeedsContainer;
            

        void OnSetImageSource(WImageSource obj)
        {
            if (obj is BitmapImage bitmapImage)
            {
                PlatformView.UriSource = bitmapImage.UriSource;
            }
        }

        public static void MapSource(BitmapIconHandler handler, BitmapIcon image) =>
            MapSourceAsync(handler, image).FireAndForget(handler);

        public static Task MapSourceAsync(BitmapIconHandler handler, BitmapIcon image) =>
            handler.SourceLoader.UpdateImageSourceAsync();

        public static void MapTintColor(BitmapIconHandler handler, BitmapIcon image)
        {
            if (image.TintColor is not null)
                handler.PlatformView.Foreground = image.TintColor.ToPlatform();
        }
    }
}


#endif