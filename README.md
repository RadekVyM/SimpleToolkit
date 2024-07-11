# <img src="./images/logo_with_background.svg" width="40" height="25"> SimpleToolkit

SimpleToolkit is a .NET MAUI library of helpers and simple, easily customizable controls.

The library consists of these NuGet packages:

- [SimpleToolkit.Core](#simpletoolkitcore) - set of simple .NET MAUI controls and helpers

  [![NuGet](https://img.shields.io/nuget/v/SimpleToolkit.Core.svg?label=SimpleToolkit.Core)](https://www.nuget.org/packages/SimpleToolkit.Core/)

- [SimpleToolkit.SimpleShell](#simpletoolkitsimpleshell) - simplified implementation of .NET MAUI `Shell`

  [![NuGet](https://img.shields.io/nuget/v/SimpleToolkit.SimpleShell.svg?label=SimpleToolkit.SimpleShell)](https://www.nuget.org/packages/SimpleToolkit.SimpleShell/)

I have split the content of this library into multiple NuGet packages because there may be people who want to use only the `SimpleShell` control, for example, and do not want to use other controls.

> [!CAUTION]
> Long-term support is not guaranteed. However, this repository is released under the MIT license, so you can always fork the repository and build the packages yourself.

## [Samples](./docs/Samples.md)

Here are some of my samples that were built using this library:

<p align="center">
    <img src="https://raw.githubusercontent.com/RadekVyM/MarvelousMAUI/main/images/android_illustrations_20.gif" width="230" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <img src="https://raw.githubusercontent.com/RadekVyM/MarvelousMAUI/main/images/iphone_wonders_transitions_20.gif" width="239" />
</p>
<p align="center">
    <a href="https://github.com/RadekVyM/MarvelousMAUI"><em>Marvelous .NET MAUI</em></a>
</p>
<p align="center">
    <img src="https://raw.githubusercontent.com/RadekVyM/Gadgets-Store-App/main/samples/ios_gadgets_store_app.gif" width="239" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <img src="https://raw.githubusercontent.com/RadekVyM/Bet-App/main/Images/ios_betapp.webp" width="236" />
</p>
<p align="center">
    <a href="https://github.com/RadekVyM/Gadgets-Store-App"><em>Gadget Store App</em></a>
    &nbsp;|&nbsp;
    <a href="https://github.com/RadekVyM/Bet-App"><em>Bet App</em></a>
</p>
<p align="center">
    <img src="https://raw.githubusercontent.com/RadekVyM/Navbar-Animation-1/main/Images/android_navbaranimation1.webp" width="230" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <img src="https://raw.githubusercontent.com/RadekVyM/Navbar-Animation-2/main/images/iphone_navbaranimation_2.webp" width="236" />
</p>
<p align="center">
    <a href="https://github.com/RadekVyM/Navbar-Animation-1"><em>Navbar Animation #1</em></a>
    &nbsp;|&nbsp;
    <a href="https://github.com/RadekVyM/Navbar-Animation-2"><em>Navbar Animation #2</em></a>
</p>
<p align="center">
    <img src="https://raw.githubusercontent.com/RadekVyM/HamburgerMenuApp/main/images/android.gif" width="230" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <img src="https://raw.githubusercontent.com/RadekVyM/Waste-App/main/Images/ios_wasteapp.webp" width="236" />
</p>
<p align="center">
    <a href="https://github.com/RadekVyM/HamburgerMenuApp"><em>Hamburger Menu App</em></a>
    &nbsp;|&nbsp;
    <a href="https://github.com/RadekVyM/Waste-App"><em>Waste App</em></a>
</p>

> [!TIP]
> Check out a list of all samples [here](./docs/Samples.md).

## Supported platforms

This library is built for the following platforms:

- Android
- iOS/Mac Catalyst
- Windows (WinUI)

## SimpleToolkit.Core

[![SimpleToolkit.Core](https://img.shields.io/nuget/v/SimpleToolkit.Core.svg?label=SimpleToolkit.Core)](https://www.nuget.org/packages/SimpleToolkit.Core/)
[![Documentation](https://img.shields.io/badge/-Documentation%20-forestgreen)](./docs/SimpleToolkit.Core)

The _SimpleToolkit.Core_ package is a set of simple .NET MAUI controls and helpers.

These are all the controls this package has to offer:

- [Icon](./docs/SimpleToolkit.Core/Icon.md) - control that allows you to display a tinted image
- [ContentButton](./docs/SimpleToolkit.Core/ContentButton.md) - button that can hold whatever content you want
- [Popover](./docs/SimpleToolkit.Core/Popover.md) - control that allows you to display custom popovers (flyouts) anchored to any control

In order to use the controls listed above, you need to call the `UseSimpleToolkit()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleToolkit();
```

This package also contains some useful helpers. For example, there are helpers that allow you to force application content to be displayed behind the system bars (status and navigation bars) on Android and iOS.

See [documentation](./docs/SimpleToolkit.Core) for more information.

## SimpleToolkit.SimpleShell

[![SimpleToolkit.SimpleShell](https://img.shields.io/nuget/v/SimpleToolkit.SimpleShell.svg?label=SimpleToolkit.SimpleShell)](https://www.nuget.org/packages/SimpleToolkit.SimpleShell/)
[![Documentation](https://img.shields.io/badge/-Documentation%20-forestgreen)](./docs/SimpleToolkit.SimpleShell)

The _SimpleToolkit.SimpleShell_ package provides you with a simplified implementation of .NET MAUI `Shell` that lets you easily create a custom navigation experience in your .NET MAUI applications. The implementation is simply called `SimpleShell`.

All `SimpleShell` is is just a set of containers for your application content with the ability to put the hosting area for pages wherever you want. This gives you the **flexibility** to add custom tab bars, navigation bars, flyouts, etc. to your `Shell` application.

Bear in mind that **`SimpleShell` does not come with any navigation controls.** `SimpleShell` just gives you the ability to use custom navigation controls along with the URI-based navigation and automatic navigation stack management.

> [!IMPORTANT]
> Before you begin using `SimpleShell`, I highly recommend familiarizing yourself with the original .NET MAUI `Shell` - especially with the URI-based [navigation](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation), which works exactly the same as in `SimpleShell`. The `SimpleShell` class inherits from the `Shell` class.

In order to use `SimpleShell`, you need to call the `UseSimpleShell()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleShell();
```

### Why not use `SimpleShell` and use .NET MAUI `Shell` instead

- .NET MAUI `Shell` offers a platform-specific appearance.
- Platform-specific navigation controls that .NET MAUI `Shell` provides probably have better performance than controls composed of multiple .NET MAUI views.
- A `SimpleShell`-based application may not have as good accessibility in some scenarios due to the lack of platform-specific navigation controls. .NET MAUI `Shell` should be accessible out of the box since it uses platform-specific controls.
- Maybe I have implemented something wrong that has a negative impact on the performance, stability, accessibility, or something like that.

See [documentation](./docs/SimpleToolkit.SimpleShell) for more information.