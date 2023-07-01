using SimpleToolkit.SimpleShell.Extensions;
using SimpleToolkit.SimpleShell.NavigationManager;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Platform;
#if ANDROID
using PageContainer = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
#elif IOS || MACCATALYST
using PageContainer = UIKit.UIView;
#elif WINDOWS
using PageContainer = Microsoft.UI.Xaml.Controls.Grid;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PageContainer = System.Object;
#endif

namespace SimpleToolkit.SimpleShell.Handlers
{
    public partial class SimpleShellSectionHandler : IAppearanceObserver
    {
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

        private IView rootPageContainer;

        protected SimpleStackNavigationManager navigationManager;
        protected ShellSection shellSection;


        public SimpleShellSectionHandler(IPropertyMapper mapper, CommandMapper commandMapper)
            : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
        {
        }

        public SimpleShellSectionHandler() : base(Mapper, CommandMapper)
        {
        }


        public override void SetVirtualView(IElement view)
        {
            if (shellSection is not null)
            {
                ((IShellSectionController)shellSection).NavigationRequested -= OnNavigationRequested;
                ((IShellController)shellSection.FindParentOfType<SimpleShell>()).RemoveAppearanceObserver(this);
            }

            // If we've already connected to the navigation manager
            // then we need to make sure to disconnect and connect up to 
            // the new incoming virtual view
            if (navigationManager?.StackNavigation is not null &&
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
            if (shellSection is not null)
            {
                ((IShellSectionController)shellSection).NavigationRequested += OnNavigationRequested;
                ((IShellController)shellSection.FindParentOfType<SimpleShell>()).AddAppearanceObserver(this, shellSection);
            }
        }

        public virtual void SetRootPageContainer(IView view)
        {
            rootPageContainer = view;
            navigationManager?.UpdateRootPageContainer(view);
        }

        public virtual void OnAppearanceChanged(ShellAppearance appearance)
        {
        }

        protected override void ConnectHandler(PageContainer platformView)
        {
            navigationManager?.Connect(VirtualView, platformView);
            navigationManager?.UpdateRootPageContainer(rootPageContainer);
            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(PageContainer platformView)
        {
            navigationManager?.Disconnect(VirtualView, platformView);
            base.DisconnectHandler(platformView);
        }

        void OnNavigationRequested(object sender, NavigationRequestedEventArgs e)
        {
            SyncNavigationStack(e.Animated, e);
        }

        protected virtual void SyncNavigationStack(bool animated, NavigationRequestedEventArgs e)
        {
            var pageStack = new List<IView>()
            {
                (VirtualView.CurrentItem as IShellContentController).GetOrCreateContent()
            };

#if DEBUG
            LogStack(e, pageStack, VirtualView);
#endif

            if (e?.RequestType != NavigationRequestType.PopToRoot) // See https://github.com/dotnet/maui/pull/10653
            {
                for (var i = 1; i < VirtualView.Navigation.NavigationStack.Count; i++)
                {
                    pageStack.Add(VirtualView.Navigation.NavigationStack[i]);
                }
            }
            
            // The point of this is to push the shell navigation over to using the INavigationStack
            // work flow. Ideally we rewrite all the push/pop/etc.. parts inside ShellSection.cs
            // to just use INavigationStack but that will be easier once all platforms are using
            // ShellHandler
            (VirtualView as IStackNavigation).RequestNavigation(new ArgsNavigationRequest(pageStack, animated, e?.RequestType ?? NavigationRequestType.Unknown));
        }

        protected virtual SimpleStackNavigationManager CreateNavigationManager() =>
            navigationManager ??= new SimpleStackNavigationManager(MauiContext ?? throw new InvalidOperationException("MauiContext cannot be null"));

        public static void RequestNavigation(SimpleShellSectionHandler handler, IStackNavigation view, object arg3)
        {
            if (arg3 is NavigationRequest nr)
            {
                var shell = handler.VirtualView.FindParentOfType<SimpleShell>();
                var container = GetShellSectionContainer(handler.VirtualView);

                handler.navigationManager?.NavigateTo(nr, shell, container);
            }
            else
            {
                throw new InvalidOperationException("Args must be NavigationRequest");
            }
        }

        public static void MapCurrentItem(SimpleShellSectionHandler handler, ShellSection item)
        {
            handler.SyncNavigationStack(false, null);
        }

        private static IView GetShellSectionContainer(ShellSection section)
        {
            var container = SimpleShell.GetShellSectionContainer(section);

            if (container is null)
            {
                var template = SimpleShell.GetShellSectionContainerTemplate(section);

                if (template is not null)
                {
                    container = template.CreateContent() as IView ?? throw new InvalidOperationException("ShellSectionContainer has to implement the IView interface");
                }

                SimpleShell.SetShellSectionContainer(section, container);
            }

            container?.ToHandler(section.Handler.MauiContext);

            if (container is BindableObject bindable && !bindable.IsSet(BindableObject.BindingContextProperty))
                bindable.BindingContext = section;

            return container;
        }

        private static void LogStack(NavigationRequestedEventArgs e, List<IView> pageStack, ShellSection virtualView)
        {
            var requestType = e?.RequestType.ToString() ?? "null";
            var sequence = string.Join(" -> ", pageStack
                .Select(p => p.GetType().Name)
                .Concat(virtualView.Navigation.NavigationStack.Where(p => p is not null).Select(p => p.GetType().Name)));
            System.Diagnostics.Debug.WriteLine($"Type: {requestType}\t\t Stack: {sequence}");
        }
    }

    public class ArgsNavigationRequest : NavigationRequest
    {
        public NavigationRequestType RequestType { get; }

        public ArgsNavigationRequest(IReadOnlyList<IView> newNavigationStack, bool animated, NavigationRequestType requestType) : base(newNavigationStack, animated)
        {
            RequestType = requestType;
        }
    }
}
