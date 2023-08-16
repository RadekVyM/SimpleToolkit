namespace SimpleToolkit.SimpleShell.NavigationManager;

public class NativeSimpleStackNavigationManager : ISimpleStackNavigationManager
{
    protected IMauiContext mauiContext;

    public IStackNavigation StackNavigation => throw new NotImplementedException();
    public IReadOnlyList<IView> NavigationStack => throw new NotImplementedException();


    public NativeSimpleStackNavigationManager(IMauiContext mauiContext)
    {
        this.mauiContext = mauiContext;
    }


    public void NavigateTo(NavigationRequest args, SimpleShell shell, IView shellSectionContainer)
    {
        throw new NotImplementedException();
    }

    public void UpdateRootPageContainer(IView rootPageContainer)
    {
        throw new NotImplementedException();
    }
}