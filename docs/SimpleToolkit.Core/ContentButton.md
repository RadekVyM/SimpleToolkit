# ContentButton

`ContentButton` is just a button that can hold whatever content you want. In order to use this control, you need to call the `UseSimpleToolkit()` extension method in your `MauiProgram.cs` file:

```csharp
builder.UseSimpleToolkit();
```

`ContentButton` can be found in the following XAML namespace:

```xml
xmlns:simpleCore="clr-namespace:SimpleToolkit.Core;assembly=SimpleToolkit.Core"
```

## Example

Let's define a `ContentButton` with an `Icon` and `Label`:

```xml
<simpleCore:ContentButton
    Clicked="StarButtonClicked"
    Background="Orange"
    HorizontalOptions="Center"
    StrokeShape="{RoundRectangle CornerRadius=6}">
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
</simpleCore:ContentButton>
```

The button border can be modified the same way as the `Border` control. Output:

<p align="center">
    <img src="../images/star_button.png" data-canonical-src="../images/star_button.png" />
</p>

## Visual states

`ContentButton` provides the same visual states as .NET MAUI `Button` does:

```xml
<simpleCore:ContentButton
    Clicked="StarButtonClicked"
    Background="Orange"
    HorizontalOptions="Center"
    StrokeShape="{RoundRectangle CornerRadius=6}">
    <VisualStateManager.VisualStateGroups>
        <VisualStateGroupList>
            <VisualStateGroup>
                <VisualState x:Name="Normal"/>
                <VisualState x:Name="Pressed">
                    <VisualState.Setters>
                        <Setter
                            Property="Background" 
                            Value="OrangeRed"/>
                    </VisualState.Setters>
                </VisualState>
                <VisualState x:Name="PointerOver">
                    <VisualState.Setters>
                        <Setter
                            Property="Background" 
                            Value="DarkOrange"/>
                    </VisualState.Setters>
                </VisualState>
            </VisualStateGroup>
        </VisualStateGroupList>
    </VisualStateManager.VisualStateGroups>
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
</simpleCore:ContentButton>
```

Output:

<p align="center">
    <img src="../images/star_button_visual_states.gif" data-canonical-src="../images/star_button_visual_states.gif" width="250"/>
</p>

## Implementation details

Since version 5.0.0, the `ContentButton` class is inherited from the .NET MAUI `Border` control. `ContentButton` has these events and properties in addition to `Border`s events and properties:

- `Clicked` - an event that fires when the button is clicked
- `Pressed` - an event that fires when the button is pressed
- `Released` - an event that fires when the button is released
- `Command` - a command that is invoked when the button is clicked
- `CommandParameter` - a parameter to pass to the `Command` property
