#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace SimpleToolkit.SimpleShell.Handlers
{ 
    public partial class SimpleNavigationHostHandler : ViewHandler<ISimpleNavigationHost, UIView>
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


        protected override UIView CreatePlatformView()
        {
            return new CustomContentView();
        }

        public virtual void SetContent(UIView view)
        {
            PlatformView.ClearSubviews();
            if (view is not null)
                PlatformView.AddSubview(view);
        }
    }
}

#endif
