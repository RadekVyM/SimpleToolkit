namespace SimpleShellSample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

		//(Content as Grid).Children.Add(new SimpleShellSample.SimpleShell.SimpleNavigationHost());
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		var parent = this.Parent;
        var platform = Handler.PlatformView;
	}
}