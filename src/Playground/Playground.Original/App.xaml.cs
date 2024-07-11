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

        RefreshShell(MauiProgram.UsedAppShell);
    }

    public void RefreshShell(AppShellType type)
    {
        MainPage = type switch
        {
            AppShellType.Normal => new NormalAppShell(),
            _ => new PlaygroundAppShell()
        };
    }
}