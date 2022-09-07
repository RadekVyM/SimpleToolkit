#if ANDROID || IOS || MACCATALYST || WINDOWS

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Extensions;
#if ANDROID
using PlatformShell = Android.Views.View;
#elif IOS || MACCATALYST
using PlatformShell = UIKit.UIView;
#elif WINDOWS
using PlatformShell = Microsoft.UI.Xaml.FrameworkElement;
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
        };

        public static CommandMapper<ISimpleShell, SimpleShellHandler> CommandMapper = new(ViewHandler.ViewCommandMapper)
        {
        };

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


        protected override PlatformShell CreatePlatformView()
        {
            _ = VirtualView.Content ?? throw new ArgumentNullException("Content property cannot be null");

            var content = VirtualView.Content.ToPlatform(MauiContext);

            navigationHost = VirtualView.Content.FindSimpleNavigationHost();

            return content;
        }

        public virtual void SwitchShellItem(ShellItem newItem, bool animate = true)
        {
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
        }

        protected virtual SimpleShellItemHandler CreateShellItemHandler()
        {
            var itemHandler = (SimpleShellItemHandler)VirtualView.CurrentItem.ToHandler(MauiContext);

            if (itemHandler.PlatformView != GetNavigationHostContent())
                (navigationHost.Handler as SimpleNavigationHostHandler)?.SetContent(itemHandler.PlatformView);

            if (itemHandler.VirtualView != VirtualView.CurrentItem)
                itemHandler.SetVirtualView(VirtualView.CurrentItem);

            return itemHandler;
        }

        public static void MapCurrentItem(SimpleShellHandler handler, ISimpleShell shell)
        {
            handler.SwitchShellItem(shell.CurrentItem, true);
        }
    }
}

#endif