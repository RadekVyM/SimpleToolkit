# SimpleToolkit.SimpleButton

`SimpleButton` is a flexible .NET MAUI control that inherits from `Border`, allowing you to wrap any content and turn it into a fully functional button.

## 🚀 Getting Started

1. **Initialize**: Call the `UseSimpleButton()` extension method in your `MauiProgram.cs` file:
```csharp
builder.UseSimpleButton();
```

2. **Namespace**: Add the following XAML namespace to your page:
```xml
xmlns:sb="clr-namespace:SimpleToolkit.SimpleButton;assembly=SimpleToolkit.SimpleButton"
```

## 📖 Usage Example

Let's define a `SimpleButton` with an `Image` and `Label`:

```xml
<sb:SimpleButton
    Clicked="StarButtonClicked"
    Background="Orange"
    HorizontalOptions="Center"
    StrokeShape="{RoundRectangle CornerRadius=6}">
    <HorizontalStackLayout Padding="12,10" Spacing="10">
        <Image
            Source="star.png" TintColor="White"
            VerticalOptions="Center"
            HeightRequest="18" WidthRequest="18"/>
        <Label
            Text="Star this repo" TextColor="White"
            FontAttributes="Bold"
            VerticalOptions="Center"/>
    </HorizontalStackLayout>
</sb:SimpleButton>
```

Output:

<p align="center">
    <img src="../images/star_button.png" data-canonical-src="../images/star_button.png" />
</p>

## 🎨 Visual States

`SimpleButton` supports standard .NET MAUI visual states to provide touch feedback:

```xml
<sb:SimpleButton
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
        <Image
            Source="star.png" TintColor="White"
            VerticalOptions="Center"
            HeightRequest="18" WidthRequest="18"/>
        <Label
            Text="Star this repo" TextColor="White"
            FontAttributes="Bold"
            VerticalOptions="Center"/>
    </HorizontalStackLayout>
</sb:SimpleButton>
```

Output:

<p align="center">
    <img src="../images/star_button_visual_states.gif" data-canonical-src="../images/star_button_visual_states.gif" width="250"/>
</p>

## 🛠 API Reference

Since `SimpleButton` inherits from `Border`, it includes all standard Border properties plus:

| Member | Description |
| --- | --- |
| **`Clicked`** | Event fired when the button is tapped/clicked. |
| **`Pressed`** | Event fired when the button is pressed. |
| **`Released`** | Event fired when the touch is released. |
| **`Command`** | `ICommand` invoked on click. |
| **`CommandParameter`** | Object passed to the `Command`. |