<div align="center">

<p>
<img src="./images/logo_with_background.svg" width="40" height="40">
</p>

# SimpleToolkit

<br />

**SimpleToolkit** is a collection of helpers and lightweight, easily customizable .NET MAUI controls designed to give developers full control over their app's UI and navigation.

</div>

<br />

> [!CAUTION]
> Long-term support is not guaranteed. However, this repository is released under the MIT license, so you can always fork the repository and build the packages yourself.

## 📦 NuGet Packages

The library is split into modular packages so you only pull in what you need:

- [SimpleToolkit.SimpleShell](#-simpletoolkitsimpleshell) — a lightweight, decoupled implementation of .NET MAUI `Shell`

  [![NuGet](https://img.shields.io/nuget/v/SimpleToolkit.SimpleShell.svg?label=SimpleToolkit.SimpleShell)](https://www.nuget.org/packages/SimpleToolkit.SimpleShell/)

- [SimpleToolkit.SimpleButton](#-simpletoolkitsimplebutton) — a button that can hold whatever content you want

  [![NuGet](https://img.shields.io/nuget/v/SimpleToolkit.SimpleButton.svg?label=SimpleToolkit.SimpleButton)](https://www.nuget.org/packages/SimpleToolkit.SimpleButton/)

- [SimpleToolkit.Helpers](#-simpletoolkithelpers) — a collection of essential cross-platform helpers for .NET MAUI

  [![NuGet](https://img.shields.io/nuget/v/SimpleToolkit.Helpers.svg?label=SimpleToolkit.Helpers)](https://www.nuget.org/packages/SimpleToolkit.Helpers/)

## 📱 [Samples](./docs/Samples.md)

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
    <img src="https://raw.githubusercontent.com/RadekVyM/Gadgets-Store-App/main/samples/android_gadgets_store_app.webp" width="234" />
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
> View the full list of available samples [here](./docs/Samples.md).

## ✅ Supported Platforms

* **Android**
* **iOS / Mac Catalyst**
* **Windows (WinUI)**

## 🌐 SimpleToolkit.SimpleShell

[![SimpleToolkit.SimpleShell](https://img.shields.io/nuget/v/SimpleToolkit.SimpleShell.svg?label=SimpleToolkit.SimpleShell)](https://www.nuget.org/packages/SimpleToolkit.SimpleShell/)
[![Documentation](https://img.shields.io/badge/-Documentation%20-forestgreen)](./docs/SimpleToolkit.SimpleShell)

`SimpleShell` is a lightweight, decoupled implementation of .NET MAUI `Shell`. It allows you to create entirely custom navigation experiences while retaining the core benefits of `Shell`.

All `SimpleShell` is is just a set of containers for your application content with the ability to put the hosting area for pages wherever you want. This gives you the **flexibility** to add custom tab bars, navigation bars, flyouts, etc. to your `Shell` application.

Bear in mind that **`SimpleShell` does not come with any navigation controls.** `SimpleShell` just gives you the ability to use custom navigation controls along with the URI-based navigation and automatic navigation stack management.

> [!IMPORTANT]
> Before you begin using `SimpleShell`, I highly recommend familiarizing yourself with the standard .NET MAUI `Shell` — especially with the URI-based [navigation](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation), which works exactly the same as in `SimpleShell`.

### Initialization

Initialize `SimpleShell` in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleShell();
```

### Limitations and Trade-offs

While `SimpleShell` offers increased flexibility, there are scenarios where the standard .NET MAUI `Shell` might be a better fit:

* **Platform-Native Aesthetics:** The standard `Shell` provides a look and feel that is native to each specific operating system.
* **Performance Optimization:** Native navigation controls provided by the standard `Shell` may offer superior performance compared to custom controls composed of multiple .NET MAUI views.
* **Accessibility:** Standard `Shell` is designed to be accessible out of the box by leveraging platform-specific controls. A `SimpleShell` implementation requires manual effort to ensure it meets the same accessibility standards.
* **Maturity:** As a custom implementation, `SimpleShell` may have edge cases or performance impacts that have not yet been as rigorously tested as the official MAUI components.

> See [documentation](./docs/SimpleToolkit.SimpleShell) for more information.

## 🔲 SimpleToolkit.SimpleButton

[![SimpleToolkit.SimpleButton](https://img.shields.io/nuget/v/SimpleToolkit.SimpleButton.svg?label=SimpleToolkit.SimpleButton)](https://www.nuget.org/packages/SimpleToolkit.SimpleButton/)
[![Documentation](https://img.shields.io/badge/-Documentation%20-forestgreen)](./docs/SimpleToolkit.SimpleButton)

`SimpleButton` is a button control that can hold whatever content you want.

### Initialization

Initialize `SimpleButton` in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleButton();
```

> See [documentation](./docs/SimpleToolkit.SimpleButton) for more information.

## 📐 SimpleToolkit.Helpers

[![SimpleToolkit.Helpers](https://img.shields.io/nuget/v/SimpleToolkit.Helpers.svg?label=SimpleToolkit.Helpers)](https://www.nuget.org/packages/SimpleToolkit.Helpers/)
[![Documentation](https://img.shields.io/badge/-Documentation%20-forestgreen)](./docs/SimpleToolkit.Helpers)

A collection of essential cross-platform helpers for .NET MAUI to bridge the gap between shared code and native platform APIs:

- `WindowInsetsProvider` — Provides a unified way to retrieve current window insets.

> See [documentation](./docs/SimpleToolkit.Helpers) for more information.
