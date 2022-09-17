# Safe area helpers

Helpers for managing the safe area of an application window.

## Application content behind system bars

Thanks to the `DisplayContentUnderBars()` extension method, you can force application content to be displayed behind system bars (status and navigation bars) on **Android** and **iOS**. Just call the method on a `MauiAppBuilder` instance in your `MauiProgram.cs` file:

```csharp
builder.DisplayContentUnderBars();
```

Output:

TODO: images

The method also sets the status bar background color to transparent and the text color to dark on Android to match the look with iOS.

## Safe area insets

Safe area insets can be obtained by subscribing to safe area changes using the `SubscribeToSafeAreaChanges()` extension method of a window:

```csharp
protected override void OnNavigatedTo(NavigatedToEventArgs args)
{
    base.OnNavigatedTo(args);

    this.Window.SubscribeToSafeAreaChanges(OnSafeAreaChanged);
}

private void OnSafeAreaChanged(Thickness safeAreaPadding)
{
    rootContainer.Padding = safeAreaPadding;
}
```

The `SubscribeToSafeAreaChanges()` method requires a method of type `Action<Thickess>` as a parameter. The passed method will be called every time the safe area of the window changes.

Subscription of the safe area changes can be canceled using the `UnsubscribeFromSafeAreaChanges()` extension method. This method takes as a parameter the method that you want to unsubscribe:

```csharp
protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
{
    base.OnNavigatedFrom(args);

    this.Window.UnsubscribeFromSafeAreaChanges(OnSafeAreaChanged);
}
```