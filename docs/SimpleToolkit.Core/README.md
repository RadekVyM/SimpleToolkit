# SimpleToolkit.Core

[![SimpleToolkit.Core](https://img.shields.io/nuget/v/SimpleToolkit.Core.svg?label=SimpleToolkit.Core)](https://www.nuget.org/packages/SimpleToolkit.Core/)

The *SimpleToolkit.Core* package is a set of simple .NET MAUI controls and helpers.

## Controls

These are all the controls this package has to offer:

- [Icon](Icon.md) - control that allows you to display a tinted image
- [ContentButton](ContentButton.md) - button that can hold whatever content you want
- [Popover](Popover.md) - control that allows you to display custom popovers (flyouts) anchored to any control

### Getting Started

In order to use the controls listed above, you need to call the `UseSimpleToolkit()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleToolkit();
```

## Helpers

These are all the helpers this package has to offer:

- [Safe area helpers](SafeAreaHelpers.md) - helpers for managing the safe area of an application window
- [System bars helpers](SystemBarsHelpers.md) - helpers for changing the appearance of system bars on Android
- Bounds helper - `GetBounds()` extension method returning the bounds of a `VisualElement` relative to another `VisualElement` or the app screen if no element is passed as an argument