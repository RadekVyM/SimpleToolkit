using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Extensions;
using SimpleToolkit.SimpleShell.NavigationManager;
using Microsoft.Maui.Controls.Internals;
#if ANDROID
using PlatformView = Android.Views.View;
#elif IOS || MACCATALYST
using PlatformView = UIKit.UIView;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
#else
using PlatformView = System.Object;
#endif

namespace SimpleToolkit.SimpleShell.Handlers;

// Based on https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/Handlers/Shell/ShellSectionHandler.Windows.cs

public abstract partial class BaseSimpleShellSectionHandler<PlatformT> : ElementHandler<ShellSection, PlatformT>, IBaseSimpleShellSectionHandler, IAppearanceObserver where PlatformT : class
{
    public static PropertyMapper<ShellSection, BaseSimpleShellSectionHandler<PlatformT>> Mapper =
        new PropertyMapper<ShellSection, BaseSimpleShellSectionHandler<PlatformT>>(ElementMapper)
        {
            [nameof(ShellSection.CurrentItem)] = MapCurrentItem,
            [SimpleShell.ShellGroupContainerProperty.PropertyName] = MapShellGroupContainer
        };

    public static CommandMapper<ShellSection, BaseSimpleShellSectionHandler<PlatformT>> CommandMapper =
        new CommandMapper<ShellSection, BaseSimpleShellSectionHandler<PlatformT>>(ElementCommandMapper)
        {
            [nameof(IStackNavigation.RequestNavigation)] = RequestNavigation
        };

    protected IView rootPageContainer;
    protected ISimpleStackNavigationManager navigationManager;
    protected ShellSection shellSection;

    PlatformView IBaseSimpleShellSectionHandler.PlatformView => this.PlatformView as PlatformView;


    public BaseSimpleShellSectionHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
    {
    }

    public BaseSimpleShellSectionHandler() : base(Mapper, CommandMapper)
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
            DisconnectNavigationManager(navigationManager.StackNavigation);

            if (view is IStackNavigation stackNavigation)
                ConnectNavigationManager(stackNavigation);
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

    public virtual void RefreshGroupContainers()
    {
        if (!navigationManager.AlreadyNavigated)
            return;

        var shell = VirtualView.FindParentOfType<SimpleShell>();
        var shellSectionContainer = GetShellGroupContainer(VirtualView);
        var shellItemContainer = GetShellItemContainer(VirtualView);

        navigationManager?.UpdateGroupContainers(shell, shellSectionContainer, shellItemContainer);
    }

    public virtual void OnAppearanceChanged(ShellAppearance appearance)
    {
    }

    protected abstract void ConnectNavigationManager(IStackNavigation stackNavigation);

    protected abstract void DisconnectNavigationManager(IStackNavigation stackNavigation);

    protected virtual void OnNavigationRequested(object sender, NavigationRequestedEventArgs e)
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

    protected virtual ISimpleStackNavigationManager CreateNavigationManager() =>
        throw new NotImplementedException("CreateNavigationManager() method must be implemented");

    public static void RequestNavigation(BaseSimpleShellSectionHandler<PlatformT> handler, IStackNavigation view, object arg3)
    {
        if (arg3 is ArgsNavigationRequest nr)
        {
            var shell = handler.VirtualView.FindParentOfType<SimpleShell>();
            var shellSectionContainer = GetShellGroupContainer(handler.VirtualView);
            var shellItemContainer = GetShellItemContainer(handler.VirtualView);

            handler.navigationManager?.NavigateTo(nr, shell, shellSectionContainer, shellItemContainer);
        }
        else
        {
            throw new InvalidOperationException("Args must be ArgsNavigationRequest");
        }
    }

    public static void MapCurrentItem(BaseSimpleShellSectionHandler<PlatformT> handler, ShellSection item)
    {
        handler.SyncNavigationStack(false, null);
    }

    public static void MapShellGroupContainer(BaseSimpleShellSectionHandler<PlatformT> handler, ShellSection item)
    {
        handler.RefreshGroupContainers();
    }

    private static IView GetShellItemContainer(ShellSection shellSection)
    {
        if (shellSection.Parent is ShellItem shellItem)
            return GetShellGroupContainer(shellItem);

        return null;
    }

    private static IView GetShellGroupContainer(ShellGroupItem group)
    {
        var container = SimpleShell.GetShellGroupContainer(group);

        if (container is null)
        {
            var template = SimpleShell.GetShellGroupContainerTemplate(group);

            if (template is not null)
                container = template.CreateContent() as IView ?? throw new InvalidOperationException("ShellGroupContainer has to implement the IView interface");

            SimpleShell.SetShellGroupContainer(group, container);
        }

        container?.ToHandler(group.Handler.MauiContext);

        if (container is BindableObject bindable && !bindable.IsSet(BindableObject.BindingContextProperty))
            bindable.BindingContext = group;
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