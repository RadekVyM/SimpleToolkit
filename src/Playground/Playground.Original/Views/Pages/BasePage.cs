namespace Playground.Original.Views.Pages;

public class BasePage : ContentPage
{
	public BasePage(
		string title,
		Color color,
        IEnumerable<PageButton> buttons = null,
        IEnumerable<IView> views = null)
	{
		Title = title;
		Background = color;

		VerticalStackLayout stackLayout;

		Content = new ScrollView
		{
			Content = stackLayout = new VerticalStackLayout
			{
				VerticalOptions = LayoutOptions.Fill,
				Children =
				{
					new Label
					{
						Text = title,
						TextColor = Colors.White,
						FontSize = 20,
						FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(20, 50),
						TextTransform = TextTransform.Uppercase,
						HorizontalTextAlignment = TextAlignment.Center
					},
                },
            }
		};

        if (buttons is not null)
        {
			foreach (var pageButton in buttons)
			{
				var button = new Button
				{
					Text = pageButton.Title,
					TextColor = Colors.White,
                    Margin = new Thickness(10),
                    HorizontalOptions = LayoutOptions.Center,
                    Background = Colors.Transparent
				};

				button.Clicked += (s, e) => pageButton.Action?.Invoke();

                stackLayout.Add(button);
            }
        }

        if (views is not null)
		{
			foreach (var view in views)
				stackLayout.Add(view);
        }

        Loaded += PageLoaded;
        Unloaded += PageUnloaded;
    }

    private void PageLoaded(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"{Title} Loaded");
    }

    private void PageUnloaded(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"{Title} Unloaded");
    }

    protected override void OnAppearing()
    {
        System.Diagnostics.Debug.WriteLine($"{Title} OnAppearing");
        base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        System.Diagnostics.Debug.WriteLine($"{Title} OnDisappearing");
        base.OnDisappearing();
    }

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        System.Diagnostics.Debug.WriteLine($"{Title} OnNavigatingFrom");
        base.OnNavigatingFrom(args);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        System.Diagnostics.Debug.WriteLine($"{Title} OnNavigatedFrom");
        base.OnNavigatedFrom(args);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        System.Diagnostics.Debug.WriteLine($"{Title} OnNavigatedTo");
        base.OnNavigatedTo(args);
    }
}

public record PageButton(string Title, Action Action);