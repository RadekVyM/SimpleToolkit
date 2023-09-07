namespace SimpleToolkit.SimpleShell.Playground
{
    internal enum AppShellType
    {
        Normal, Playground, NoTabs
    }

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = MauiProgram.UsedAppShell switch
            {
                AppShellType.Normal => new NormalAppShell(),
                AppShellType.NoTabs => new NoTabsAppShell(),
                _ => new PlaygroundAppShell()
            };
        }
    }
}