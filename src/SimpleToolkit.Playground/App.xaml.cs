namespace SimpleToolkit.SimpleShell.Playground
{
    internal enum AppShellType
    {
        Normal, Sample, Playground
    }

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = MauiProgram.UsedAppShell switch
            {
                AppShellType.Normal => new NormalAppShell(),
                AppShellType.Sample => new SampleAppShell(),
                _ => new PlaygroundAppShell()
            };
        }
    }
}