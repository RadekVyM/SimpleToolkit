# SimpleToolkit.Core

The *SimpleToolkit.Core* package is a set of simple .NET MAUI controls and helpers.

## Controls

These are all the controls this package has to offer:

- `Icon` - control that allows you to display a tinted image
- `ContentButton` - button that can hold whatever content you want
- `Popover` - control that allows you to display custom popovers (flyouts) anchored to any control

### Getting Started

In order to use the controls listed above, you need to call the `UseSimpleToolkit()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleToolkit();
```

## Helpers

These are all the helpers this package has to offer:

- Safe area helpers - helpers for managing the safe area of an application window
- System bars helpers - helpers for changing the appearance of system bars on Android

> See [documentation](https://github.com/RadekVyM/SimpleToolkit/tree/main/docs/SimpleToolkit.Core) for more information.