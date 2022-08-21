#if IOS || MACCATALYST

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Radek.SimpleShell.Controls.Platform;
using UIKit;

namespace Radek.SimpleShell.Controls.Handlers
{
    public partial class PopoverHandler : ElementHandler<IPopover, UIPopoverViewController>
    {
        protected override UIPopoverViewController CreatePlatformElement()
        {
            return new UIPopoverViewController(MauiContext);
        }

        protected override void ConnectHandler(UIPopoverViewController platformView)
        {
            base.ConnectHandler(platformView);
            platformView.SetElement(VirtualView);
        }

        protected override void DisconnectHandler(UIPopoverViewController platformView)
        {
            base.DisconnectHandler(platformView);
            platformView.CleanUp();
        }

        private static void MapContent(PopoverHandler handler, IPopover popover)
        {
            if (handler.PlatformView?.Control?.VirtualView is ContentPage contentPage)
            {
                contentPage.Content = popover.Content;
            }
        }

        private static void MapShow(PopoverHandler handler, IPopover popover, object parentView)
        {
            if (parentView is not IElement anchor)
                return;

            handler.PlatformView?.CreateControl(CreatePageHandler, popover, anchor);

            static PageHandler CreatePageHandler(IPopover virtualView)
            {
                var mauiContext = virtualView.Handler?.MauiContext ?? throw new NullReferenceException(nameof(IMauiContext));
                var view = virtualView.Content ?? throw new InvalidOperationException($"{nameof(IPopover.Content)} can't be null here.");
                var contentPage = new ContentPage
                {
                    Content = view,
                    Parent = Application.Current?.MainPage
                };

                contentPage.SetBinding(BindableObject.BindingContextProperty, new Binding { Source = virtualView, Path = BindableObject.BindingContextProperty.PropertyName });

                return (PageHandler)contentPage.ToHandler(mauiContext);
            }
        }

        private static async void MapHide(PopoverHandler handler, IPopover popover, object arg3)
        {
            var vc = handler.PlatformView.ViewController;
            if (vc is not null)
            {
                await vc.DismissViewControllerAsync(true);
            }

            //handler.DisconnectHandler(handler.PlatformView);
        }
    }
}

#endif