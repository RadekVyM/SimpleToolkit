# ContentButton

In order to use the control, you need to call the `UseSimpleToolkit()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleToolkit();
```

`ContentButton` is just a button that can hold whatever content you want:

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
    <img src="../images/star_button.png" data-canonical-src="../images/star_button.png" />
</p>

## Implementation details

The `ContentButton` class is inherited from the .NET MAUI `ContentView` control. `ContentButton` has these events and properties in addition to `ContentView`s events and properties:

- `Clicked` - an event that fires when the button is clicked
- `Pressed` - an event that fires when the button is pressed
- `Released` - an event that fires when the button is released
- `Command` - a command that is invoked when the button is clicked
- `CommandParameter` - a parameter to pass to the `Command` property
