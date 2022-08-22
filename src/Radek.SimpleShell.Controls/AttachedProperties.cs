namespace Radek.SimpleShell.Controls
{
    public static class SimpleIcon
    {
        public static readonly BindableProperty SelectedIconProperty =
            BindableProperty.CreateAttached("SelectedIcon", typeof(ImageSource), typeof(BaseShellItem), null);

        public static ImageSource GetSelectedIcon(BindableObject view)
        {
            return (ImageSource)view.GetValue(SelectedIconProperty);
        }

        public static void SetSelectedIcon(BindableObject view, ImageSource value)
        {
            view.SetValue(SelectedIconProperty, value);
        }
    }
}
