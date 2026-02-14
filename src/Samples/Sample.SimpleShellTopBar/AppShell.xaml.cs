using Sample.SimpleShellTopBar.Views.Pages;

namespace Sample.SimpleShellTopBar;

public partial class AppShell : SimpleToolkit.SimpleShell.SimpleShell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(FirstDetailPage), typeof(FirstDetailPage));
        Routing.RegisterRoute(nameof(SecondDetailPage), typeof(SecondDetailPage));
    }

    private async void ShellItemButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var shellItem = button?.BindingContext as BaseShellItem;

        if (shellItem?.Route is null)
            return;

        // Navigate to a new tab if it is not the current tab
        if (!CurrentState.Location.OriginalString.Contains(shellItem.Route))
            await GoToAsync($"///{shellItem.Route}");
    }
}