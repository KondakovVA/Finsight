using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace Finsight.Client.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public const string Invert = "Invert";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
                throw new InvalidOperationException("Целевой тип должен быть Visibility.");

            bool? bValue = (bool?)value;

            if (parameter != null && parameter as string == Invert)
                bValue = !bValue;

            return bValue.HasValue && bValue.Value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
