#if WINDOWS

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;
using WBitmapIcon = Microsoft.UI.Xaml.Controls.BitmapIcon;
using WBorder = Microsoft.UI.Xaml.Controls.Border;

namespace SimpleToolkit.Core.Handlers
{
    public class IconHandler : ViewHandler<Icon, WBorder>, IElementHandler
    {
        private WBitmapIcon bitmapIcon;
        private FontIcon fontIcon;
        private double fontSize;

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

            fontIcon.SizeChanged += FontIconSizeChanged;

            var container = new WBorder();

            return container;
        }

        private void FontIconSizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
        {
            // Fit glyph to the View
            if (VirtualView.Height is not -1 && VirtualView.Width is not -1)
            {
                fontIcon.FontSize = Math.Min(fontSize, Math.Min(VirtualView.Height, VirtualView.Width));
            }
        }

        public static void MapSource(IconHandler handler, Icon icon)
        {
            MapSourceAsync(handler, icon).FireAndForget(handler);
        }

        public static Task MapSourceAsync(IconHandler handler, Icon icon)
        {
            var iconSource = icon.Source.ToIconSource(handler.MauiContext);

            if (iconSource is BitmapIconSource bitmapIconSource)
            {
                handler.bitmapIcon.UriSource = bitmapIconSource.UriSource;
                handler.PlatformView.Child = handler.bitmapIcon;
            }
            else if (iconSource is FontIconSource fontIconSource)
            {
                handler.fontIcon.FontFamily = fontIconSource.FontFamily;
                handler.fontSize = fontIconSource.FontSize;
                if (icon.Height is not -1 && icon.Width is not -1)
                    handler.fontIcon.FontSize = Math.Min(fontIconSource.FontSize, Math.Min(icon.Height, icon.Width));
                else
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

        public static void MapTintColor(IconHandler handler, Icon icon)
        {
            if (icon.TintColor is not null)
            {
                var color = icon.TintColor.ToPlatform();

                handler.bitmapIcon.Foreground = color;
                handler.fontIcon.Foreground = color;
            }
        }
    }
}


#endif