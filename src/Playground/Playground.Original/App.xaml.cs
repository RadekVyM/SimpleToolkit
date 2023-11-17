namespace Playground.Original
{
    internal enum AppShellType
    {
        Normal, Playground
    }

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = MauiProgram.UsedAppShell switch
            {
                AppShellType.Normal => new NormalAppShell(),
                _ => new PlaygroundAppShell()
            };
        }
    }
}