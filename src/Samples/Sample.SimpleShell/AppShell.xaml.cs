using Sample.SimpleShell.Views.Pages;
using SimpleToolkit.SimpleShell.Extensions;

namespace Sample.SimpleShell;

public partial class AppShell : SimpleToolkit.SimpleShell.SimpleShell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(YellowDetailPage), typeof(YellowDetailPage));

        this.SetTransition(Transitions.CustomPlatformTransition);
    }

    private async void ShellItemButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var shellItem = button.BindingContext as BaseShellItem;

        // Navigate to a new tab if it is not the current tab
        if (!CurrentState.Location.OriginalString.Contains(shellItem.Route))
            await GoToAsync($"///{shellItem.Route}");
    }

    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await GoToAsync("..");
    }
}