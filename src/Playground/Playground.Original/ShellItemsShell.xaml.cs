using Playground.Original.Views.Pages;
using SimpleToolkit.Core;
using SimpleToolkit.SimpleShell;
using SimpleToolkit.SimpleShell.Extensions;

namespace Playground.Original;

public partial class ShellItemsShell : SimpleShell
{
	public ShellItemsShell()
	{
        Routing.RegisterRoute(nameof(FirstYellowDetailPage), typeof(FirstYellowDetailPage));
        Routing.RegisterRoute(nameof(SecondYellowDetailPage), typeof(SecondYellowDetailPage));
        Routing.RegisterRoute(nameof(ThirdYellowDetailPage), typeof(ThirdYellowDetailPage));
        Routing.RegisterRoute(nameof(FourthYellowDetailPage), typeof(FourthYellowDetailPage));
        Routing.RegisterRoute(nameof(FirstGreenDetailPage), typeof(FirstGreenDetailPage));

        InitializeComponent();

        Loaded += AppShellLoaded;
        Unloaded += AppShellUnloaded;

		this.SetTransition(SimpleShell.UsesPlatformTransitions ?
			Transitions.CustomPlatformTransition :
			Transitions.DefaultUniversalTransition);
	}


    private void AppShellLoaded(object sender, EventArgs e)
    {
        Window.SubscribeToSafeAreaChanges(OnSafeAreaChanged);
    }

    private void AppShellUnloaded(object sender, EventArgs e)
    {
        Window.UnsubscribeFromSafeAreaChanges(OnSafeAreaChanged);
    }

    private void OnSafeAreaChanged(Thickness safeAreaPadding)
    {
        rootContainer.Padding = safeAreaPadding;
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);

        // Just to check if handlers are set to correct items
        var shellItemHandlers = Items.Select(i => i.Handler).ToArray();
        var shellSectionHandlers = ShellSections?.Select(i => i.Handler).ToArray();
        var items = Items;

        var tabBars = TabBars;
        var flyoutItems = FlyoutItems;
        //var menuItems = items.OfType<MenuShellItem>().Select(m => m.MenuItem);
    }

    private async void ShellItemButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var shellItem = button.BindingContext as BaseShellItem;

        // Navigate to a new tab if it is not the current tab
        if (!CurrentState.Location.OriginalString.Contains(shellItem.Route))
            await GoToAsync($"///{shellItem.Route}", true);
    }

    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await GoToAsync("..");
    }
}