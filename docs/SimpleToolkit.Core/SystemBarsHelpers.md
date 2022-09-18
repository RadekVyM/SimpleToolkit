# System bars helpers

Helpers for changing the appearance of system bars **on Android**:

```csharp
#if ANDROID

this.Window.SetStatusBarAppearance(color: Colors.DarkOrange, lightElements: true);
this.Window.SetNavigationBarAppearance(color: Colors.White, lightElements: false);

#endif
```

Appearance of the status bar can be changed using the `SetStatusBarAppearance()` extension method of a window. Appearance of the navigation bar can be changed using the `SetNavigationBarAppearance()` extension method of a window. Both methods take two parameters:

- `color` - new background color of the bar
- `lightElements` - if text and icons should be ligth or dark

Output:

<p align="center">
    <img src="../images/pixel_system_bars.png" data-canonical-src="../images/pixel_system_bars.png" width="220" />
</p>

> This can also be done using the [.NET MAUI Community Toolkit](https://github.com/CommunityToolkit/Maui) features that offer more options.

## Default appearance

Default appearance of the bars for each window can be changed using the `SetDefaultStatusBarAppearance()` and `SetDefaultNavigationBarAppearance()` extension methods. These methods has to be called on a `MauiAppBuilder` instance in your `MauiProgram.cs` file:

```csharp
#if ANDROID

builder.SetDefaultStatusBarAppearance(color: Colors.DarkOrange, lightElements: true);
builder.SetDefaultNavigationBarAppearance(color: Colors.White, lightElements: false);

#endif
```