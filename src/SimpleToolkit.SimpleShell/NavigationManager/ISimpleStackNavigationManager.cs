using SimpleToolkit.SimpleShell.Handlers;

namespace SimpleToolkit.SimpleShell.NavigationManager;

public interface ISimpleStackNavigationManager
{
    IStackNavigation? StackNavigation { get; }
    IReadOnlyList<IView> NavigationStack { get; }
    bool AlreadyNavigated { get; }

    void NavigateTo(ArgsNavigationRequest args, SimpleShell shell, IView? shellSectionContainer, IView? shellItemContainer);
    void UpdateRootPageContainer(IView? rootPageContainer);
    void UpdateGroupContainers(SimpleShell shell, IView? shellSectionContainer, IView? shellItemContainer);
}