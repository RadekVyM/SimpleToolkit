# SimpleToolkit.SimpleShell

[![SimpleToolkit.SimpleShell](https://img.shields.io/nuget/v/SimpleToolkit.SimpleShell.svg?label=SimpleToolkit.SimpleShell)](https://www.nuget.org/packages/SimpleToolkit.SimpleShell/)

The _SimpleToolkit.SimpleShell_ package provides you with a simplified implementation of .NET MAUI `Shell` that lets you easily create a custom navigation experience in your .NET MAUI applications. Same as .NET MAUI `Shell`, `SimpleShell` provides you with:

- A single place to describe the logical hierarchy of an app.
- A URI-based navigation scheme that permits navigation to any page in the app.

**`SimpleShell` does not come with any navigation controls.** `SimpleShell` just gives you the ability to use custom navigation controls along with the URI-based navigation and automatic navigation stack management.

> [!IMPORTANT]
> Before you begin using `SimpleShell`, I highly recommend familiarizing yourself with the original .NET MAUI `Shell` - especially with the URI-based [navigation](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation), which works exactly the same as in `SimpleShell`.

## Getting Started

In order to use _SimpleToolkit.SimpleShell_, you need to call the `UseSimpleShell()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleShell();
```

This method also takes a boolean `usePlatformTransitions` parameter, which defaults to `true` and controls whether platform-specific animated transitions between pages are used.

`SimpleShell` uses platform-specific animated transitions by default. Although, these animated transitions can be modified, it is quite limited. If you want to take full control over the transitions, you need to disable the platform-specific ones by setting the `usePlatformTransitions` parameter to `false` and define your own [platform-independent animations](Transitions.md).

## SimpleShell

`SimpleShell` is a simplified implementation of .NET MAUI `Shell`. All `SimpleShell` is is just a set of containers for your application content with the ability to put the hosting area for pages wherever you want. This gives you the flexibility to add custom tab bars, navigation bars, flyouts, etc. to your `Shell` application while using the URI-based navigation.

Even though `SimpleShell` inherits from the `Shell` class, many of its properites are not mapped to any platform controls. However, you can bind these properties to your custom controls.

### XAML namespace

All `SimpleShell` related controls and attached properties can be found in the following XAML namespace:

```xml
xmlns:simpleShell="clr-namespace:SimpleToolkit.SimpleShell;assembly=SimpleToolkit.SimpleShell"
```

### Logical hierarchy

Let's say we want to have an app with four root pages - `YellowPage`, `GreenPage`, `RedPage` and `BluePage` - and one detail page - `YellowDetailPage`. In `SimpleShell`, the logical hierarchy of the app can be defined like this:

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<simpleShell:SimpleShell
    x:Class="SimpleToolkit.SimpleShellSample.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:simpleShell="clr-namespace:SimpleToolkit.SimpleShell;assembly=SimpleToolkit.SimpleShell"
    xmlns:pages="clr-namespace:SimpleToolkit.SimpleShellSample.Views.Pages"
    x:Name="thisShell"

    Background="White">

    <TabBar>
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

        <Tab
            Title="Red"
            Route="RedTab">
            <ShellContent
                Title="Red"
                ContentTemplate="{DataTemplate pages:RedPage}"
                Route="RedPage"/>
        </Tab>

        <Tab
            Title="Blue"
            Route="BlueTab">
            <ShellContent
                Title="Blue"
                ContentTemplate="{DataTemplate pages:BluePage}"
                Route="BluePage"/>
        </Tab>
    </TabBar>

</simpleShell:SimpleShell>
```

As you can see, the logical hierarchy is defined using `ShellContent`, `Tab`, `TabBar` and `FlyoutItem` elements as in the original .NET MAUI `Shell`.

> I refer to the `TabBar` and `FlyoutItem` elements as `ShellItem` and to the `Tab` element as `ShellSection`. The `ShellItem` and `ShellSection` classes are their predecessors.

Semantics of the `TabBar` and `FlyoutItem` elements is ignored in `SimpleShell`. It does not matter which one you use. You can even use the `ShellItem` element instead. You can also use multiple `TabBar` elements at once.

As in the original .NET MAUI `Shell`, `ShellItem` and `ShellSection` elements can be left out and implicitly generated:

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<simpleShell:SimpleShell
    x:Class="SimpleToolkit.SimpleShellSample.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:simpleShell="clr-namespace:SimpleToolkit.SimpleShell;assembly=SimpleToolkit.SimpleShell"
    xmlns:pages="clr-namespace:SimpleToolkit.SimpleShellSample.Views.Pages"
    x:Name="thisShell"

    Background="White">

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

</simpleShell:SimpleShell>
```

#### Detail pages

Detail pages are registered using the `RegisterRoute()` static method:

```csharp
public partial class AppShell : SimpleToolkit.SimpleShell.SimpleShell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(YellowDetailPage), typeof(YellowDetailPage));
    }
}
```

### `Content` and `SimpleNavigationHost`

At this moment, there are no navigation controls in the app. A simple app bar and tab bar can be defined like this:

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<simpleShell:SimpleShell
    x:Class="SimpleToolkit.SimpleShellSample.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:simpleShell="clr-namespace:SimpleToolkit.SimpleShell;assembly=SimpleToolkit.SimpleShell"
    xmlns:pages="clr-namespace:SimpleToolkit.SimpleShellSample.Views.Pages"
    x:Name="thisShell"

    Background="White">

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

    <simpleShell:SimpleShell.Content>
        <Grid
            RowDefinitions="50, *, 50">
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
            <HorizontalStackLayout
                x:Name="tabBar"
                Grid.Row="2"
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
    </simpleShell:SimpleShell.Content>

</simpleShell:SimpleShell>
```

The visual structure of a `SimpleShell` app can be manually defined using several containers. A container is a view which wraps a hosting area for pages. Such a container is, for example, `Content`, which can be set using the `Content` property and which wraps the entire content of the app.

The hosting area for pages is represented by the `SimpleNavigationHost` view that can occur somewhere in a container view hierarchy **just once**.

#### `SimpleShell` properties

`SimpleShell` provides you with some additional **bindable properties** that you can bind to when creating custom navigation controls:

- `CurrentPage` - the currently selected `Page`
- `CurrentShellSection` - the currently selected `ShellSection` (`Tab`)
- `CurrentShellContent` - the currently selected `ShellContent`
- `ShellSections` - read-only list of all `ShellSection`s in the shell
- `ShellContents` - read-only list of all `ShellContent`s in the shell
- `FlyoutItems` - read-only list of all `FlyoutItem`s in the shell
- `TabBars` - read-only list of all `TabBar`s in the shell
- `RootPageContainer` - a view that wraps all root pages (`ShellContent`s)

#### Navigation

Navigation between pages works almost the same as in .NET MAUI `Shell`, just use the common `Shell.Current.GoToAsync()`. `SimpleShell` differs only in these cases:

- The `animate` parameter value has no effect on whether the transition animation is played or not.
- When platform-specific transition animations are used, the `Task` returned by the `GoToAsync()` method will complete once the navigation has been initiated, not once the animation has been completed. In other words, the returned `Task` is not waiting for the animation to complete. The same applies to `Navigated` events.

> [!TIP]
> See [.NET MAUI documentation](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation) for more information.

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

This diagram shows a simplified visual structure of our shell:

<p align="center">
    <picture>
        <source srcset="../images/content_container_dark.png" media="(prefers-color-scheme: dark)">
        <img src="../images/content_container_light.png" width="850">
    </picture>
</p>

> Solid-lined rectangles represent containers.

Output:

<table>
    <tr>
        <th align="center">
            Android
        </th>
        <th align="center">
            iOS
        </th>
    </tr>
    <tr>
        <td align="center">
            <video src="https://github.com/RadekVyM/SimpleToolkit/assets/65116078/ae980939-b701-4a53-b789-7d26a4265ea2">
        </td>
        <td align="center">
            <video src="https://github.com/RadekVyM/SimpleToolkit/assets/65116078/5b9befb7-db1a-4500-a9ca-b86ab5b829e3">
        </td>
    </tr>
    <tr>
        <th align="center">
            macOS
        </th>
        <th align="center">
            Windows
        </th>
    </tr>
    <tr>
        <td align="center">
            <video src="https://github.com/RadekVyM/SimpleToolkit/assets/65116078/4d104c10-ec97-4851-ae82-d756c5cdde2b">
        </td>
        <td align="center">
            <video src="https://github.com/RadekVyM/SimpleToolkit/assets/65116078/eee4439a-5918-4fcc-aef3-4b8476c2392d">
        </td>
    </tr>
</table>

## `RootPageContainer`

We usually do not want tab bars, floating buttons and other navigation elements to be visible on all of our pages. Because of this, we can specify a `RootPageContainer` view that wraps all the root pages (`ShellContent`s).

Let's move the tab bar from the above sample to `RootPageContainer`:

```xml
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
```

The `RootPageContainer` view must contain a `SimpleNavigationHost` element somewhere in its view hieararchy. This element will host all the root pages.

This diagram shows a simplified visual structure of our shell:

<p align="center">
    <picture>
        <source srcset="../images/rootpage_container_dark.png" media="(prefers-color-scheme: dark)">
        <img src="../images/rootpage_container_light.png" width="850">
    </picture>
</p>

> Solid-lined rectangles represent containers.

Tab bar is not visible on the detail page:

<table>
    <tr>
        <th align="center">
            Android
        </th>
        <th align="center">
            iOS
        </th>
    </tr>
    <tr>
        <td align="center">
            <video src="https://github.com/RadekVyM/SimpleToolkit/assets/65116078/b37e1561-be40-4aef-a255-060cfbfc9572">
        </td>
        <td align="center">
            <video src="https://github.com/RadekVyM/SimpleToolkit/assets/65116078/84cf922e-f698-4f96-b06c-561df5c61e1c">
        </td>
    </tr>
    <tr>
        <th align="center">
            macOS
        </th>
        <th align="center">
            Windows
        </th>
    </tr>
    <tr>
        <td align="center">
            <video src="https://github.com/RadekVyM/SimpleToolkit/assets/65116078/f90f09bb-ce7c-4002-9e21-9bd7d7afb962">
        </td>
        <td align="center">
            <video src="https://github.com/RadekVyM/SimpleToolkit/assets/65116078/04ac5e38-af50-4066-95ec-f2fd0d30c672">
        </td>
    </tr>
</table>

## `ShellGroupContainer`

You can also specify a container view for each `ShellItem` or `ShellSection` via the `SimpleShell.ShellGroupContainerTemplate` attached property. The container view is defined using `DataTemplate` which allows the container to be created on demand in response to navigation. The created view is cached in the `SimpleShell.ShellGroupContainer` attached property.

A view defined in the `SimpleShell.ShellGroupContainerTemplate` property has to contain a `SimpleNavigationHost` element somewhere in its view hierarchy. This element will host the root pages.

Let's change the main tab bar from the above sample to display `ShellSection`s instead of all the root pages:

```xml
<simpleShell:SimpleShell.RootPageContainer>
    <Grid
        RowDefinitions="*, 50">
        <simpleShell:SimpleNavigationHost/>
        <HorizontalStackLayout
            x:Name="tabBar"
            Grid.Row="1"
            Margin="20,5"
            HorizontalOptions="Center" Spacing="10"
            BindableLayout.ItemsSource="{Binding ShellSections, Source={x:Reference thisShell}}">
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
```

The `ShellSection` with two root pages will contain a top tab bar:

```xml
<Tab
    Title="Yellow-Green"
    Route="YellowGreenTab">
    <simpleShell:SimpleShell.ShellGroupContainerTemplate>
        <DataTemplate
            x:DataType="ShellSection">
            <Grid
                RowDefinitions="50, *">
                <HorizontalStackLayout
                    Margin="20,5"
                    HorizontalOptions="Start" Spacing="10"
                    BindableLayout.ItemsSource="{Binding Items}">
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
                <simpleShell:SimpleNavigationHost
                    Grid.Row="1"/>
            </Grid>
        </DataTemplate>
    </simpleShell:SimpleShell.ShellGroupContainerTemplate>

    <ShellContent
        Title="Yellow"
        ContentTemplate="{DataTemplate pages:YellowPage}"
        Route="YellowPage"/>

    <ShellContent
        Title="Green"
        ContentTemplate="{DataTemplate pages:GreenPage}"
        Route="GreenPage"/>
</Tab>
```

Binding context of a view defined in the template is a respective `ShellSection` (`Tab`) instance.

This diagram shows a simplified visual structure of our shell:

<p align="center">
    <picture>
        <source srcset="../images/shellsection_container_dark.png" media="(prefers-color-scheme: dark)">
        <img src="../images/shellsection_container_light.png" width="850">
    </picture>
</p>

> Solid-lined rectangles represent containers.

The yellow and green pages are grouped under one tab:

<table>
    <tr>
        <th align="center">
            Android
        </th>
        <th align="center">
            iOS
        </th>
    </tr>
    <tr>
        <td align="center">
            <video src="https://github.com/RadekVyM/SimpleToolkit/assets/65116078/6d49a71b-dc7d-4a8a-9030-ba3a48a59568">
        </td>
        <td align="center">
            <video src="https://github.com/RadekVyM/SimpleToolkit/assets/65116078/2e7e471a-1f23-4e9e-9e29-2a88bb231f40">
        </td>
    </tr>
    <tr>
        <th align="center">
            macOS
        </th>
        <th align="center">
            Windows
        </th>
    </tr>
    <tr>
        <td align="center">
            <video src="https://github.com/RadekVyM/SimpleToolkit/assets/65116078/40478570-d732-4d72-a153-23f8457b07e7">
        </td>
        <td align="center">
            <video src="https://github.com/RadekVyM/SimpleToolkit/assets/65116078/fd321f04-6c51-4110-993b-ae22ae2ca26b">
        </td>
    </tr>
</table>

## Transitions

`SimpleShell` allows you to define custom transitions between pages during navigation:

https://github.com/RadekVyM/SimpleToolkit/assets/65116078/694efb22-2a1f-4ec2-b169-307499357ae4

See [documentation](Transitions.md) for more information.

## Visual states

`SimpleShell` provides multiple groups of visual states which help to define the shell appearance based on the current state of navigation. See [documentation](VisualStates.md) for more information.

## Implementation details

The `SimpleShell` class inherits from the .NET MAUI `Shell` class, but all the [handlers](https://learn.microsoft.com/dotnet/maui/user-interface/handlers) are implemented from the ground up. These handlers are inspired by the WinUI version of `Shell` handlers.

## Why not use `SimpleShell` and use .NET MAUI `Shell` instead

- .NET MAUI `Shell` offers a platform-specific appearance.
- Platform-specific navigation controls that .NET MAUI `Shell` provides probably have better performance than controls composed of multiple .NET MAUI views.
- A `SimpleShell`-based application may not have as good accessibility in some scenarios due to the lack of platform-specific navigation controls. .NET MAUI `Shell` should be accessible out of the box since it uses platform-specific controls.
- Maybe I have implemented something wrong that has a negative impact on the performance, accessibility, or something like that.
