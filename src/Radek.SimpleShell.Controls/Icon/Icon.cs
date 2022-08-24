namespace Radek.SimpleShell.Controls
{
    public class Icon : Image
    {
        public static readonly BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(Icon), defaultValue: Colors.Black);

        public virtual Color TintColor
        {
            get => (Color)GetValue(TintColorProperty);
            set => SetValue(TintColorProperty, value);
        }
    }
}
