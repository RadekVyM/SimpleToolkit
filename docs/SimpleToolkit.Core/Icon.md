# Icon

In order to use the control, you need to call the `UseSimpleToolkit()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleToolkit();
```

Thanks to the `Icon` control, you are able to display a tinted image:

```xml
<simpleCore:Icon Source="star.png"/>
<simpleCore:Icon Source="star.png" TintColor="Gray"/>
<simpleCore:Icon Source="star.png" TintColor="Orange"/>
```

Output:

<p align="center">
    <img src="../images/stars.png" data-canonical-src="../images/stars.png" />
</p>

## Implementation details

The `Icon` class is inherited from the .NET MAUI `Image` class, but behind the scenes it is implemented in the same way as .NET MAUI `Image` **only** on Android and iOS. WinUI implementation is based on `BitmapIcon` and `FontIcon` controls. Because of that, the control supports only these image sources on Windows:

- `FileImageSource`
- `UriImageSource`
- `FontImageSource`

These `Image` properties are **not supported** at all:

- `Aspect` - the default behavior is `AspectFit`
- `IsAnimationPlaying`
- `IsLoading`
- `IsOpaque`
