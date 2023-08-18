#if !(ANDROID || IOS || MACCATALYST || WINDOWS)

namespace SimpleToolkit.SimpleShell.NavigationManager;

public partial class NativeSimpleStackNavigationManager
{
    protected override void OnBackStackChanged(IReadOnlyList<IView> newPageStack)
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