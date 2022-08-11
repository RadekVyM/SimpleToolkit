#if __IOS__ || MACCATALYST

using CoreAnimation;
using CoreGraphics;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace Radek.SimpleShell.Handlers
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

        public UIView Container { get; protected set; }

        protected override UIView CreatePlatformView()
        {
            Container = new CustomContentView();

            return Container;
        }

        public virtual void SetContent(UIView view)
        {
            //view.Frame = PlatformView.Bounds;
            Container.ClearSubviews();
            Container.AddSubview(view);
        }
    }

    // TODO: Clipping to Border shape does not work
    public class CustomContentView : Microsoft.Maui.Platform.ContentView
    {
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            foreach (var subview in Subviews)
            {
                subview.Frame = Bounds;
            }
        }
    }
}

#endif
