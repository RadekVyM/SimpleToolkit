using IImage = Microsoft.Maui.IImage;

namespace SimpleToolkit.Core
{
    /// <summary>
    /// Control displaying a tinted image.
    /// </summary>
    public interface IIcon : IImage
    {
        /// <summary>
        /// Tint color of an image.
        /// </summary>
        Color TintColor { get; set; }
    }
}
