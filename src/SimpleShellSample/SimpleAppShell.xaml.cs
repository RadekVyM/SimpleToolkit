using Radek.SimpleShell;
using SimpleShellSample.Views.Pages;

namespace SimpleShellSample;

public partial class SimpleAppShell : SimpleShell
{
	public SimpleAppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(FirstYellowDetailPage), typeof(FirstYellowDetailPage));
        Routing.RegisterRoute(nameof(SecondYellowDetailPage), typeof(SecondYellowDetailPage));
        Routing.RegisterRoute(nameof(ThirdYellowDetailPage), typeof(ThirdYellowDetailPage));
        Routing.RegisterRoute(nameof(FirstGreenDetailPage), typeof(FirstGreenDetailPage));
    }

	private async void Button_Clicked(object sender, EventArgs e)
	{
        var button = sender as Button;
        var shellItem = button.BindingContext as BaseShellItem;
        
        var items = Items;
        var flyoutItems = FlyoutItems;
        var menuItems = MenuBarItems;
        var toolbarItems = ToolbarItems;

		await this.GoToAsync($"//{shellItem.Route}");
    }

    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await this.GoToAsync("..");
    }
}
