namespace Playground.Original;

internal enum AppShellType
{
    Normal, Playground
}

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        RefreshShell();
    }

    public void RefreshShell()
    {
        MainPage = MauiProgram.UsedAppShell switch
        {
            AppShellType.Normal => new NormalAppShell(),
            _ => new PlaygroundAppShell()
        };
    }
}