# SimpleToolkit.SimpleShell.Controls

[![SimpleToolkit.SimpleShell.Controls](https://img.shields.io/nuget/v/SimpleToolkit.SimpleShell.Controls.svg?label=SimpleToolkit.SimpleShell.Controls)](https://www.nuget.org/packages/SimpleToolkit.SimpleShell.Controls/)

*SimpleToolkit.SimpleShell.Controls* is a collection of ready-to-use, navigation-related controls (not only) for `SimpleShell`.

> I am still not decided if I want to continue and how to develop the `SimpleToolkit.SimpleShell.Controls` package. **The package API is likely to change in the future.** For this reason, this package is still in preview and has poor documentation.

<p align="center">
    <a href="https://giphy.com/gifs/wiesemann1893-transparent-logo-wiesemann-dWa2rUaiahx1FB3jor">
        <img width="250" src="https://media1.giphy.com/media/dWa2rUaiahx1FB3jor/giphy.gif?cid=790b7611041afeb62e8c51c2423440574e3473ff3a4cdd49&rid=giphy.gif&ct=g">
    </a>
</p>

## Getting Started

In order to use *SimpleToolkit.SimpleShell.Controls*, you need to call the `UseSimpleToolkit()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleToolkit();
```

## Controls

The package currently supports only two controls:

- `ListPopover` - popover containing a list of selectable text items
- `TabBar`

All controls can be styled using different design languages. These are currently supported:

- Material 3
- Cupertino
- Fluent (WinUI 3)

<p align="center">
    <img width="350" src="../images/listpopovers.png">
</p>

<p align="center">
    <img width="350" src="../images/tabbars.png">
</p>