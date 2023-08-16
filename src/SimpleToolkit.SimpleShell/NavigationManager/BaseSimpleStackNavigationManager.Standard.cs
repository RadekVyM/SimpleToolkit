#if !(ANDROID || IOS || MACCATALYST || WINDOWS)

namespace SimpleToolkit.SimpleShell.NavigationManager;

public abstract partial class BaseSimpleStackNavigationManager
{
    protected virtual void ReplaceRootPageContainer(IView rootPageContainer, bool isCurrentPageRoot)
    {
    }
}

#endif