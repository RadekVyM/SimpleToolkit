# <img src="./images/logo_with_background.svg" width="40" height="25"> SimpleToolkit

SimpleToolkit is a .NET MAUI library of helpers and simple, easily customizable controls.

The library consists of three NuGet packages:

- [SimpleToolkit.Core](#simpletoolkitcore) - set of simple .NET MAUI controls and helpers

    [![NuGet](https://img.shields.io/nuget/v/SimpleToolkit.Core.svg?label=SimpleToolkit.Core)](https://www.nuget.org/packages/SimpleToolkit.Core/)

- [SimpleToolkit.SimpleShell](#simpletoolkitsimpleshell) - simplified implementation of .NET MAUI `Shell`

    [![NuGet](https://img.shields.io/nuget/v/SimpleToolkit.SimpleShell.svg?label=SimpleToolkit.SimpleShell)](https://www.nuget.org/packages/SimpleToolkit.SimpleShell/)

- [SimpleToolkit.SimpleShell.Controls](#simpletoolkitsimpleshellcontrols) - collection of ready-to-use, navigation-related controls

    [![NuGet](https://img.shields.io/nuget/v/SimpleToolkit.SimpleShell.Controls.svg?label=SimpleToolkit.SimpleShell.Controls)](https://www.nuget.org/packages/SimpleToolkit.SimpleShell.Controls/)

I have split the content of this library into three NuGet packages because there may be people who want to use only the `SimpleShell` control, for example, and do not want to use other controls.

> ⚠ **Warning:** Long-term support is not guaranteed. Use at your own risk.

## Samples

Here are some of my samples that were built using this library:

<p align="center">
    <img src="https://raw.githubusercontent.com/RadekVyM/Gadgets-Store-App/maui/samples/gadget_store_home.png" data-canonical-src="https://github.com/RadekVyM/Gadgets-Store-App" width="500" />
</p>
<p align="center">
    <a href="https://github.com/RadekVyM/Gadgets-Store-App"><em>Gadget Store App</em></a>
</p>
<p align="center">
    <img src="https://raw.githubusercontent.com/RadekVyM/Navbar-Animation-1/main/Images/navbaranimation%20gif%20720.gif" data-canonical-src="https://github.com/RadekVyM/Navbar-Animation-1" height="500" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <img src="https://raw.githubusercontent.com/RadekVyM/Navbar-Animation-1/main/Images/iphone_navbaranimation_1.png" data-canonical-src="https://github.com/RadekVyM/Navbar-Animation-1" height="500" />
</p>
<p align="center">
    <a href="https://github.com/RadekVyM/Navbar-Animation-1"><em>Navbar Animation #1</em></a>
</p>
<p align="center">
    <img src="https://raw.githubusercontent.com/RadekVyM/Navbar-Animation-2/main/images/android_navbaranimation_2.gif" data-canonical-src="https://github.com/RadekVyM/Navbar-Animation-2" height="500" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <img src="https://raw.githubusercontent.com/RadekVyM/Navbar-Animation-2/main/images/iphone_navbaranimation_2.png" data-canonical-src="https://github.com/RadekVyM/Navbar-Animation-2" height="500" />
</p>
<p align="center">
    <a href="https://github.com/RadekVyM/Navbar-Animation-2"><em>Navbar Animation #2</em></a>
</p>

## SimpleToolkit.Core

[![SimpleToolkit.Core](https://img.shields.io/nuget/v/SimpleToolkit.Core.svg?label=SimpleToolkit.Core)](https://www.nuget.org/packages/SimpleToolkit.Core/)
[![Documentation](https://img.shields.io/badge/-Documentation%20-green)](./docs/SimpleToolkit.Core)

The *SimpleToolkit.Core* package is a set of simple .NET MAUI controls and helpers.

These are all the controls this package has to offer:

- [Icon](./docs/SimpleToolkit.Core/Icon.md) - control that allows you to display a tinted image
- [ContentButton](./docs/SimpleToolkit.Core/ContentButton.md) - button that can hold whatever content you want
- [Popover](./docs/SimpleToolkit.Core/Popover.md) - control that allows you to display custom popovers (flyouts) anchored to any control

The package also contains some useful helpers for managing safe areas of an application window. For example, there are helpers that allow you to force application content to be displayed behind system bars (status and navigation bars) on Android and iOS. See [documentation](./docs/SimpleToolkit.Core/README.md).

## SimpleToolkit.SimpleShell

[![SimpleToolkit.SimpleShell](https://img.shields.io/nuget/v/SimpleToolkit.SimpleShell.svg?label=SimpleToolkit.SimpleShell)](https://www.nuget.org/packages/SimpleToolkit.SimpleShell/)

The *SimpleToolkit.SimpleShell* package provides you with a simplified implementation of .NET MAUI `Shell` that lets you easily create a custom navigation experience in your .NET MAUI applications.

### Getting Started

In order to use *SimpleToolkit.SimpleShell*, you need to call the `UseSimpleShell()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleShell();
```

### SimpleShell

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
                <img src="./images/readme/android_simpleshell.gif" data-canonical-src="./images/readme/android_popover.gif" width="148"/>
            </td>
            <td>
                <img src="./images/readme/ios_simpleshell.gif" data-canonical-src="./images/readme/android_popover.gif" width="180"/>
            </td>
            <td>
                <img src="./images/readme/windows_simpleshell.gif" data-canonical-src="./images/readme/android_popover.gif" width="280"/>
            </td>
        </tr>
    </table>
</p>

#### Visual states

`SimpleShell` provides multiple groups of states.

#### `ShellSection` states

States in the `SimpleShellSectionState.[ShellSection route]` format:

```xml
<VisualStateManager.VisualStateGroups>
    <VisualStateGroup x:Name="SimpleShellSectionStates">
        <VisualState x:Name="SimpleShellSectionState.HomeTab">
            <VisualState.Setters>
                <Setter TargetName="tabBar" Property="View.Background" Value="Red"/>
            </VisualState.Setters>
        </VisualState>
        <VisualState x:Name="SimpleShellSectionState.SettingsTab">
            <VisualState.Setters>
                <Setter TargetName="tabBar" Property="View.Background" Value="Green"/>
            </VisualState.Setters>
        </VisualState>
    </VisualStateGroup>
</VisualStateManager.VisualStateGroups>
```

When a user navigates to a tab with a `HomeTab` route, the view named `tabBar` will have a red background, and when to a tab with a `SettingsTab` route, the view named `tabBar` will have a green background. 

#### `ShellContent` states

States in the `SimpleShellContentState.[ShellContent route]` format:

```xml
<VisualStateManager.VisualStateGroups>
    <VisualStateGroup x:Name="SimpleShellContentStates">
        <VisualState x:Name="SimpleShellContentState.HomePage">
            <VisualState.Setters>
                <Setter TargetName="tabBar" Property="View.Background" Value="Yellow"/>
            </VisualState.Setters>
        </VisualState>
        <VisualState x:Name="SimpleShellContentState.SettingsPage">
            <VisualState.Setters>
                <Setter TargetName="tabBar" Property="View.Background" Value="Blue"/>
            </VisualState.Setters>
        </VisualState>
    </VisualStateGroup>
</VisualStateManager.VisualStateGroups>
```

When a user navigates to a `ShellContent` with a `HomePage` route, the view named `tabBar` will have a yellow background, and when to a `ShellContent` with a `SettingsPage` route, the view named `tabBar` will have a blue background.

#### Page type states

States in the `SimplePageState.[Page type name]` format:

```xml
<VisualStateManager.VisualStateGroups>
    <VisualStateGroup x:Name="SimplePageStates">
        <VisualState x:Name="SimplePageState.HomePage">
            <VisualState.Setters>
                <Setter TargetName="tabBar" Property="View.Background" Value="Purple"/>
            </VisualState.Setters>
        </VisualState>
        <VisualState x:Name="SimplePageState.SettingsPage">
            <VisualState.Setters>
                <Setter TargetName="tabBar" Property="View.Background" Value="Orange"/>
            </VisualState.Setters>
        </VisualState>
    </VisualStateGroup>
</VisualStateManager.VisualStateGroups>
```

When a user navigates to a `HomePage` page, the view named `tabBar` will have a purple background, and when to a `SettingsPage` page, the view named `tabBar` will have a orange background. 

#### Navigation stack states

When a user navigates to a page that is part of the shell hierarchy, `SimpleShell` goes to the `RootPage` state, otherwise `SimpleShell` goes to the `RegisteredPage` state:

```xml
<VisualStateManager.VisualStateGroups>
    <VisualStateGroup x:Name="SimplePageTypeStates">
        <VisualState x:Name="SimplePageTypeState.RegisteredPage">
            <VisualState.Setters>
                <Setter TargetName="backButton" Property="Button.IsVisible" Value="true"/>
            </VisualState.Setters>
        </VisualState>
        <VisualState x:Name="SimplePageTypeState.RootPage">
            <VisualState.Setters>
                <Setter TargetName="backButton" Property="Button.IsVisible" Value="false"/>
            </VisualState.Setters>
        </VisualState>
    </VisualStateGroup>
</VisualStateManager.VisualStateGroups>
```

#### Why not use `SimpleShell` and use .NET MAUI `Shell` instead

- .NET MAUI `Shell` offers a platform-specific appearance.
- Platform-specific navigation controls that .NET MAUI `Shell` provides probably have better performance than controls composed of multiple .NET MAUI views.
- A `SimpleShell`-based application may not have as good accessibility in some scenarios due to the lack of platform-specific navigation controls. .NET MAUI `Shell` should be accessible out of the box since it uses platform-specific controls.
- Maybe I have implemented something wrong that has a negative impact on the performance, accessibility, or something like that.

#### Implementation details

The `SimpleShell` class is inherited from the .NET MAUI `Shell` class, but all the handlers are implemented from the ground up. These handlers are inspired by the WinUI version of `Shell` handlers.

`SimpleShell` currently does not provide any page transitions. Pages are simply swapped in a container during navigation.

## SimpleToolkit.SimpleShell.Controls

[![SimpleToolkit.SimpleShell.Controls](https://img.shields.io/nuget/v/SimpleToolkit.SimpleShell.Controls.svg?label=SimpleToolkit.SimpleShell.Controls)](https://www.nuget.org/packages/SimpleToolkit.SimpleShell.Controls/)

*SimpleToolkit.SimpleShell.Controls* is a collection of ready-to-use, navigation-related controls (not only) for `SimpleShell`.

### Getting Started

In order to use *SimpleToolkit.SimpleShell.Controls*, you need to call the `UseSimpleToolkit()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleToolkit();
```

<p align="center">
    <a href="https://giphy.com/gifs/wiesemann1893-transparent-logo-wiesemann-dWa2rUaiahx1FB3jor">
        <img width="250" src="https://media1.giphy.com/media/dWa2rUaiahx1FB3jor/giphy.gif?cid=790b7611041afeb62e8c51c2423440574e3473ff3a4cdd49&rid=giphy.gif&ct=g">
    </a>
</p>