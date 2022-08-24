#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace Radek.SimpleShell.Controls.Handlers
{
    public class IconHandler : ViewHandler<Icon, UIImageView>
    {
        public static IPropertyMapper<Icon, IconHandler> Mapper = new PropertyMapper<Icon, IconHandler>(ViewHandler.ViewMapper)
        {
            [nameof(Icon.Source)] = MapSource,
            [nameof(Icon.TintColor)] = MapTintColor,
        };

        public static CommandMapper<Icon, IconHandler> CommandMapper = new(ViewHandler.ViewCommandMapper)
        {
        };

        ImageSourcePartLoader _imageSourcePartLoader;
        public ImageSourcePartLoader SourceLoader =>
            _imageSourcePartLoader ??= new ImageSourcePartLoader(this, () => VirtualView, OnSetImageSource);

        public IconHandler() : base(Mapper)
        {
        }

        public IconHandler(IPropertyMapper mapper) : base(mapper ?? Mapper)
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
            if (VirtualView is Icon bitmapIcon)
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
            UpdateValue(nameof(Icon.Source));
        }

        public static void MapSource(IconHandler handler, Icon image) =>
            MapSourceAsync(handler, image).FireAndForget(handler);

        public static Task MapSourceAsync(IconHandler handler, Icon image) =>
            handler.SourceLoader.UpdateImageSourceAsync();

        public static void MapTintColor(IconHandler handler, Icon image)
        {
            if (image is Icon bitmapIcon)
            {
                handler.ApplyTint(bitmapIcon.TintColor);
            }
        }
    }
}


#endif