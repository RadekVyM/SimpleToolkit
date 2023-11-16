# SimpleToolkit.SimpleShell

The _SimpleToolkit.SimpleShell_ package provides you with a simplified implementation of .NET MAUI `Shell` that lets you easily create a custom navigation experience in your .NET MAUI applications. Same as .NET MAUI `Shell`, `SimpleShell` provides you with:

- A single place to describe the logical hierarchy of an app.
- A URI-based navigation scheme that permits navigation to any page in the app.

**`SimpleShell` does not come with any navigation controls.** `SimpleShell` just gives you the ability to use custom navigation controls along with the URI-based navigation and automatic navigation stack management.

## Getting Started

In order to use _SimpleToolkit.SimpleShell_, you need to call the `UseSimpleShell()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleShell();
```

This method also takes a boolean parameter `usePlatformTransitions`, which defaults to `true` and controls wheter platform-specific animated transitions between pages are used.

### XAML namespace

All `SimpleShell` related controls and attached properties can be found in the following XAML namespace:

```xml
xmlns:simpleShell="clr-namespace:SimpleToolkit.SimpleShell;assembly=SimpleToolkit.SimpleShell"
```

## SimpleShell

`SimpleShell` is a simplified implementation of .NET MAUI `Shell`. All `SimpleShell` is is just a set of containers for your application content with the ability to put the hosting area for pages wherever you want. This gives you the flexibility to add custom tab bars, navigation bars, flyouts, etc. to your `Shell` application while using the URI-based navigation.

Let's say we have four root pages - `YellowPage`, `GreenPage`, `RedPage` and `BluePage` - and one detail page - `YellowDetailPage`. Shell with a simple app bar and tab bar can be defined like this:

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
            await GoToAsync($"///{shellItem.Route}", true);
    }

    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await GoToAsync("..");
    }
}
```

Navigation between pages works almost the same as in .NET MAUI `Shell`, just use the common `Shell.Current.GoToAsync()`. `SimpleShell` differs only in these cases:

- The `animate` parameter value has no effect on whether the transition animation is played or not.
- When platform-specific transition animations are used, the `Task` returned by the `GoToAsync()` method will complete once the navigation has been initiated, not once the animation has been completed. In other words, the returned `Task` is not waiting for the animation to complete. The same applies to `Navigated` events.

Pages that are not part of the shell hierarchy can be registered using the `Routing.RegisterRoute()` method.

## Why not use `SimpleShell` and use .NET MAUI `Shell` instead

- .NET MAUI `Shell` offers a platform-specific appearance.
- Platform-specific navigation controls that .NET MAUI `Shell` provides probably have better performance than controls composed of multiple .NET MAUI views.
- A `SimpleShell`-based application may not have as good accessibility in some scenarios due to the lack of platform-specific navigation controls. .NET MAUI `Shell` should be accessible out of the box since it uses platform-specific controls.
- Maybe I have implemented something wrong that has a negative impact on the performance, accessibility, or something like that.

> See [documentation](https://github.com/RadekVyM/SimpleToolkit/tree/main/docs/SimpleToolkit.SimpleShell) for more information.
