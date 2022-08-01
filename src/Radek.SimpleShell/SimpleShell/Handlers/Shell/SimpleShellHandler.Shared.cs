using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
#if ANDROID
using PlatformShell = Android.Views.View;
#elif __IOS__ || MACCATALYST
using PlatformShell = UIKit.UIView;
#elif WINDOWS
using PlatformShell = Microsoft.UI.Xaml.FrameworkElement;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformShell = System.Object;
#endif

namespace Radek.SimpleShell.Handlers
{
#if ANDROID || __IOS__ || MACCATALYST || WINDOWS

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
            if (VirtualView.Content is null)
            {
                throw new ArgumentNullException("Content property cannot be null");
            }

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

        private static void MapCurrentItem(SimpleShellHandler handler, ISimpleShell shell)
        {
            handler.SwitchShellItem(shell.CurrentItem, true);
        }
    }

#else

    public partial class SimpleShellHandler : ElementHandler<ISimpleShell, System.Object>
    {
        public SimpleShellHandler(IPropertyMapper mapper, CommandMapper commandMapper)
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
