#if ANDROID

using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Color = Microsoft.Maui.Graphics.Color;

namespace SimpleToolkit.Core.Handlers;

public partial class IconHandler : ViewHandler<Icon, ImageView>
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


    protected override ImageView CreatePlatformView()
    {
        var imageView = new AppCompatImageView(Context);

        // Enable view bounds adjustment on measure.
        // This allows the ImageView's OnMeasure method to account for the image's intrinsic
        // aspect ratio during measurement, which gives us more useful values during constrained
        // measurement passes.
        imageView.SetAdjustViewBounds(true);

        imageView.SetScaleType(Aspect.AspectFit.ToScaleType());

        return imageView;
    }

    protected override void DisconnectHandler(ImageView platformView)
    {
        base.DisconnectHandler(platformView);
        SourceLoader.Reset();
    }

    public override void PlatformArrange(Microsoft.Maui.Graphics.Rect frame)
    {
        if (PlatformView.GetScaleType() == ImageView.ScaleType.CenterCrop)
        {
            // If the image is center cropped (AspectFill), then the size of the image likely exceeds
            // the view size in some dimension. So we need to clip to the view's bounds.

            var (left, top, right, bottom) = PlatformView.Context!.ToPixels(frame);
            var clipRect = new Android.Graphics.Rect(0, 0, right - left, bottom - top);
            PlatformView.ClipBounds = clipRect;
        }
        else
        {
            PlatformView.ClipBounds = null;
        }

        base.PlatformArrange(frame);
    }

    private void OnSetImageSource(Drawable obj) =>
        PlatformView.SetImageDrawable(obj);

    private void ApplyTint(Color color)
    {
        PlatformView.ClearColorFilter();
        if (color is not null)
            PlatformView.SetColorFilter(color.ToPlatform(), PorterDuff.Mode.SrcIn ?? throw new InvalidOperationException("PorterDuff.Mode.SrcIn should not be null at runtime"));
    }

    public static void MapSource(IconHandler handler, Icon icon) =>
        MapSourceAsync(handler, icon).FireAndForget(handler);

    public static Task MapSourceAsync(IconHandler handler, Icon icon) =>
        handler.SourceLoader.UpdateImageSourceAsync();

    public static void MapTintColor(IconHandler handler, Icon icon) =>
        handler.ApplyTint(icon.TintColor);
}

#endif