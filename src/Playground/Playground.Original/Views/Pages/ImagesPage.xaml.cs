using SimpleToolkit.Core;

namespace Playground.Original.Views.Pages;

public partial class ImagesPage : ContentPage
{
    public IList<ImageModel> Images { get; init; }

    public ImagesPage()
    {
        Images = [
            new("https://picsum.photos/id/10/250/250"),
            new("https://picsum.photos/id/11/250/120"),
            new("https://picsum.photos/id/15/250/360"),
            new("https://picsum.photos/id/13/250/200"),
            new("https://picsum.photos/id/17/250/300"),
            new("https://picsum.photos/id/28/250/250"),
            new("https://picsum.photos/id/29/250/150"),
            new("https://picsum.photos/id/66/250/250"),
            new("https://picsum.photos/id/70/250/300"),
            new("https://picsum.photos/id/93/250/180"),
            new("https://picsum.photos/id/110/250/280"),
            new("https://picsum.photos/id/116/250/260"),
            new("https://picsum.photos/id/128/250/220"),
            new("https://picsum.photos/id/177/250/300"),
        ];

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

public record ImageModel(string Url);