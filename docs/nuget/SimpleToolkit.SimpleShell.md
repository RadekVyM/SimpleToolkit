# SimpleToolkit.SimpleShell

The *SimpleToolkit.SimpleShell* package provides you with a simplified implementation of .NET MAUI `Shell` that lets you easily create a custom navigation experience in your .NET MAUI applications.

## Getting Started

In order to use *SimpleToolkit.SimpleShell*, you need to call the `UseSimpleShell()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleShell();
```

## SimpleShell

`SimpleShell` is a simplified implementation of .NET MAUI `Shell`. All `SimpleShell` is is just a simple container for your content with the ability to put the hosting area for pages wherever you want, giving you the flexibility to add custom tab bars, navigation bars, flyouts, etc. to your `Shell` application while using great `Shell` URI-based navigation.

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
- `RootPageContainer` - sets a view that will wrap all your root pages (`ShellContent`s)

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

## Why not use `SimpleShell` and use .NET MAUI `Shell` instead

- .NET MAUI `Shell` offers a platform-specific appearance.
- Platform-specific navigation controls that .NET MAUI `Shell` provides probably have better performance than controls composed of multiple .NET MAUI views.
- A `SimpleShell`-based application may not have as good accessibility in some scenarios due to the lack of platform-specific navigation controls. .NET MAUI `Shell` should be accessible out of the box since it uses platform-specific controls.
- Maybe I have implemented something wrong that has a negative impact on the performance, accessibility, or something like that.

> See [documentation](https://github.com/RadekVyM/SimpleToolkit/tree/main/docs/SimpleToolkit.SimpleShell) for more information.