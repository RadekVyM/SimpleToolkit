#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace Radek.SimpleShell.Controls.Handlers
{
    public class BitmapIconHandler : ViewHandler<BitmapIcon, UIImageView>
    {
        public static IPropertyMapper<BitmapIcon, BitmapIconHandler> Mapper = new PropertyMapper<BitmapIcon, BitmapIconHandler>(ViewHandler.ViewMapper)
        {
            [nameof(BitmapIcon.Source)] = MapSource,
            [nameof(BitmapIcon.TintColor)] = MapTintColor,
        };

        public static CommandMapper<BitmapIcon, BitmapIconHandler> CommandMapper = new(ViewHandler.ViewCommandMapper)
        {
        };

        ImageSourcePartLoader _imageSourcePartLoader;
        public ImageSourcePartLoader SourceLoader =>
            _imageSourcePartLoader ??= new ImageSourcePartLoader(this, () => VirtualView, OnSetImageSource);

        public BitmapIconHandler() : base(Mapper)
        {
        }

        public BitmapIconHandler(IPropertyMapper mapper) : base(mapper ?? Mapper)
        {
        }

        protected override UIImageView CreatePlatformView()
        {
            var imageView = new MauiImageView();

            imageView.ContentMode = Aspect.AspectFit.ToUIViewContentMode();
            imageView.ClipsToBounds = imageView.ContentMode == UIViewContentMode.ScaleAspectFill;

            return imageView;
        }

        protected override void ConnectHandler(UIImageView platformView)
        {
            base.ConnectHandler(platformView);

            if (PlatformView is MauiImageView imageView)
                imageView.WindowChanged += OnWindowChanged;
        }

        protected override void DisconnectHandler(UIImageView platformView)
        {
            base.DisconnectHandler(platformView);

            if (platformView is MauiImageView imageView)
                imageView.WindowChanged -= OnWindowChanged;

            SourceLoader.Reset();
        }

        public override bool NeedsContainer =>
            VirtualView?.Background != null ||
            base.NeedsContainer;

        void OnSetImageSource(UIImage obj)
        {
            PlatformView.Image = obj.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            if (VirtualView is BitmapIcon bitmapIcon)
                PlatformView.TintColor = bitmapIcon.TintColor.ToPlatform();
        }

        private void ApplyTint(Color color)
        {
            if (PlatformView.Image is not null)
            {
                if (color is not null)
                {
                    if (PlatformView.Image.RenderingMode != UIImageRenderingMode.AlwaysTemplate)
                        PlatformView.Image = PlatformView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                    PlatformView.TintColor = color.ToPlatform();
                }
            }
        }

        void OnWindowChanged(object sender, EventArgs e)
        {
            // SourceManager is not public
            //if (SourceLoader.SourceManager.IsResolutionDependent)
            UpdateValue(nameof(BitmapIcon.Source));
        }

        public static void MapSource(BitmapIconHandler handler, BitmapIcon image) =>
            MapSourceAsync(handler, image).FireAndForget(handler);

        public static Task MapSourceAsync(BitmapIconHandler handler, BitmapIcon image) =>
            handler.SourceLoader.UpdateImageSourceAsync();

        public static void MapTintColor(BitmapIconHandler handler, BitmapIcon image)
        {
            if (image is BitmapIcon bitmapIcon)
            {
                handler.ApplyTint(bitmapIcon.TintColor);
            }
        }
    }
}


#endif