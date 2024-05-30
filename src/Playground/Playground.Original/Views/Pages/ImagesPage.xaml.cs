using SimpleToolkit.Core;

namespace Playground.Original.Views.Pages;

public partial class ImagesPage : ContentPage
{
    public ImagesPage()
    {
        InitializeComponent();

        Loaded += OnPageLoaded;
        Loaded -= OnPageUnloaded;
    }

    private void OnSafeAreaChanges(Thickness safeArea)
    {
        waterfall.Padding = new Thickness(
            safeArea.Left + 20,
            10,
            safeArea.Right + 20,
            10);
    }

    private void OnPageLoaded(object sender, EventArgs e)
    {
        this.Window.SubscribeToSafeAreaChanges(OnSafeAreaChanges);
    }

    private void OnPageUnloaded(object sender, EventArgs e)
    {
        this.Window.UnsubscribeFromSafeAreaChanges(OnSafeAreaChanges);
    }
}