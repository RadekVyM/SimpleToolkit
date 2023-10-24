namespace SimpleToolkit.SimpleShell.Controls;

internal static class ColorExtensions
{
    public static Brush OffsetBrushColorValue(this Brush brush, float valueOffset)
    {
        if (brush is SolidColorBrush solidColorBrush)
        {
            return new SolidColorBrush(solidColorBrush.Color?.OffsetColorValue(valueOffset));
        }
        else if (brush is LinearGradientBrush linearGradientBrush)
        {
            return new LinearGradientBrush(linearGradientBrush.GradientStops.OffsetStopsCollectionColorValue(valueOffset), linearGradientBrush.StartPoint, linearGradientBrush.EndPoint);
        }
        else if (brush is RadialGradientBrush radialGradientBrush)
        {
            return new RadialGradientBrush(radialGradientBrush.GradientStops.OffsetStopsCollectionColorValue(valueOffset), radialGradientBrush.Center, radialGradientBrush.Radius);
        }

        return brush;
    }

    public static GradientStopCollection OffsetStopsCollectionColorValue(this GradientStopCollection stops, float valueOffset)
    {
        var newStops = new GradientStopCollection();

        foreach (var stop in stops)
        {
            stops.Add(new GradientStop(stop.Color?.OffsetColorValue(valueOffset), stop.Offset));
        }

        return newStops;
    }

    public static Color OffsetColorValue(this Color color, float valueOffset)
    {
        var (h, s, v) = color.ToHsv();

        v = v + valueOffset > 1f ? v - valueOffset : v + valueOffset;

        return Color.FromHsv(h, s, v);
    }

    public static (float h, float s, float v) ToHsv(this Microsoft.Maui.Graphics.Color color)
    {
        var h = color.GetHue();
        var s = color.GetSaturation();
        var l = color.GetLuminosity();

        var v = l + s * Math.Min(l, 1 - l);
        s = v == 0 ? 0 : 2 * (1 - (l / v));
        return (h, s, v);
    }
}