using System.Runtime.CompilerServices;
using Radek.SimpleShell;

namespace SimpleShellSample;

public partial class AppShell : SimpleShell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("MainPage", typeof(MainPage));
		Routing.RegisterRoute("SchoolPage", typeof(SchoolPage));
    }

	protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		base.OnPropertyChanged(propertyName);

		if (propertyName == CurrentItemProperty.PropertyName)
		{
			var v = 5;
		}
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
        var button = sender as Button;
        var shellItem = button.BindingContext as BaseShellItem;

		this.GoToAsync($"///{shellItem.Route}");
	}

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);
        var platform = Handler.PlatformView;
    }

	protected override void OnNavigating(ShellNavigatingEventArgs args)
	{
		base.OnNavigating(args);
	}
}
