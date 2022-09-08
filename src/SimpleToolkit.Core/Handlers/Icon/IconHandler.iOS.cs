#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace SimpleToolkit.Core.Handlers
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

        ImageSourcePartLoader imageSourcePartLoader;
        public ImageSourcePartLoader SourceLoader =>
            imageSourcePartLoader ??= new ImageSourcePartLoader(this, () => VirtualView, OnSetImageSource);

        public override bool NeedsContainer =>
            VirtualView?.Background != null ||
            base.NeedsContainer;


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

        void OnSetImageSource(UIImage obj)
        {
            PlatformView.Image = obj.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            PlatformView.TintColor = VirtualView.TintColor.ToPlatform();
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

        public static void MapSource(IconHandler handler, Icon icon) =>
            MapSourceAsync(handler, icon).FireAndForget(handler);

        public static Task MapSourceAsync(IconHandler handler, Icon icon) =>
            handler.SourceLoader.UpdateImageSourceAsync();

        public static void MapTintColor(IconHandler handler, Icon icon) => 
            handler.ApplyTint(icon.TintColor);
    }
}


#endif