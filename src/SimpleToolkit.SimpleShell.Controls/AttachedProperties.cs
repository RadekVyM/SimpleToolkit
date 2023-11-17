namespace SimpleToolkit.SimpleShell.Controls;

public static class SimpleShellIcon
{
    public static readonly BindableProperty SelectedIconProperty =
        BindableProperty.CreateAttached("SelectedIcon", typeof(ImageSource), typeof(BaseShellItem), null);

    /// <summary>
    /// Returns an <see cref="ImageSource"/> of an image that should be displayed when the item is selected.
    /// </summary>
    /// <param name="item">The item to which the <see cref="ImageSource"/> is attached.</param>
    /// <returns>The <see cref="ImageSource"/> that is attached to the item.</returns>
    public static ImageSource GetSelectedIcon(BindableObject item)
    {
        _ = item ?? throw new ArgumentNullException(nameof(item));
        return (ImageSource)item.GetValue(SelectedIconProperty);
    }

    /// <summary>
    /// Attaches to the item an <see cref="ImageSource"/> of an image that should be displayed when the item is selected.
    /// </summary>
    /// <param name="item">The item to which the <see cref="ImageSource"/> will be attached.</param>
    /// <param name="value">The <see cref="ImageSource"/> of an image that will be attached to the item.</param>
    public static void SetSelectedIcon(BindableObject item, ImageSource value)
    {
        _ = item ?? throw new ArgumentNullException(nameof(item));
        item.SetValue(SelectedIconProperty, value);
    }
}