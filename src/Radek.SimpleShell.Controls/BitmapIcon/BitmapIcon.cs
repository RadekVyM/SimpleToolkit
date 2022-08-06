using IImage = Microsoft.Maui.IImage;

namespace Radek.SimpleShell.Controls
{
    // TODO: BitmapIcon should probably not inherit from Image - IsAnimationPlaying and Aspect does not make sense on Windows
    public class BitmapIcon : Image, IImage
    {
        public static readonly BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(TabView), defaultValue: Colors.Black);

        public virtual Color TintColor
        {
            get => (Color)GetValue(TintColorProperty);
            set => SetValue(TintColorProperty, value);
        }
    }
}
