using SimpleToolkit.Core;

namespace Playground.Original.Views.Pages;

public class WaterfallPage : ContentPage
{
    private readonly WaterfallGrid waterfall;


    public WaterfallPage()
    {
        Content = new ScrollView
        {
            Content = waterfall = Waterfall()
        };
        Title = "Waterfall";

        Loaded += OnPageLoaded;
        Loaded -= OnPageUnloaded;
    }


    private WaterfallGrid Waterfall()
    {
        var waterfall = new WaterfallGrid
        {
            Background = Colors.LightBlue,
            Span = 3,
            RowSpacing = 4,
            ColumnSpacing = 8,
            AlignContent = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start,
            Children = 
            {
                Cell(0, 60, LayoutOptions.Start),
                Cell(1, 100),
                Cell(2, 30),
                Cell(3, 80, LayoutOptions.Center),
                Cell(4, 40),
                Cell(5, 50, LayoutOptions.End),
                Cell(6, 60),
                Cell(7, 40),
                Cell(8, 30),
                Cell(9, 70),
            }
        };

        return waterfall;
    }

    private Grid Cell(int number, double height, LayoutOptions? horizontalOptions = null)
    {
        return new Grid
        {
            Background = Colors.Green,
            MinimumHeightRequest = height,
            HorizontalOptions = horizontalOptions ?? LayoutOptions.Fill,
            Children =
            {
                new Label { Text = number.ToString(), Padding = new Thickness(5), FontSize = 20, FontFamily = "OpenSansRegular" }
            }
        };
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