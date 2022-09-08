#if WINDOWS

using Microsoft.Maui.Handlers;
using WBorder = Microsoft.UI.Xaml.Controls.Border;
using WFrameworkElement = Microsoft.UI.Xaml.FrameworkElement;

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleNavigationHostHandler : ViewHandler<ISimpleNavigationHost, WBorder>
    {
        public static IPropertyMapper<ISimpleNavigationHost, SimpleNavigationHostHandler> Mapper = new PropertyMapper<ISimpleNavigationHost, SimpleNavigationHostHandler>(ViewHandler.ViewMapper)
        {
        };

        public static CommandMapper<ISimpleNavigationHost, SimpleNavigationHostHandler> CommandMapper = new(ViewHandler.ViewCommandMapper)
        {
        };

        public WBorder Container { get; protected set; }
        
        
        public SimpleNavigationHostHandler(IPropertyMapper mapper, CommandMapper commandMapper)
            : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
        {
        }

        public SimpleNavigationHostHandler()
            : base(Mapper, CommandMapper)
        {
        }


        protected override WBorder CreatePlatformView()
        {
            Container = new WBorder();
            return Container;
        }

        public virtual void SetContent(WFrameworkElement element)
        {
            Container.Child = element;
        }
    }
}

#endif