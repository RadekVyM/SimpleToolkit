using Radek.SimpleShell;
using Radek.SimpleShell.Controls;
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
        
        if (!CurrentState.Location.OriginalString.Contains(shellItem.Route))
            await this.GoToAsync($"///{shellItem.Route}");
    }

    private async void TabView_ItemSelected(object sender, Radek.SimpleShell.Controls.TabViewItemSelectedEventArgs e)
    {
        if (!CurrentState.Location.OriginalString.Contains(e.ShellItem.Route))
            await this.GoToAsync($"///{e.ShellItem.Route}");
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

    private void DesignButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        button.ShowAttachedPopover();
    }

    private void MaterialButtonClicked(object sender, EventArgs e)
    {
        VisualStateManager.GoToState(this, "Material3");
        designButton.HideAttachedPopover();
    }

    private void CupertinoButtonClicked(object sender, EventArgs e)
    {
        VisualStateManager.GoToState(this, "Cupertino");
        designButton.HideAttachedPopover();
    }

    private void FluentButtonClicked(object sender, EventArgs e)
    {
        VisualStateManager.GoToState(this, "Fluent");
        designButton.HideAttachedPopover();
    }
}
