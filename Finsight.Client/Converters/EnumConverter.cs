using System.Globalization;
using System.Windows.Data;
using Finsight.Client.Utils;

namespace Finsight.Client.Converters
{
    public class EnumConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
                return EnumLocalizationProvider.GetLocalizedString(enumValue);
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
