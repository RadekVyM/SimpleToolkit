namespace SimpleToolkit.Template.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        MainPage = this.Handler.MauiContext.Services.GetRequiredService<AppShell>();
    }
}
