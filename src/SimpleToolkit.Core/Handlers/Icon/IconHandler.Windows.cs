using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;
using WBorder = Microsoft.UI.Xaml.Controls.Border;

namespace SimpleToolkit.Core.Handlers;

public class IconHandler : ViewHandler<Icon, WBorder>, IElementHandler
{
    private IconSourceElement? iconElement;

    public static IPropertyMapper<Icon, IconHandler> Mapper = new PropertyMapper<Icon, IconHandler>(ViewHandler.ViewMapper)
    {
        [nameof(Icon.Source)] = MapSource,
        [nameof(Icon.TintColor)] = MapTintColor,
    };

    public static CommandMapper<Icon, IconHandler> CommandMapper = new(ViewHandler.ViewCommandMapper)
    {
    };


    public IconHandler() : base(Mapper)
    {
    }

    public IconHandler(IPropertyMapper mapper) : base(mapper ?? Mapper)
    {
    }


    protected override WBorder CreatePlatformView()
    {
        iconElement = new IconSourceElement
        {
            HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Stretch,
            VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Stretch,
        };
        return new WBorder { Child = iconElement };
    }

    protected override void ConnectHandler(WBorder platformView)
    {
        if (iconElement is not null)
            iconElement.SizeChanged += FontIconSizeChanged;
        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(WBorder platformView)
    {
        if (iconElement is not null)
            iconElement.SizeChanged -= FontIconSizeChanged;
        base.DisconnectHandler(platformView);
    }

    private void FontIconSizeChanged(object? sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
    {
        if (sender is not IconSourceElement iconElement || iconElement.ActualHeight < 0 || iconElement.ActualWidth < 0)
            return;

        // Fit glyph to the View
        if (VirtualView.Source?.ToIconSource(MauiContext ?? throw new NullReferenceException("MauiContext should not be null here.")) is FontIconSource fontIconSource)
        {
            const double fontSizeScale = 0.86;

            UpdateIconSourceDefaults(this, VirtualView, fontIconSource);
            fontIconSource.FontSize = Math.Min(iconElement.ActualHeight, iconElement.ActualWidth) * fontSizeScale;
            iconElement.IconSource = fontIconSource;
        }
    }

    private static void UpdateIconSourceDefaults(IconHandler handler, Icon icon, IconSource? iconSource)
    {
        if (iconSource is BitmapIconSource bitmapIconSource)
        {
            bitmapIconSource.ShowAsMonochrome = true;
        }

        if (iconSource is FontIconSource fontIconSource)
        {
            if (handler.iconElement?.IconSource is FontIconSource oldFontIconSource)
            {
                fontIconSource.FontSize = oldFontIconSource.FontSize;
            }

            fontIconSource.IsTextScaleFactorEnabled = false;
            fontIconSource.Foreground = icon.TintColor?.ToPlatform();
        }
    }

    public static void MapSource(IconHandler handler, Icon icon)
    {
        var iconSource = icon.Source?.ToIconSource(handler.MauiContext ?? throw new NullReferenceException("MauiContext should not be null here."));
        UpdateIconSourceDefaults(handler, icon, iconSource);

        if (handler.iconElement is not null)
            handler.iconElement.IconSource = iconSource;
    }

    public static void MapTintColor(IconHandler handler, Icon icon)
    {
        UpdateIconSourceDefaults(handler, icon, handler.iconElement.IconSource);

        handler.iconElement.Foreground = icon.TintColor?.ToPlatform();
    }
}