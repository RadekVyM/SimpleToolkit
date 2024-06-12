using Sample.SimpleShellSharpnadoTabs.Views.Pages;
using SimpleToolkit.Core;

namespace Sample.SimpleShellSharpnadoTabs;

public partial class AppShell : SimpleToolkit.SimpleShell.SimpleShell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(FirstDetailPage), typeof(FirstDetailPage));

        Loaded += AppShellLoaded;
    }

    private void AppShellLoaded(object? sender, EventArgs e)
    {
        this.Window.SubscribeToSafeAreaChanges(safeArea => contentView.Padding = safeArea);
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);
        // Make sure that the index is always correct
        tabHostView.SelectedIndex = ShellSections.ToList().IndexOf(CurrentShellSection);
    }

    private async void SelectedTabIndexChanged(object sender, SelectedPositionChangedEventArgs e)
    {
        if (e.SelectedPosition is not int position)
            return;

        var shellItem = ShellSections[position];

        // Navigate to a new tab if it is not the current tab
        if (!CurrentState.Location.OriginalString.Contains(shellItem.Route))
            await GoToAsync($"///{shellItem.Route}");
    }
}