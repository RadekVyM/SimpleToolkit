using Sample.SimpleShellTopBar.Views.Controls;

namespace Sample.SimpleShellTopBar.Views.Pages;

public partial class SecondRootPage : ContentPage
{
	private readonly Random random = new Random();

	public SecondRootPage()
	{
		InitializeComponent();
	}

	private void ButtonClicked(object sender, EventArgs e)
	{
		if ((Shell.Current as AppShell)?.ShellContents.FirstOrDefault((s) => s.Route == "SecondRootPage") is BadgeShellContent badgeShellContent)
		{
			var number = random.Next(0, 10);
			badgeShellContent.BadgeText = number == 0 ? null : number.ToString();
		}
	}
}