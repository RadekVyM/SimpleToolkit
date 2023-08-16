using SimpleToolkit.SimpleShell.Transitions;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public class NativeSimpleStackNavigationManager : BaseSimpleStackNavigationManager, ISimpleStackNavigationManager
{
    public NativeSimpleStackNavigationManager(IMauiContext mauiContext) : base(mauiContext) { }

    protected override void NavigateToPage(
        SimpleShellTransitionType transitionType,
        NavigationRequest args,
        SimpleShell shell,
        IReadOnlyList<IView> newPageStack,
        IView previousShellSectionContainer,
        IView previousPage,
        bool isPreviousPageRoot)
    {
        throw new NotImplementedException();
    }

    protected override void OnBackStackChanged(IReadOnlyList<IView> newPageStack)
    {
        throw new NotImplementedException();
    }
}