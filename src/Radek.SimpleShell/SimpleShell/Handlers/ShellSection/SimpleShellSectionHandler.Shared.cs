using Microsoft.Maui.Handlers;
using Radek.SimpleShell.NavigationManager;
#if ANDROID
using PageContainer = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
#elif __IOS__ || MACCATALYST
using PageContainer = UIKit.UIView;
#elif WINDOWS
using PageContainer = Microsoft.UI.Xaml.Controls.Frame;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PageContainer = System.Object;
#endif

namespace Radek.SimpleShell.Handlers
{
#if ANDROID || __IOS__ || MACCATALYST || WINDOWS

    public partial class SimpleShellSectionHandler
    {
        protected SimpleStackNavigationManager navigationManager;
        protected ShellSection shellSection;

        public static PropertyMapper<ShellSection, SimpleShellSectionHandler> Mapper =
                new PropertyMapper<ShellSection, SimpleShellSectionHandler>(ElementMapper)
                {
                    [nameof(ShellSection.CurrentItem)] = MapCurrentItem,
                };

        public static CommandMapper<ShellSection, SimpleShellSectionHandler> CommandMapper =
                new CommandMapper<ShellSection, SimpleShellSectionHandler>(ElementCommandMapper)
                {
                    [nameof(IStackNavigation.RequestNavigation)] = RequestNavigation
                };

        public SimpleShellSectionHandler(IPropertyMapper mapper, CommandMapper commandMapper)
            : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
        {
        }


        public SimpleShellSectionHandler() : base(Mapper, CommandMapper)
        {
        }


        public override void SetVirtualView(IElement view)
        {
            if (shellSection != null)
            {
                ((IShellSectionController)shellSection).NavigationRequested -= OnNavigationRequested;
                ((IShellController)shellSection.FindParentOfType<SimpleShell>()).RemoveAppearanceObserver(this);
            }

            // If we've already connected to the navigation manager
            // then we need to make sure to disconnect and connect up to 
            // the new incoming virtual view
            if (navigationManager?.StackNavigation != null &&
                navigationManager.StackNavigation != view)
            {
                navigationManager.Disconnect(navigationManager.StackNavigation, PlatformView);

                if (view is IStackNavigation stackNavigation)
                {
                    navigationManager.Connect(stackNavigation, PlatformView);
                }
            }

            base.SetVirtualView(view);

            shellSection = (ShellSection)view;
            if (shellSection != null)
            {
                ((IShellSectionController)shellSection).NavigationRequested += OnNavigationRequested;
                ((IShellController)shellSection.FindParentOfType<SimpleShell>()).AddAppearanceObserver(this, shellSection);
            }
        }

        protected override void ConnectHandler(PageContainer platformView)
        {
            navigationManager?.Connect(VirtualView, platformView);
            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(PageContainer platformView)
        {
            navigationManager?.Disconnect(VirtualView, platformView);
            base.DisconnectHandler(platformView);
        }

        void OnNavigationRequested(object sender, object e)
        {
            SyncNavigationStack(false);
        }

        protected virtual void SyncNavigationStack(bool animated)
        {
            List<IView> pageStack = new List<IView>()
            {
                (VirtualView.CurrentItem as IShellContentController).GetOrCreateContent()
            };

            for (var i = 1; i < VirtualView.Navigation.NavigationStack.Count; i++)
            {
                pageStack.Add(VirtualView.Navigation.NavigationStack[i]);
            }

            // The point of this is to push the shell navigation over to using the INavigationStack
            // work flow. Ideally we rewrite all the push/pop/etc.. parts inside ShellSection.cs
            // to just use INavigationStack but that will be easier once all platforms are using
            // ShellHandler
            (VirtualView as IStackNavigation)
                .RequestNavigation(new NavigationRequest(pageStack, animated));
        }

        protected virtual SimpleStackNavigationManager CreateNavigationManager() =>
            navigationManager ??= new SimpleStackNavigationManager(MauiContext ?? throw new InvalidOperationException("MauiContext cannot be null"));

        public void OnAppearanceChanged(ShellAppearance appearance)
        {
        }

        public static void RequestNavigation(SimpleShellSectionHandler handler, IStackNavigation view, object arg3)
        {
            if (arg3 is NavigationRequest nr)
            {
                handler.navigationManager?.NavigateTo(nr);
            }
            else
            {
                throw new InvalidOperationException("Args must be NavigationRequest");
            }
        }

        public static void MapCurrentItem(SimpleShellSectionHandler handler, ShellSection item)
        {
            handler.SyncNavigationStack(false);
        }
    }

#else

    public partial class SimpleShellSectionHandler : ElementHandler<ShellSection, System.Object>
    {
        public SimpleShellSectionHandler(IPropertyMapper mapper, CommandMapper commandMapper)
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
