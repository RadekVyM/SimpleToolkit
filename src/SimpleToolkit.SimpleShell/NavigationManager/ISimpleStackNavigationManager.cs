namespace SimpleToolkit.SimpleShell.NavigationManager;

public interface ISimpleStackNavigationManager
{
    IStackNavigation StackNavigation { get; }
    IReadOnlyList<IView> NavigationStack { get; }

    void NavigateTo(NavigationRequest args, SimpleShell shell, IView shellSectionContainer);
    void UpdateRootPageContainer(IView rootPageContainer);
}