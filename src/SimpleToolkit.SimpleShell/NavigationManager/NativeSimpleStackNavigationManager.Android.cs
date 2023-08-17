#if ANDROID

using SimpleToolkit.SimpleShell.Transitions;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public partial class NativeSimpleStackNavigationManager
{
    protected override void OnBackStackChanged(IReadOnlyList<IView> newPageStack)
    {
        HandleNewStack(newPageStack);
        FireNavigationFinished();

        return;
    }

    protected void HandleNewStack(IReadOnlyList<IView> newPageStack, bool animated = true)
    {
        
    }
}

#endif