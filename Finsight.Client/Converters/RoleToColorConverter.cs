using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Finsight.Contract.Enum;

namespace Finsight.Client.Converters
{
    public class RoleToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                UserRole.Administrator => new SolidColorBrush(Color.FromRgb(255, 99, 71)),    // Tomato
                UserRole.Manager => new SolidColorBrush(Color.FromRgb(255, 165, 0)),         // Orange
                UserRole.Support => new SolidColorBrush(Color.FromRgb(60, 179, 113)),      // MediumSeaGreen
                _ => Brushes.Gray
            };
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}
