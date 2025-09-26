using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Finsight.Client.Converters 
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSuccess)
            {
                return isSuccess ? Brushes.Green : Brushes.Red;
            }

            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
