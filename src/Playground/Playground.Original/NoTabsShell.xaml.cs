using Playground.Original.Views.Pages;
using SimpleToolkit.SimpleShell;
using SimpleToolkit.SimpleShell.Extensions;

namespace Playground.Original;

public partial class NoTabsShell : SimpleShell
{
    public NoTabsShell()
    {
        Routing.RegisterRoute(nameof(FirstYellowDetailPage), typeof(FirstYellowDetailPage));
        Routing.RegisterRoute(nameof(SecondYellowDetailPage), typeof(SecondYellowDetailPage));
        Routing.RegisterRoute(nameof(ThirdYellowDetailPage), typeof(ThirdYellowDetailPage));
        Routing.RegisterRoute(nameof(FourthYellowDetailPage), typeof(FourthYellowDetailPage));
        Routing.RegisterRoute(nameof(FirstGreenDetailPage), typeof(FirstGreenDetailPage));

        InitializeComponent();

        this.SetTransition(Transitions.DefaultUniversalTransition);
    }


    private async void ShellItemButtonClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button || button.BindingContext is not BaseShellItem shellItem)
            return;

        // Navigate to a new tab if it is not the current tab
        if (!CurrentState.Location.OriginalString.Contains(shellItem.Route))
            await GoToAsync($"///{shellItem.Route}", true);
    }

    private async void BackButtonClicked(object? sender, EventArgs e)
    {
        await GoToAsync("..");
    }
}