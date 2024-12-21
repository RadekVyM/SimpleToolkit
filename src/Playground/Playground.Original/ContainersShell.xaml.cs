using Playground.Original.Extensions;
using Playground.Original.Views.Pages;
using SimpleToolkit.Core;
using SimpleToolkit.SimpleShell;

#if ANDROID
using PlatformContainer = Android.Views.ViewGroup;
#elif IOS || MACCATALYST
using PlatformContainer = UIKit.UIView;
#elif WINDOWS
using PlatformContainer = Microsoft.UI.Xaml.Controls.Panel;
#endif

namespace Playground.Original;

public partial class ContainersShell : SimpleShell
{
    private int itemContainerIndex = 0;
    private int firstTabContainerIndex = 0;
    private int secondTabContainerIndex = 0;


	public ContainersShell()
	{
		Routing.RegisterRoute(nameof(FirstYellowDetailPage), typeof(FirstYellowDetailPage));
        Routing.RegisterRoute(nameof(SecondYellowDetailPage), typeof(SecondYellowDetailPage));
        Routing.RegisterRoute(nameof(ThirdYellowDetailPage), typeof(ThirdYellowDetailPage));
        Routing.RegisterRoute(nameof(FourthYellowDetailPage), typeof(FourthYellowDetailPage));
        Routing.RegisterRoute(nameof(FirstGreenDetailPage), typeof(FirstGreenDetailPage));

        InitializeComponent();

        Loaded += AppShellLoaded;
        Unloaded += AppShellUnloaded;

        Navigated += AppShellNavigated;
	}


    private void AppShellLoaded(object? sender, EventArgs e)
    {
        Window.SubscribeToSafeAreaChanges(OnSafeAreaChanged);
    }

    private void AppShellUnloaded(object? sender, EventArgs e)
    {
        Window.UnsubscribeFromSafeAreaChanges(OnSafeAreaChanged);
    }

    private void OnSafeAreaChanged(Thickness safeAreaPadding)
    {
        contentContainer.Padding = safeAreaPadding;
    }

    private async void AppShellNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        await UpdateStatsDelayed();
    }

    private DataTemplate CreateContainer(string text)
    {
        return new DataTemplate(() =>
            new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Star),
                },
                Children =
                {
                    new Label { Text = text, Margin = 5 },
                    CreateHost()
                }
            });

        SimpleNavigationHost CreateHost()
        {
            var host = new SimpleNavigationHost();

            Grid.SetRow(host, 1);

            return host;
        }
    }

    private async Task UpdateStatsDelayed()
    {
        await Task.Delay(300);
        UpdateStats();
    }

    private void UpdateStats()
    {
        contentContainerLabel.Text = $"Content: {GetContainerStats(contentContainer)}";
        rootContainerLabel.Text = $"Root: {GetContainerStats(rootContainer)}";
        itemContainerLabel.Text = GetShellItemStats(shellItem);
        firstTabContainerLabel.Text = GetShellItemStats(firstTab);
        secondTabContainerLabel.Text = GetShellItemStats(secondTab);
    }

    private string GetShellItemStats(BaseShellItem shellItem)
    {
        var container = SimpleShell.GetShellGroupContainer(shellItem);
        return GetContainerStats(container);
    }

    private string GetContainerStats(IView? container)
    {
        if (container?.Handler?.PlatformView is not PlatformContainer platformContainer)
            return "none";

        var navHost = container.FindSimpleNavigationHost();

        if (navHost?.Handler?.PlatformView is not PlatformContainer platformNavHost)
            return "none";

        bool hasParent = false;
        int navHostChildrenCount = 0;

#if ANDROID
        hasParent = platformContainer.Parent is not null;
        navHostChildrenCount = platformNavHost.ChildCount;
#elif IOS || MACCATALYST
        hasParent = platformContainer.Superview is not null;
        navHostChildrenCount = platformNavHost.Subviews.Length;
#elif WINDOWS
        hasParent = platformContainer.Parent is not null;
        navHostChildrenCount = platformNavHost.Children.Count;
#endif

        return $"Parent: {hasParent}; Children: {navHostChildrenCount}";
    }

    private async void ShellItemButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button?.BindingContext is not BaseShellItem shellItem)
            return;

        // Navigate to a new tab if it is not the current tab
        if (!CurrentState.Location.OriginalString.Contains(shellItem.Route))
            await GoToAsync($"///{shellItem.Route}", true);
    }

    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await GoToAsync("..");
    }

    private async void RecreateItemContainerClicked(object sender, EventArgs e)
    {
        SimpleShell.SetShellGroupContainerTemplate(shellItem, CreateContainer($"Item Container: {++itemContainerIndex}"));
        await UpdateStatsDelayed();
    }

    private async void ClearItemContainerClicked(object sender, EventArgs e)
    {
        SimpleShell.SetShellGroupContainerTemplate(shellItem, null);
        await UpdateStatsDelayed();
    }

    private async void RecreateFirstTabContainerClicked(object sender, EventArgs e)
    {
        SimpleShell.SetShellGroupContainerTemplate(firstTab, CreateContainer($"First Tab Container: {++firstTabContainerIndex}"));
        await UpdateStatsDelayed();
    }

    private async void ClearFirstTabContainerClicked(object sender, EventArgs e)
    {
        SimpleShell.SetShellGroupContainerTemplate(firstTab, null);
        await UpdateStatsDelayed();
    }

    private async void RecreateSecondTabContainerClicked(object sender, EventArgs e)
    {
        SimpleShell.SetShellGroupContainerTemplate(secondTab, CreateContainer($"Second Tab Container: {++secondTabContainerIndex}"));
        await UpdateStatsDelayed();
    }

    private async void ClearSecondTabContainerClicked(object sender, EventArgs e)
    {
        SimpleShell.SetShellGroupContainerTemplate(secondTab, null);
        await UpdateStatsDelayed();
    }

    private void RefreshClicked(object sender, EventArgs e)
    {
        UpdateStats();
    }
}