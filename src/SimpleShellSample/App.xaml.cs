namespace SimpleShellSample;

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
