
using Microsoft.Maui.Handlers;
#if WINDOWS
using WBorder = Microsoft.UI.Xaml.Controls.Border;
using WFrameworkElement = Microsoft.UI.Xaml.FrameworkElement;
#endif

namespace SimpleToolkit.SimpleShell.Handlers
{
#if WINDOWS
    public partial class SimpleNavigationHostHandler : ViewHandler<ISimpleNavigationHost, WBorder>
    {
        public static IPropertyMapper<ISimpleNavigationHost, SimpleNavigationHostHandler> Mapper = new PropertyMapper<ISimpleNavigationHost, SimpleNavigationHostHandler>(ViewHandler.ViewMapper)
        {
        };

        public static CommandMapper<ISimpleNavigationHost, SimpleNavigationHostHandler> CommandMapper = new(ViewHandler.ViewCommandMapper)
        {
        };

        public SimpleNavigationHostHandler(IPropertyMapper mapper, CommandMapper commandMapper)
            : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
        {
        }

        public SimpleNavigationHostHandler()
            : base(Mapper, CommandMapper)
        {
        }

        public WBorder Container { get; protected set; }

        protected override WBorder CreatePlatformView()
        {
            Container = new WBorder();

            //Frame.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 255, 0));

            return Container;
        }

        public virtual void SetContent(WFrameworkElement element)
        {
            Container.Child = element;
        }
    }
#endif

#if !ANDROID && !(__IOS__ || MACCATALYST) && !WINDOWS
    public partial class SimpleNavigationHostHandler : ElementHandler<ISimpleNavigationHost, System.Object>
    {
        public SimpleNavigationHostHandler(IPropertyMapper mapper, CommandMapper commandMapper)
            : base(mapper, commandMapper)
        {
        }

        protected override System.Object CreatePlatformElement()
        {
            throw new NotImplementedException();
        }
    }
#endif
}