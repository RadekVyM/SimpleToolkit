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

public interface IBaseSimpleShellSectionHandler
{
    void RefreshGroupContainers();
    void SetRootPageContainer(IView? view);
    void SetVirtualView(IElement view);
    ShellSection VirtualView { get; }
    PlatformView PlatformView { get; }
}