using System.Globalization;

namespace Sample.SimpleShellSharpnadoTabs.Converter;

public class TabIconConverter : IMultiValueConverter
{
    public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        // Tabs do not support color filters on Windows
#if !WINDOWS
        return values[0];
#else
        if (values.Length < 4)
            return null;

        if (values[0] is not FontImageSource imageSource)
            return values[0];

        if (values[1] is Color defaultColor && values[2] is Color selectedColor && values[3] is bool isSelected)
            imageSource.Color = isSelected ? selectedColor : defaultColor;

        return imageSource;
#endif
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
