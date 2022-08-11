namespace Radek.SimpleShell.Controls
{
    public class BitmapIcon : Image
    {
        public static readonly BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(BitmapIcon), defaultValue: Colors.Black);

        public virtual Color TintColor
        {
            get => (Color)GetValue(TintColorProperty);
            set => SetValue(TintColorProperty, value);
        }
    }
}
