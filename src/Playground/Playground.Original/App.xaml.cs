namespace Playground.Original;

public enum AppShellType
{
    Normal, Playground, Containers, NoTabs, ShellItems
}

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    public void RefreshShell(AppShellType type)
    {
        Shell.Current.Window.Page = GetShellPage(type);
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(GetShellPage(MauiProgram.UsedAppShell));
    }

    private static Page GetShellPage(AppShellType type) => type switch
    {
        AppShellType.Normal => new NormalAppShell(),
        AppShellType.Containers => new ContainersShell(),
        AppShellType.NoTabs => new NoTabsShell(),
        AppShellType.ShellItems => new ShellItemsShell(),
        _ => new PlaygroundAppShell()
    };
}