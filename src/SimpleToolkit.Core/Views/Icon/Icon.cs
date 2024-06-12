using System.ComponentModel;

namespace SimpleToolkit.Core;

/// <summary>
/// Control displaying a tinted image.
/// </summary>
public class Icon : Image, IIcon
{
    public static readonly BindableProperty TintColorProperty =
        BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(Icon), defaultValue: Colors.Black);

    public virtual Color TintColor
    {
        get => (Color)GetValue(TintColorProperty);
        set => SetValue(TintColorProperty, value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Aspect Aspect { get => base.Aspect; set => base.Aspect = value; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public new bool IsAnimationPlaying { get => base.IsAnimationPlaying; set => base.IsAnimationPlaying = value; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public new bool IsLoading { get => base.IsLoading; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public new bool IsOpaque { get => base.IsOpaque; set => base.IsOpaque = value; }
}