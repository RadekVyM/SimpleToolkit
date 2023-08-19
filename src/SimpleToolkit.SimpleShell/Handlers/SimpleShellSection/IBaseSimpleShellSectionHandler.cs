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
    void SetRootPageContainer(IView view);
    ShellSection VirtualView { get; }
    void SetVirtualView(IElement view);
    PlatformView PlatformView { get; }

}