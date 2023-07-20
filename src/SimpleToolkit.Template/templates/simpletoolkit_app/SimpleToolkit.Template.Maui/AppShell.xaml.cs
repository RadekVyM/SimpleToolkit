using SimpleToolkit.Core;

namespace SimpleToolkit.Template.Maui;

public partial class AppShell : SimpleToolkit.SimpleShell.SimpleShell
{
    public AppShell()
    {
        InitializeComponent();

        Loaded += AppShellLoaded;
    }


    private static void AppShellLoaded(object sender, EventArgs e)
    {
        var shell = sender as AppShell;

        shell.Window.SubscribeToSafeAreaChanges(safeArea =>
        {
            shell.tabBar.Padding = new Thickness(safeArea.Left, 0, safeArea.Right, safeArea.Bottom);
        });
    }

    private async void ItemClicked(object sender, EventArgs e)
    {
        var button = sender as BindableObject;
        var shellItem = button.BindingContext as BaseShellItem;

        if (!CurrentState.Location.OriginalString.Contains(shellItem.Route))
            await GoToAsync($"///{shellItem.Route}", true);
    }
}