using Radek.SimpleShell;
using SimpleShellSample.Views.Pages;
using System.Windows.Input;

namespace SimpleShellSample;

public partial class SimpleAppShell : SimpleShell
{
    public ICommand BackCommand { get; set; }

	public SimpleAppShell()
	{
        BackCommand = new Command(async () => {
            await this.GoToAsync("..");
        });
		
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

        if (!CurrentState.Location.OriginalString.Contains(shellItem.Route))
            await this.GoToAsync($"///{shellItem.Route}");
    }

    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await this.GoToAsync("..");
    }

    private bool orangeAdded = false;

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        if (!orangeAdded)
        {
            this.Items.Add(new ShellContent()
            {
                Title = "Orange",
                Route = nameof(OrangePage),
                ContentTemplate = new DataTemplate(typeof(OrangePage))
            });
        }

        orangeAdded = true;
    }
}
