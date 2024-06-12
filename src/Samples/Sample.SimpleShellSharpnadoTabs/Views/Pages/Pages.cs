using Sample.SimpleShellSharpnadoTabs.Views.Controls;

namespace Sample.SimpleShellSharpnadoTabs.Views.Pages;

public class BasePage : ContentPage
{
    public BasePage(string title, IEnumerable<IView>? content = null)
    {
        Title = title;

        var stackLayout = new VerticalStackLayout
        {
            VerticalOptions = LayoutOptions.Center,
            Spacing = 20
        };

        stackLayout.Add(new Label
        {
            Text = title,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        });

        if (content is not null)
        {
            foreach (var child in content)
                stackLayout.Add(child);
        }

        Content = stackLayout;
    }
}

public class HomePage : BasePage
{
    public HomePage() : base("Home Page", [DetailButton()]) { }

    private static Button DetailButton()
    {
        var button = new Button
        {
            Text = "Detail Page",
            HorizontalOptions= LayoutOptions.Center,
            BorderWidth = 1,
            BorderColor = Colors.Gray
        };

        button.Clicked += static (sender, e) => Shell.Current.GoToAsync(nameof(FirstDetailPage));

        return button;
    }
}

public class CameraPage : BasePage
{
    public CameraPage() : base("Camera Page") { }
}

public class ImagesPage : BasePage
{
    public ImagesPage() : base("Images Page") { }
}

public class AccountPage : BasePage
{
    public AccountPage() : base("Account Page") { }
}

public class BaseDetailPage : ContentPage
{
    public BaseDetailPage(string title)
    {
        Title = title;
        Content = new TopBarScaffold
        {
            Title = title,
            IsBackButtonVisible = true,
            PageContent = new Label
            {
                Text = title,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }
        };
    }
}

public class FirstDetailPage : BaseDetailPage
{
    public FirstDetailPage() : base("Detail Page") { }
}