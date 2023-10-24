using Microsoft.Maui.Controls.Shapes;

namespace SimpleToolkit.Core.MarkupExtensions;

public class PathGeometryExtension : IMarkupExtension<Geometry>
{
    /// <summary>
    /// String representation of a path.
    /// </summary>
    public string Path { get; set; }

    public Geometry ProvideValue(IServiceProvider serviceProvider)
    {
        var pathGeometryConverter = new PathGeometryConverter();
        var path = pathGeometryConverter.ConvertFromInvariantString(Path) as Geometry;

        return path;
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return (this as IMarkupExtension<Geometry>).ProvideValue(serviceProvider);
    }
}