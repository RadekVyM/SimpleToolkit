#if !(ANDROID || IOS || MACCATALYST || WINDOWS)

namespace SimpleToolkit.SimpleShell.NavigationManager;

public partial class NativeSimpleStackNavigationManager
{
    protected async Task NavigateNativelyToPageInContainer(
        SimpleShell shell,
        IView previousShellItemContainer,
        IView previousShellSectionContainer,
        IView previousPage,
        bool isPreviousPageRoot)
    {
        throw new NotImplementedException();
    }

    protected void HandleNewStack(IReadOnlyList<IView> newPageStack, bool animated = true)
    {
        NavigationStack = newPageStack;
        throw new NotImplementedException();
    }
}

#endif