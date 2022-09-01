namespace SimpleToolkit.SimpleShell.Playground
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (MauiProgram.UseSimpleShell)
                MainPage = new SimpleAppShell();
            else
                MainPage = new NormalAppShell();
        }
    }
}