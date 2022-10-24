# SimpleToolkit.SimpleShell

[![SimpleToolkit.SimpleShell](https://img.shields.io/nuget/v/SimpleToolkit.SimpleShell.svg?label=SimpleToolkit.SimpleShell)](https://www.nuget.org/packages/SimpleToolkit.SimpleShell/)

The *SimpleToolkit.SimpleShell* package provides you with a simplified implementation of .NET MAUI `Shell` that lets you easily create a custom navigation experience in your .NET MAUI applications.

## Getting Started

In order to use *SimpleToolkit.SimpleShell*, you need to call the `UseSimpleShell()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleShell();
```

## SimpleShell

`SimpleShell` is a simplified implementation of .NET MAUI `Shell`. All `SimpleShell` is is just a simple container for your content with the ability to put the hosting area for pages wherever you want. Thanks to that, you are able to add custom tab bars, navigation bars, flyouts, etc. to your `Shell` application while using great `Shell` URI-based navigation.

```xml
<simpleShell:SimpleShell
    x:Class="SimpleSample.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:simpleShell="clr-namespace:SimpleToolkit.SimpleShell;assembly=SimpleToolkit.SimpleShell"
    xmlns:pages="clr-namespace:SimpleSample.Views.Pages"
    x:Name="thisShell">

    <ShellContent
        Title="Icons"
        Icon="icon.png"
        ContentTemplate="{DataTemplate pages:IconPage}"
        Route="IconPage"/>

    <ShellContent
        Title="Buttons"
        Icon="button.png"
        ContentTemplate="{DataTemplate pages:ContentButtonPage}"
        Route="ContentButtonPage"/>

    <ShellContent
        Title="Popovers"
        Icon="popover.png"
        ContentTemplate="{DataTemplate pages:PopoverPage}"
        Route="PopoverPage"/>

    <simpleShell:SimpleShell.Content>
        <Grid
            RowDefinitions="50, *, 50">
            <Button
                x:Name="backButton"
                Clicked="BackButtonClicked"
                Text="Back"
                Margin="20,5"
                HorizontalOptions="Start"
                Background="DarkOrange"/>
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
                            Clicked="TabButtonClicked"
                            Background="DarkOrange"
                            Text="{Binding Title}"/>
                    </DataTemplate>
                </BindableLayout.ItemTemplate>
            </HorizontalStackLayout>
        </Grid>
    </simpleShell:SimpleShell.Content>
</simpleShell:SimpleShell>
```

As you can see, the logical navigation structure is defined with `ShellContent`, `Tab`, etc. as in normal .NET MAUI `Shell`. However, visual structure has to be defined manually using the `Content` property. The hosting area for pages is represented by the `SimpleNavigationHost` view that can occur in the visual hierarchy **just once**.

SimpleShell provides you with some bindable properties which simplify the creation of custom navigation controls:

- `CurrentPage` - the currently selected `Page`
- `CurrentShellSection` - the currently selected `ShellSection` (`Tab`)
- `CurrentShellContent` - the currently selected `ShellContent`
- `ShellSections` - read-only list of all `ShellSection`s in the shell
- `ShellContents` - read-only list of all `ShellContent`s in the shell
- `RootPageOverlay` - you can use this property to set a view that will be displayed over all root pages (`ShellContent`s). This is well suited for tab bars, floating buttons, or flyouts that should be visible only on a root page

The code behind of the XAML sample above:

```csharp
private async void TabButtonClicked(object sender, EventArgs e)
{
    var button = sender as Button;
    var shellItem = button.BindingContext as BaseShellItem;

    // Navigate to a new tab if it is not the current tab
    if (!CurrentState.Location.OriginalString.Contains(shellItem.Route))
        await this.GoToAsync($"///{shellItem.Route}");
}
```

Navigation between pages works exactly the same as in .NET MAUI `Shell`, just use the common `Shell.Current.GoToAsync()`. Pages that are not part of the shell hierarchy can be registered using the `Routing.RegisterRoute()` method.

Output:

<p align="center">
    <table>
        <tr>
            <th>
                <p align="center">Android</p>
            </th>
            <th>
                <p align="center">iOS</p>
            </th>
            <th>
                <p align="center">Windows</p>
            </th>
        </tr>
        <tr>
            <td>
                <img src="../images/android_simpleshell.gif" data-canonical-src="../images/android_popover.gif" width="148"/>
            </td>
            <td>
                <img src="../images/ios_simpleshell.gif" data-canonical-src="../images/ios_simpleshell.gif" width="180"/>
            </td>
            <td>
                <img src="../images/windows_simpleshell.gif" data-canonical-src="../images/windows_simpleshell.gif" width="280"/>
            </td>
        </tr>
    </table>
</p>

## Visual states

`SimpleShell` provides multiple groups of visual states which help to define the shell appearance based on the current state of navigation. See [documentation](VisualStates.md) for more information.

## Transitions

`SimpleShell` allows you to define custom transitions between pages during navigation:

<p align="center">
    <img width="350" src="../images/windows_transitions.gif">
</p>

See [documentation](Transitions.md) for more information.

## Implementation details

The `SimpleShell` class is inherited from the .NET MAUI `Shell` class, but all the handlers are implemented from the ground up. These handlers are inspired by the WinUI version of `Shell` handlers.

`SimpleShell` currently does not provide any page transitions. Pages are simply swapped in a container during navigation.

## Why not use `SimpleShell` and use .NET MAUI `Shell` instead

- .NET MAUI `Shell` offers a platform-specific appearance.
- Platform-specific navigation controls that .NET MAUI `Shell` provides probably have better performance than controls composed of multiple .NET MAUI views.
- A `SimpleShell`-based application may not have as good accessibility in some scenarios due to the lack of platform-specific navigation controls. .NET MAUI `Shell` should be accessible out of the box since it uses platform-specific controls.
- Maybe I have implemented something wrong that has a negative impact on the performance, accessibility, or something like that.