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

public partial class NativeSimpleShellSectionHandler : BaseSimpleShellSectionHandler<PageContainer>
{
    private new NativeSimpleStackNavigationManager navigationManager => base.navigationManager as NativeSimpleStackNavigationManager;


    public NativeSimpleShellSectionHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
    {
    }

    public NativeSimpleShellSectionHandler() : base(Mapper, CommandMapper)
    {
    }


    protected override void ConnectNavigationManager(IStackNavigation stackNavigation)
    {
        //navigationManager.Connect(stackNavigation, GetPageContainer(PlatformView));
    }

    protected override void DisconnectNavigationManager(IStackNavigation stackNavigation)
    {
        //navigationManager.Disconnect(navigationManager.StackNavigation, PlatformView);
    }

    protected override void ConnectHandler(PageContainer platformView)
    {
        //navigationManager?.Connect(VirtualView, GetPageContainer(platformView));
        navigationManager?.UpdateRootPageContainer(rootPageContainer);
        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(PageContainer platformView)
    {
        //navigationManager?.Disconnect(VirtualView, platformView);

#if IOS || MACCATALYST
        var controller = platformView.NextResponder as UIKit.UIViewController;
        controller.RemoveFromParentViewController();
        controller.DidMoveToParentViewController(null);
#endif

        base.DisconnectHandler(platformView);
    }

    protected override ISimpleStackNavigationManager CreateNavigationManager() =>
        base.navigationManager ??= new NativeSimpleStackNavigationManager(MauiContext ?? throw new InvalidOperationException("MauiContext cannot be null"));
}