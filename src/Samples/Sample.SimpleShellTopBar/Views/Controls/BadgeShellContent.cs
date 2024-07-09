namespace Sample.SimpleShellTopBar.Views.Controls;

public class BadgeShellContent : ShellContent
{
    public static readonly BindableProperty BadgeTextProperty =
        BindableProperty.Create(nameof(BadgeText), typeof(string), typeof(BadgeShellContent), defaultBindingMode: BindingMode.OneWay);

    public string? BadgeText
    {
        get => (string?)GetValue(BadgeTextProperty);
        set => SetValue(BadgeTextProperty, value);
    }
}