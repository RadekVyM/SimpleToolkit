using Playground.Core.Views.Pages;
using SimpleToolkit.SimpleShell;

namespace Playground.Containers;

public partial class AppShell : SimpleShell
{
    private int itemContainerIndex = 0;
    private int firstTabContainerIndex = 0;
    private int secondTabContainerIndex = 0;

    public AppShell()
    {
        Routing.RegisterRoute(nameof(FirstYellowDetailPage), typeof(FirstYellowDetailPage));
        Routing.RegisterRoute(nameof(SecondYellowDetailPage), typeof(SecondYellowDetailPage));
        Routing.RegisterRoute(nameof(ThirdYellowDetailPage), typeof(ThirdYellowDetailPage));
        Routing.RegisterRoute(nameof(FourthYellowDetailPage), typeof(FourthYellowDetailPage));
        Routing.RegisterRoute(nameof(FirstGreenDetailPage), typeof(FirstGreenDetailPage));

        InitializeComponent();
    }

    private DataTemplate CreateContainer(string text)
    {
        return new DataTemplate(() =>
            new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Star),
                },
                Children =
                {
                    new Label { Text = text, Margin = 5 },
                    CreateHost()
                }
            });

        SimpleNavigationHost CreateHost()
        {
            var host = new SimpleNavigationHost();

            Grid.SetRow(host, 1);

            return host;
        }
    }

    private async void ShellItemButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var shellItem = button?.BindingContext as BaseShellItem;

        if (shellItem is null)
            return;

        // Navigate to a new tab if it is not the current tab
        if (!CurrentState.Location.OriginalString.Contains(shellItem.Route))
            await GoToAsync($"///{shellItem.Route}", true);
    }

    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await GoToAsync("..");
    }

    private void RecreateItemContainerClicked(object sender, EventArgs e)
    {
        SimpleShell.SetShellGroupContainerTemplate(shellItem, CreateContainer($"Item Container: {++itemContainerIndex}"));
    }

    private void ClearItemContainerClicked(object sender, EventArgs e)
    {
        SimpleShell.SetShellGroupContainerTemplate(shellItem, null);
    }

    private void RecreateFirstTabContainerClicked(object sender, EventArgs e)
    {
        SimpleShell.SetShellGroupContainerTemplate(firstTab, CreateContainer($"First Tab Container: {++firstTabContainerIndex}"));
    }

    private void ClearFirstTabContainerClicked(object sender, EventArgs e)
    {
        SimpleShell.SetShellGroupContainerTemplate(firstTab, null);
    }

    private void RecreateSecondTabContainerClicked(object sender, EventArgs e)
    {
        SimpleShell.SetShellGroupContainerTemplate(secondTab, CreateContainer($"Second Tab Container: {++secondTabContainerIndex}"));
    }

    private void ClearSecondTabContainerClicked(object sender, EventArgs e)
    {
        SimpleShell.SetShellGroupContainerTemplate(secondTab, null);
    }
}
