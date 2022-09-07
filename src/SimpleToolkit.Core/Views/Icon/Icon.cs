namespace SimpleToolkit.Core
{
    /// <summary>
    /// Control displaying a tinted image.
    /// </summary>
    public class Icon : Image, IIcon
    {
        public static readonly BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(Icon), defaultValue: Colors.Black);

        public virtual Color TintColor
        {
            get => (Color)GetValue(TintColorProperty);
            set => SetValue(TintColorProperty, value);
        }
    }
}
