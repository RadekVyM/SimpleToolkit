﻿using SimpleToolkit.SimpleShell.NavigationManager;
#if ANDROID
using PageContainer = Android.Widget.FrameLayout;
#elif IOS || MACCATALYST
using SimpleToolkit.SimpleShell.Platform;
using PageContainer = UIKit.UIView;
#elif WINDOWS
using PageContainer = Microsoft.UI.Xaml.Controls.Frame;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PageContainer = System.Object;
#endif

namespace SimpleToolkit.SimpleShell.Handlers;

public partial class PlatformSimpleShellSectionHandler : BaseSimpleShellSectionHandler<PageContainer>
{
    private new PlatformSimpleStackNavigationManager? navigationManager => base.navigationManager as PlatformSimpleStackNavigationManager;


    public PlatformSimpleShellSectionHandler(IPropertyMapper mapper, CommandMapper commandMapper)
        : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
    {
    }

    public PlatformSimpleShellSectionHandler() : base(Mapper, CommandMapper)
    {
    }


    protected override void ConnectNavigationManager(IStackNavigation stackNavigation)
    {
        navigationManager?.Connect(stackNavigation, PlatformView, RootContentContainer);
    }

    protected override void DisconnectNavigationManager(IStackNavigation stackNavigation)
    {
        navigationManager?.Disconnect(navigationManager.StackNavigation, PlatformView, RootContentContainer);
    }

    protected override void ConnectHandler(PageContainer platformView)
    {
        navigationManager?.Connect(VirtualView, PlatformView, RootContentContainer);
        navigationManager?.UpdateRootPageContainer(rootPageContainer);
        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(PageContainer platformView)
    {
        navigationManager?.Disconnect(VirtualView, platformView, RootContentContainer);

#if IOS || MACCATALYST
        var controller = platformView.NextResponder as UIKit.UIViewController;
        controller?.RemoveFromParentViewController();
        controller?.DidMoveToParentViewController(null);
#endif

        base.DisconnectHandler(platformView);
    }

    protected override ISimpleStackNavigationManager CreateNavigationManager() =>
        base.navigationManager ??= new PlatformSimpleStackNavigationManager(MauiContext ?? throw new InvalidOperationException("MauiContext cannot be null"));
}