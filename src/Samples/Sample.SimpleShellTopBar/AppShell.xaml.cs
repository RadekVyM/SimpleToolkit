using Sample.SimpleShellTopBar.Views.Pages;
using SimpleToolkit.Core;

namespace Sample.SimpleShellTopBar;

public partial class AppShell : SimpleToolkit.SimpleShell.SimpleShell
{
    public AppShell()
    {
        InitializeComponent();

        Loaded += AppShellLoaded;
        Unloaded += AppShellUnloaded;

        Routing.RegisterRoute(nameof(FirstDetailPage), typeof(FirstDetailPage));
        Routing.RegisterRoute(nameof(SecondDetailPage), typeof(SecondDetailPage));
    }

    private void AppShellLoaded(object? sender, EventArgs e)
    {
        Window.SubscribeToSafeAreaChanges(OnSafeAreaChanged);
    }

    private void AppShellUnloaded(object? sender, EventArgs e)
    {
        Window.UnsubscribeFromSafeAreaChanges(OnSafeAreaChanged);
    }

    private void OnSafeAreaChanged(Thickness safeAreaPadding)
    {
        rootContainer.Padding = safeAreaPadding;
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