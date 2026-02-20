# SimpleToolkit.SimpleShell

`SimpleShell` is a lightweight, decoupled implementation of .NET MAUI `Shell`. It provides the powerful URI-based navigation and stack management of `Shell` but **without any forced UI**.

You gain 100% control over your navigation UI (TabBars, Flyouts, TitleBars) while keeping the standard `Shell.Current.GoToAsync()` workflow.

## 🚀 Getting Started

1. **Initialize**: Register the handler in your `MauiProgram.cs`:
```csharp
// usePlatformTransitions: true (default) uses native page animations
builder.UseSimpleShell(usePlatformTransitions: true);
```

2. **Namespace**: Add this to your XAML:
```xml
xmlns:simpleShell="clr-namespace:SimpleToolkit.SimpleShell;assembly=SimpleToolkit.SimpleShell"
```

## 🏗️ Core Concepts

`SimpleShell` is a simplified implementation of .NET MAUI `Shell`. All `SimpleShell` is is just a set of containers for your application content with the ability to put the hosting area for pages wherever you want. This gives you the flexibility to add custom tab bars, navigation bars, flyouts, etc. to your `Shell` application while using the URI-based navigation.

## 📖 Usage Example

Let's say we have four root pages — `YellowPage`, `GreenPage`, `RedPage` and `BluePage` — and one detail page — `YellowDetailPage`. Shell with a simple app bar and tab bar can be defined like this:

```xml
<simpleShell:SimpleShell
    x:Class="SimpleToolkit.SimpleShellSample.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:simpleShell="clr-namespace:SimpleToolkit.SimpleShell;assembly=SimpleToolkit.SimpleShell"
    xmlns:pages="clr-namespace:SimpleToolkit.SimpleShellSample.Views.Pages"
    x:Name="thisShell"

    Background="White">

    <!-- Pages can be grouped into tabs (ShellSections) -->
    <Tab
        Title="Yellow-Green"
        Route="YellowGreenTab">
        <ShellContent
            Title="Yellow"
            ContentTemplate="{DataTemplate pages:YellowPage}"
            Route="YellowPage"/>

        <ShellContent
            Title="Green"
            ContentTemplate="{DataTemplate pages:GreenPage}"
            Route="GreenPage"/>
    </Tab>

    <ShellContent
        Title="Red"
        ContentTemplate="{DataTemplate pages:RedPage}"
        Route="RedPage"/>

    <ShellContent
        Title="Blue"
        ContentTemplate="{DataTemplate pages:BluePage}"
        Route="BluePage"/>

    <simpleShell:SimpleShell.RootPageContainer>
        <Grid
            RowDefinitions="*, 50">
            <simpleShell:SimpleNavigationHost/>
            <HorizontalStackLayout
                x:Name="tabBar"
                Grid.Row="1"
                Margin="20,5"
                HorizontalOptions="Center" Spacing="10"
                BindableLayout.ItemsSource="{Binding ShellContents, Source={x:Reference thisShell}}">
                <BindableLayout.ItemTemplate>
                    <DataTemplate
                        x:DataType="BaseShellItem">
                        <Button
                            Clicked="ShellItemButtonClicked"
                            Background="Black"
                            Text="{Binding Title}"/>
                    </DataTemplate>
                </BindableLayout.ItemTemplate>
            </HorizontalStackLayout>
        </Grid>
    </simpleShell:SimpleShell.RootPageContainer>

    <simpleShell:SimpleShell.Content>
        <Grid
            RowDefinitions="50, *">
            <Button
                x:Name="backButton"
                Clicked="BackButtonClicked"
                Text="Back"
                Margin="20,5"
                HorizontalOptions="Start"
                Background="Black"/>
            <Label
                Margin="20,5"
                HorizontalOptions="Center" VerticalOptions="Center"
                Text="{Binding CurrentShellContent.Title, Source={x:Reference thisShell}}"
                FontAttributes="Bold" FontSize="18"/>
            <simpleShell:SimpleNavigationHost
                Grid.Row="1"/>
        </Grid>
    </simpleShell:SimpleShell.Content>
</simpleShell:SimpleShell>
```

As you can see, the logical hierarchy is defined using `ShellContent`, `Tab`, `TabBar` and `FlyoutItem` elements as in the original .NET MAUI `Shell`. However, visual structure is defined manually using the `Content` or `RootPageContainer` property. The hosting area for pages is represented by the `SimpleNavigationHost` view.

The code behind of the XAML sample above:

```csharp
public partial class AppShell : SimpleToolkit.SimpleShell.SimpleShell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(YellowDetailPage), typeof(YellowDetailPage));
    }

    private async void ShellItemButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var shellItem = button.BindingContext as BaseShellItem;

        // Navigate to a new tab if it is not the current tab
        if (!CurrentState.Location.OriginalString.Contains(shellItem.Route))
            await GoToAsync($"///{shellItem.Route}");
    }

    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await GoToAsync("..");
    }
}
```

Navigation between pages works almost the same as in .NET MAUI `Shell`, just use the common `Shell.Current.GoToAsync()`. Pages that are not part of the shell hierarchy can be registered using the `Routing.RegisterRoute()` method.


## ⚖️ When to use SimpleShell?

**Choose SimpleShell if:**

* You need a completely custom design that native Shell doesn't allow.
* You want a unified UI look across iOS, Android, and Windows.
* You want to host the navigation area anywhere in your layout.

**Stick to .NET MAUI Shell if:**

* You want a 100% "Native" look and feel.
* Out-of-the-box accessibility is your top priority.
* You don't want to build your own TabBar/Flyout logic.

> 📖 **Full Documentation:** [Detailed Guide & API Reference](https://github.com/RadekVyM/SimpleToolkit/tree/main/docs/SimpleToolkit.SimpleShell)