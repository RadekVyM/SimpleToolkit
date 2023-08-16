using SimpleToolkit.SimpleShell.NavigationManager;
#if ANDROID
using PageContainer = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
#elif IOS || MACCATALYST
using SimpleToolkit.SimpleShell.Platform;
using PageContainer = UIKit.UIView;
#elif WINDOWS
using PageContainer = Microsoft.UI.Xaml.Controls.Grid;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PageContainer = System.Object;
#endif

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class SimpleShellSectionHandler : BaseSimpleShellSectionHandler<PageContainer>
{
    private new SimpleStackNavigationManager navigationManager => base.navigationManager as SimpleStackNavigationManager;


    public SimpleShellSectionHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
    {
    }

    public SimpleShellSectionHandler() : base(Mapper, CommandMapper)
    {
    }


    protected override void ConnectNavigationManager(IStackNavigation stackNavigation)
    {
        navigationManager.Connect(stackNavigation, GetPageContainer(PlatformView));
    }

    protected override void DisconnectNavigationManager(IStackNavigation stackNavigation)
    {
        navigationManager.Disconnect(navigationManager.StackNavigation, PlatformView);
    }

    protected override void ConnectHandler(PageContainer platformView)
    {
        navigationManager?.Connect(VirtualView, GetPageContainer(platformView));
        navigationManager?.UpdateRootPageContainer(rootPageContainer);
        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(PageContainer platformView)
    {
        navigationManager?.Disconnect(VirtualView, platformView);

#if IOS || MACCATALYST
        var controller = platformView.NextResponder as SimpleShellSectionController;
        controller.PopGestureRecognized -= NavigationControllerPopGestureRecognized;
        controller.RemoveFromParentViewController();
        controller.DidMoveToParentViewController(null);
#endif

        base.DisconnectHandler(platformView);
    }

    protected override ISimpleStackNavigationManager CreateNavigationManager() =>
        base.navigationManager ??= new SimpleStackNavigationManager(MauiContext ?? throw new InvalidOperationException("MauiContext cannot be null"));

    private static PageContainer GetPageContainer(PageContainer platformView)
    {
#if IOS || MACCATALYST
        var controller = platformView.NextResponder as SimpleShellSectionController;
        var contentController = controller.ViewControllers.FirstOrDefault() as SimpleShellSectionContentController;

        return contentController?.View ?? platformView;
#else
        return platformView;
#endif
    }
}
