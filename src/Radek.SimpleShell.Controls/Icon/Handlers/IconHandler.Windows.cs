#if WINDOWS

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;
using WBitmapIcon = Microsoft.UI.Xaml.Controls.BitmapIcon;
using WBorder = Microsoft.UI.Xaml.Controls.Border;

namespace Radek.SimpleShell.Controls.Handlers
{
    public class IconHandler : ViewHandler<Icon, WBorder>, IElementHandler
    {
        private WBitmapIcon bitmapIcon;
        private FontIcon fontIcon;

        public static IPropertyMapper<Icon, IconHandler> Mapper = new PropertyMapper<Icon, IconHandler>(ViewHandler.ViewMapper)
        {
            [nameof(Icon.Source)] = MapSource,
            [nameof(Icon.TintColor)] = MapTintColor,
        };

        public static CommandMapper<Icon, IconHandler> CommandMapper = new(ViewHandler.ViewCommandMapper)
        {
        };

        public override bool NeedsContainer =>
            VirtualView?.Background != null ||
            base.NeedsContainer;


        public IconHandler() : base(Mapper)
        {
        }

        public IconHandler(IPropertyMapper mapper) : base(mapper ?? Mapper)
        {
        }


        protected override WBorder CreatePlatformView()
        {
            fontIcon = new FontIcon
            {
                HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center,
                VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center
            };
            bitmapIcon = new WBitmapIcon
            {
                HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center,
                VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center,
            };

            var container = new WBorder();

            return container;
        }

        public static void MapSource(IconHandler handler, Icon image)
        {
            MapSourceAsync(handler, image).FireAndForget(handler);
        }

        public static Task MapSourceAsync(IconHandler handler, Icon image)
        {
            var iconSource = image.Source.ToIconSource(handler.MauiContext);

            if (iconSource is BitmapIconSource bitmapIconSource)
            {
                handler.bitmapIcon.UriSource = bitmapIconSource.UriSource;
                handler.PlatformView.Child = handler.bitmapIcon;
            }
            else if (iconSource is FontIconSource fontIconSource)
            {
                handler.fontIcon.FontFamily = fontIconSource.FontFamily;
                handler.fontIcon.FontSize = fontIconSource.FontSize;
                handler.fontIcon.FontStyle = fontIconSource.FontStyle;
                handler.fontIcon.FontWeight = fontIconSource.FontWeight;
                handler.fontIcon.Glyph = fontIconSource.Glyph;
                handler.fontIcon.IsTextScaleFactorEnabled = fontIconSource.IsTextScaleFactorEnabled;
                handler.fontIcon.MirroredWhenRightToLeft = fontIconSource.MirroredWhenRightToLeft;
                handler.PlatformView.Child = handler.fontIcon;
            }
            
            return Task.CompletedTask;
        }

        public static void MapTintColor(IconHandler handler, Icon image)
        {
            if (image.TintColor is not null)
            {
                var color = image.TintColor.ToPlatform();

                handler.bitmapIcon.Foreground = color;
                handler.fontIcon.Foreground = color;
            }
        }
    }
}


#endif