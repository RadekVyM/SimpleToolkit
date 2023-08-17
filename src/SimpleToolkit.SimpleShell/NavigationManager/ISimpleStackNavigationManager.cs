using SimpleToolkit.SimpleShell.Handlers;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public interface ISimpleStackNavigationManager
{
    IStackNavigation StackNavigation { get; }
    IReadOnlyList<IView> NavigationStack { get; }

    void NavigateTo(ArgsNavigationRequest args, SimpleShell shell, IView shellSectionContainer);
    void UpdateRootPageContainer(IView rootPageContainer);
}