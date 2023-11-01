#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace SimpleToolkit.Core.Handlers;

public class IconHandler : ViewHandler<Icon, UIImageView>, IImageHandler
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
        imageSourcePartLoader ??= new ImageSourcePartLoader(new SourceSetter(this));

    public override bool NeedsContainer =>
        VirtualView?.Background != null ||
        base.NeedsContainer;

    Microsoft.Maui.IImage IImageHandler.VirtualView => VirtualView;

    public IconHandler() : base(Mapper)
    {
    }

    public IconHandler(IPropertyMapper mapper) : base(mapper ?? Mapper)
    {
    }


    public void OnWindowChanged()
    {
        // SourceManager is not public
        //if (SourceLoader.SourceManager.RequiresReload(PlatformView))
            UpdateValue(nameof(Icon.Source));
    }

    protected override UIImageView CreatePlatformView()
    {
        var imageView = new MauiImageView(this);

        imageView.ContentMode = Aspect.AspectFit.ToUIViewContentMode();
        imageView.ClipsToBounds = imageView.ContentMode == UIViewContentMode.ScaleAspectFill;

        return imageView;
    }

    protected override void DisconnectHandler(UIImageView platformView)
    {
        base.DisconnectHandler(platformView);

        SourceLoader.Reset();
    }

    private void ApplyTint(Color color)
    {
        if (PlatformView.Image is not null && color is not null)
        {
            if (PlatformView.Image.RenderingMode != UIImageRenderingMode.AlwaysTemplate)
                PlatformView.Image = PlatformView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            PlatformView.TintAdjustmentMode = UIViewTintAdjustmentMode.Normal;
            PlatformView.TintColor = color.ToPlatform();
        }
    }

    public static void MapSource(IconHandler handler, Icon icon) =>
        MapSourceAsync(handler, icon).FireAndForget(handler);

    public static Task MapSourceAsync(IconHandler handler, Icon icon) =>
        handler.SourceLoader.UpdateImageSourceAsync();

    public static void MapTintColor(IconHandler handler, Icon icon) => 
        handler.ApplyTint(icon.TintColor);

    class SourceSetter(IconHandler handler) : SimpleIconSourcePartSetter(handler)
    {
        public override void SetImageSource(UIImage platformImage)
        {
            if (Handler?.PlatformView is not UIImageView imageView)
                return;

            imageView.Image = platformImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            imageView.TintColor = Handler.VirtualView.TintColor?.ToPlatform();

            if (Handler?.VirtualView is Microsoft.Maui.IImage image && image.Source is IStreamImageSource)
                imageView.InvalidateMeasure(image);
        }
    }
}

#endif