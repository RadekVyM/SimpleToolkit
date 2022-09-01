using IImage = Microsoft.Maui.IImage;

namespace SimpleToolkit.Core
{
    public interface IIcon : IImage
    {
        Color TintColor { get; set; }
    }
}
