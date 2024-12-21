namespace Playground.Original.Views.Pages;

public class BluePage : BasePage
{
	public BluePage() : base("Blue Page", Colors.CadetBlue) { }
}

public class GrayPage : BasePage
{
    public GrayPage() : base(
        "Gray Page",
        Colors.Gray,
        [
            new PageButton("🐚 Recreate SimpleShell", static () => (App.Current as App)?.RefreshShell(AppShellType.Playground)),
            new PageButton("🐚 Recreate Shell", static () => (App.Current as App)?.RefreshShell(AppShellType.Normal)),
            new PageButton("🐚 Recreate ContainersShell", static () => (App.Current as App)?.RefreshShell(AppShellType.Containers)),
            new PageButton("🐚 Recreate NoTabsShell", static () => (App.Current as App)?.RefreshShell(AppShellType.NoTabs)),
            new PageButton("🐚 Recreate ShellItemsShell", static () => (App.Current as App)?.RefreshShell(AppShellType.ShellItems)),
        ]) { }
}

public class OrangePage : BasePage
{
    public OrangePage() : base("Orange Page", Colors.DarkOrange) { }
}

public class RedPage : BasePage
{
    public RedPage() : base("Red Page", Colors.DarkRed) { }
}

public class YellowPage : BasePage
{
    public YellowPage() : base(
        "Yellow Page",
        Colors.Goldenrod,
        [
            new PageButton("Go to first detail page", static async () => await Shell.Current.GoToAsync(nameof(FirstYellowDetailPage), true)),
            new PageButton("Go to second detail page", static async () => await Shell.Current.GoToAsync($"/{nameof(FirstYellowDetailPage)}/{nameof(SecondYellowDetailPage)}", true)),
        ])
    {
        ToolbarItems.Add(new ToolbarItem("Revert", "case_empty.png", Activated, ToolbarItemOrder.Secondary));
        ToolbarItems.Add(new ToolbarItem("Settings", "flame_empty.png", Activated, ToolbarItemOrder.Secondary));
        ToolbarItems.Add(new ToolbarItem("Send Feedback", null, Activated, ToolbarItemOrder.Secondary));
        ToolbarItems.Add(new ToolbarItem("Help", "avatar_empty.png", Activated, ToolbarItemOrder.Secondary));
    }

    static void Activated() { }

    protected override bool OnBackButtonPressed()
    {
        System.Diagnostics.Debug.WriteLine($"{nameof(YellowPage)}: Back button pressed");
        return base.OnBackButtonPressed();
    }
}

public class FirstYellowDetailPage : BasePage
{
    public FirstYellowDetailPage() : base(
        "First Yellow Detail Page",
        Colors.Goldenrod,
        [
            new PageButton("Go to second detail page", static async () => await Shell.Current.GoToAsync(nameof(SecondYellowDetailPage), true)),
        ])
    { }

    protected override bool OnBackButtonPressed()
    {
        System.Diagnostics.Debug.WriteLine($"{nameof(FirstYellowDetailPage)}: Back button pressed");
        return base.OnBackButtonPressed();
    }
}

public class SecondYellowDetailPage : BasePage
{
    public SecondYellowDetailPage() : base(
        "Second Yellow Detail Page",
        Colors.Goldenrod,
        [
            new PageButton("Go to root page", static async () => await Shell.Current.Navigation.PopToRootAsync()),
            new PageButton("Go to third detail page", static async () => await Shell.Current.GoToAsync(nameof(ThirdYellowDetailPage), true)),
            new PageButton("Go to fourth detail page", static async () => await Shell.Current.GoToAsync(nameof(FourthYellowDetailPage), false)),
        ]) { }
}

public class FourthYellowDetailPage : BasePage
{
    public FourthYellowDetailPage() : base(
        "Fourth Yellow Detail Page",
        Colors.Goldenrod,
        [
            new PageButton("Twice back", static async () => await Shell.Current.GoToAsync($"../..", true)),
            new PageButton("Go to green page", static async () => await Shell.Current.GoToAsync($"//{nameof(GreenPage)}", true)),
            new PageButton("Go to green detail page", static async () => await Shell.Current.GoToAsync($"//{nameof(GreenPage)}/{nameof(FirstGreenDetailPage)}", true)),
        ])
    { }
}

public class ThirdYellowDetailPage : BasePage
{
    public ThirdYellowDetailPage() : base(
        "Third Yellow Detail Page",
        Colors.Goldenrod,
        [
            new PageButton("Twice back", static async () => await Shell.Current.GoToAsync($"../..", true)),
            new PageButton("Go to green page", static async () => await Shell.Current.GoToAsync($"//{nameof(GreenPage)}", true)),
        ])
    {
        Shell.SetPresentationMode(this, PresentationMode.ModalAnimated);
    }
}

public class GreenPage : BasePage
{
    public GreenPage() : base(
        "Green Page",
        Colors.ForestGreen,
        [
            new PageButton("Go to first detail page", static async () => await Shell.Current.GoToAsync(nameof(FirstGreenDetailPage))),
            new PageButton("Go to second yellow detail page", static async () => await Shell.Current.GoToAsync($"//{nameof(YellowPage)}/{nameof(FirstYellowDetailPage)}/{nameof(SecondYellowDetailPage)}")),
        ]) { }
}

public class FirstGreenDetailPage : BasePage
{
    public FirstGreenDetailPage() : base(
        "First Green Detail Page",
        Colors.ForestGreen,
        [
            new PageButton("Go to red page", static async () => await Shell.Current.GoToAsync($"//{nameof(RedPage)}")),
        ])
    { }
}
