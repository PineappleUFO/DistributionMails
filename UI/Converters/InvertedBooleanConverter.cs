using System.Globalization;

namespace UI.Converters;

public class InvertedBooleanConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b) return !b;
        return default;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return default;
    }
}