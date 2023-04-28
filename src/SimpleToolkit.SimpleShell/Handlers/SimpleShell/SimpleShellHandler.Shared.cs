#if ANDROID || IOS || MACCATALYST || WINDOWS

using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Extensions;
#if ANDROID
using PlatformShell = Android.Views.ViewGroup;
#elif IOS || MACCATALYST
using PlatformShell = UIKit.UIView;
#elif WINDOWS
using Microsoft.UI.Xaml;
using PlatformShell = Microsoft.UI.Xaml.Controls.Border;
#else
using PlatformShell = System.Object;
#endif

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellHandler
    {
        public static IPropertyMapper<ISimpleShell, SimpleShellHandler> Mapper = new PropertyMapper<ISimpleShell, SimpleShellHandler>(ViewHandler.ViewMapper)
        {
            [nameof(ISimpleShell.CurrentItem)] = MapCurrentItem,
            [nameof(ISimpleShell.Content)] = MapContent,
            [nameof(ISimpleShell.RootPageOverlay)] = MapRootPageOverlay,
        };

        public static CommandMapper<ISimpleShell, SimpleShellHandler> CommandMapper = new(ViewHandler.ViewCommandMapper)
        {
        };

        protected bool platformViewHasContent = false;
        protected ISimpleNavigationHost navigationHost;

        public ShellItem SelectedItem { get; protected set; }


        public SimpleShellHandler(IPropertyMapper mapper, CommandMapper commandMapper)
            : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
        {
        }

        public SimpleShellHandler()
            : base(Mapper, CommandMapper)
        {
        }


        public virtual void SwitchShellItem(ShellItem newItem, bool animate = true)
        {
            if (!platformViewHasContent)
                return;

            // Implicit items aren't items that are surfaced to the user 
            // or data structures. So, we just want to find the element
            // the user defined on Shell
            if (RoutingExtensions.IsImplicit(newItem))
            {
                if (RoutingExtensions.IsImplicit(newItem.CurrentItem))
                    SelectedItem = newItem.CurrentItem.CurrentItem;
                else
                    SelectedItem = newItem.CurrentItem;
            }
            else
            {
                SelectedItem = newItem;
            }

            var handler = CreateShellItemHandler();
            if (handler.VirtualView != newItem)
                handler.SetVirtualView(newItem);

            UpdateRootPageOverlay(VirtualView.RootPageOverlay);
        }

        protected virtual SimpleShellItemHandler CreateShellItemHandler()
        {
            var itemHandler = (SimpleShellItemHandler)VirtualView.CurrentItem.ToHandler(MauiContext);

            if (itemHandler.PlatformView != GetNavigationHostContent() && navigationHost?.Handler is SimpleNavigationHostHandler navHostHandler)
                navHostHandler.SetContent(itemHandler.PlatformView);

            if (itemHandler.VirtualView != VirtualView.CurrentItem)
                itemHandler.SetVirtualView(VirtualView.CurrentItem);

            return itemHandler;
        }

        protected virtual void UpdateContent(IView content)
        {
            if (navigationHost is not null && navigationHost?.Handler is SimpleNavigationHostHandler navHostHandler)
                navHostHandler.SetContent(null);

            var platformContent = content.ToPlatform(MauiContext);
            platformViewHasContent = platformContent is not null;

#if ANDROID
            if (platformViewHasContent && PlatformView.GetChildAt(0) != platformContent)
            {
                PlatformView.RemoveAllViews();
                PlatformView.AddView(platformContent);
            }
#elif IOS || MACCATALYST
            if (platformViewHasContent && PlatformView.Subviews.FirstOrDefault() != platformContent)
            {
                PlatformView.ClearSubviews();
                PlatformView.AddSubview(platformContent);
            }
#elif WINDOWS
            if (platformViewHasContent && PlatformView.Child != platformContent)
            {
                PlatformView.Child = platformContent;
            }
#endif

            navigationHost = VirtualView.Content.FindSimpleNavigationHost();
        }

        protected virtual void UpdateRootPageOverlay(IView view)
        {
            if (VirtualView?.CurrentItem?.Handler is null || VirtualView.CurrentItem.Handler is not SimpleShellItemHandler shellItemHandler)
                return;

            shellItemHandler.SetRootPageOverlay(view);
        }

        protected override void DisconnectHandler(PlatformShell platformView)
        {
            base.DisconnectHandler(platformView);

            navigationHost = null;
            SelectedItem = null;

#if IOS || MACCATALYST
            navigationController.PopGestureRecognized -= NavigationControllerPopGestureRecognized;
            navigationController.RemoveFromParentViewController();
            navigationController = null;
#endif
        }

        public static void MapCurrentItem(SimpleShellHandler handler, ISimpleShell shell)
        {
            handler.SwitchShellItem(shell.CurrentItem, true);
        }

        public static void MapContent(SimpleShellHandler handler, ISimpleShell shell)
        {
            handler.UpdateContent(shell.Content);
            handler.UpdateValue(nameof(ISimpleShell.CurrentItem));
        }

        public static void MapRootPageOverlay(SimpleShellHandler handler, ISimpleShell shell)
        {
            handler.UpdateRootPageOverlay(shell.RootPageOverlay);
        }
    }
}

#endif