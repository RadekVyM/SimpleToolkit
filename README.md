# <img src="./images/logo_with_background.svg" width="40" height="22"> SimpleToolkit

SimpleToolkit is a .NET MAUI library of helpers and simple, easily customizable controls.

The library consists of three packages:

- [SimpleToolkit.Core](#simpletoolkitcore)

- [SimpleToolkit.SimpleShell](#simpletoolkitsimpleshell)

- [SimpleToolkit.SimpleShell.Controls](#simpletoolkitsimpleshellcontrols)

> ⚠ **Warning:** Long-term support is not guaranteed. Use at your own Risk.

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

![SimpleToolkit.Core](https://img.shields.io/nuget/dt/SimpleToolkit.Core)

SimpleToolkit.Core package is a set of simple .NET MAUI controls and helpers.

> The package does not depend on any other packages.

### Getting Started

In order to use the SimpleToolkit.Core you need to call the `UseSimpleToolkit()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleToolkit();
```

### Icon

Thanks to the `Icon` control you are able to display a tinted image.

```xml
<simpleCore:Icon Source="star.png"/>
<simpleCore:Icon Source="star.png" TintColor="Gray"/>
<simpleCore:Icon Source="star.png" TintColor="Orange"/>
```

Output:

<p align="center">
    <img src="./images/readme/stars.png" data-canonical-src="./images/readme/stars.png" />
</p>

#### Implementation details

The `Icon` class is inherited from .NET MAUI `Image` class, but behind the scenes is implemented in the same way as .NET MAUI `Image` only on Android and iOS. WinUI implementation is based on `BitmapIcon` and `FontIcon` controls. Because of that the control supports only these image sources on Windows:

- `FileImageSource`
- `UriImageSource`
- `FontImageSource`

These `Image` properties are not supported at all:

- `Aspect` - the default behavior is `AspectFit`
- `IsAnimationPlaying`
- `IsLoading`
- `IsOpaque`

### ContentButton

The `ContentButton` is just a button that can hold whatever content you want. 

```xml
<simpleCore:ContentButton Clicked="StarButtonClicked">
    <Border Background="Orange">
        <Border.StrokeShape>
            <RoundRectangle CornerRadius="6"/>
        </Border.StrokeShape>
        <HorizontalStackLayout Padding="12,10" Spacing="10">
            <simpleCore:Icon
                Source="star.png" TintColor="White"
                VerticalOptions="Center"
                HeightRequest="18" WidthRequest="18"/>
            <Label
                Text="Star this repo" TextColor="White"
                FontAttributes="Bold"
                VerticalOptions="Center"/>
        </HorizontalStackLayout>
    </Border>
</simpleCore:ContentButton>
```

Output:

<p align="center">
    <img src="./images/readme/star_button.png" data-canonical-src="./images/readme/star_button.png" />
</p>

#### Implementation details

The `ContentButton` class is inherited from .NET MAUI `ContentView` control. The `ContentButton` has these events in addition to `ContentView`s events and properties:

- `Clicked` - an event that fires when the button is clicked
- `Pressed` - an event that fires when the button is pressed
- `Released` - an event that fires when the button is released

### Popover

The `Popover` allows you to display custom popovers (flyouts) anchored to any control.

```xml

```

In code behind:

```csharp

```

Output:

<p align="center">
    <img src="./images/readme/star_button.png" data-canonical-src="./images/readme/star_button.png" />
</p>

#### Implementation details

The `Popover` itself does not support any styling because.

## SimpleToolkit.SimpleShell

![SimpleToolkit.SimpleShell](https://img.shields.io/nuget/dt/SimpleToolkit.SimpleShell)

SimpleToolkit.SimpleShell package provides you a simplified implementation of .NET MAUI `Shell` that allows you to easily create custom navigation experiences in your .NET MAUI applications. 


> The package does not depend on any other packages.

### Getting Started

In order to use the SimpleToolkit.Core you need to call the `UseSimpleShell()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleShell();
```

### SimpleShell

`SimpleShell` is a simplified implementation of .NET MAUI `Shell`. This is the hilight of the library.

## SimpleToolkit.SimpleShell.Controls

![SimpleToolkit.SimpleShell.Controls](https://img.shields.io/nuget/dt/SimpleToolkit.SimpleShell.Controls)

The SimpleToolkit.SimpleShell.Controls is a collection of ready-to-use navigation related controls (not only) for SimpleShell

### Getting Started

In order to use the SimpleToolkit.Core you need to call the `UseSimpleToolkit()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleToolkit();
```

### TabBar

Work in progress.