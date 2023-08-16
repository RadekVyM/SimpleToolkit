using Microsoft.Maui.Platform;
using SimpleToolkit.SimpleShell.Extensions;
#if ANDROID
using NavFrame = Microsoft.Maui.Controls.Platform.Compatibility.CustomFrameLayout;
using PlatformView = Android.Views.View;
#elif __IOS__ || MACCATALYST
using UIKit;
using NavFrame = UIKit.UIView;
using PlatformView = UIKit.UIView;
#elif WINDOWS
using NavFrame = Microsoft.UI.Xaml.Controls.Grid;
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using NavFrame = System.Object;
using PlatformView = System.Object;
#endif

namespace SimpleToolkit.SimpleShell.NavigationManager;

public abstract partial class BaseSimpleStackNavigationManager : ISimpleStackNavigationManager
{
    protected IMauiContext mauiContext;
    protected NavFrame navigationFrame;
    protected IView rootPageContainer;
    protected IView currentShellSectionContainer;
    protected bool isCurrentPageRoot = true;

    public IStackNavigation StackNavigation { get; protected set; }
    public IReadOnlyList<IView> NavigationStack { get; protected set; } = new List<IView>();


    public BaseSimpleStackNavigationManager(IMauiContext mauiContext)
    {
        this.mauiContext = mauiContext;
    }


    public abstract void NavigateTo(NavigationRequest args, SimpleShell shell, IView shellSectionContainer);

    protected virtual PlatformView GetPlatformView(IView view)
    {
        return view?.ToPlatform(mauiContext);
    }

    public virtual void UpdateRootPageContainer(IView rootPageContainer)
    {
        if (NavigationStack is null)
        {
            this.rootPageContainer = rootPageContainer;
            return;
        }

        ReplaceRootPageContainer(rootPageContainer, isCurrentPageRoot);
        this.rootPageContainer = rootPageContainer;
    }

    protected virtual object GetPageContainerNavHost(IView pageContainer)
    {
        return pageContainer?.FindSimpleNavigationHost()?.Handler?.PlatformView;
    }

    protected private void FireNavigationFinished()
    {
        StackNavigation?.NavigationFinished(NavigationStack);
    }
}