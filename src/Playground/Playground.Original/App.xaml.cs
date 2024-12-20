namespace Playground.Original;

public enum AppShellType
{
    Normal, Playground
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

    private Page GetShellPage(AppShellType type) => type switch
    {
        AppShellType.Normal => new NormalAppShell(),
        _ => new PlaygroundAppShell()
    };
}