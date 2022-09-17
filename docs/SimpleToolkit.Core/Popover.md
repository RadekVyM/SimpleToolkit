# Popover

`Popover` allows you to display custom popovers (flyouts) anchored to any control:

```xml
<Button
    VerticalOptions="Center" HorizontalOptions="Center"
    Clicked="ButtonClicked"
    Text="Show popover"
    Background="Orange">
    <simpleCore:Popover.AttachedPopover>
        <simpleCore:Popover>
            <Border
                Background="DarkOrange">
                <Border.StrokeShape>
                    <RoundRectangle CornerRadius="6"/>
                </Border.StrokeShape>

                <VerticalStackLayout Padding="12,10" Spacing="10">
                    <simpleCore:Icon
                        Source="star.png" TintColor="White"
                        VerticalOptions="Center"
                        HeightRequest="25" WidthRequest="25"/>
                    <Label
                        Text="Star this repo" TextColor="White"
                        FontAttributes="Bold"
                        VerticalOptions="Center"/>
                </VerticalStackLayout>
            </Border>
        </simpleCore:Popover>
    </simpleCore:Popover.AttachedPopover>
</Button>
```

Code behind:

```csharp
private void ButtonClicked(object sender, EventArgs e)
{
    var button = sender as Button;

    button.ShowAttachedPopover();
}
```

Output:

<p align="center">
    <table>
        <tr>
            <th>
                <p align="center">Android</p>
            </th>
            <th>
                <p align="center">iOS</p>
            </th>
            <th>
                <p align="center">Windows</p>
            </th>
        </tr>
        <tr>
            <td>
                <img src="../../images/readme/android_popover.gif" data-canonical-src="../../images/readme/android_popover.gif" width="200"/>
            </td>
            <td>
                <img src="../../images/readme/ios_popover.gif" data-canonical-src="../../images/readme/android_popover.gif" width="200"/>
            </td>
            <td>
                <img src="../../images/readme/windows_popover.gif" data-canonical-src="../../images/readme/android_popover.gif" width="200"/>
            </td>
        </tr>
    </table>
</p>

## Implementation details

The `Popover` class is inherited from the .NET MAUI `Element` class. `Popover` offers these properties and methods in addition to `Element`s properties and methods:

- `Content` - the popover content of type `View`
- `Show()` - shows the popover anchored to a view you pass as a parameter
- `Hide()` - hides the popover

Use of the methods mentioned above:

```csharp
popover.Show(anchorView);
popover.Hide();
```

Popover can be attached to a view using the `AttachedPopover` attached property. Such a popover can be displayed or hidden (dismissed) by calling the `ShowAttachedPopover()` and `HideAttachedPopover()` extension methods on the view:

```csharp
button.ShowAttachedPopover();
button.HideAttachedPopover();
```