using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FlightgearSimulator.Convertors
{
    class DashboardColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value.ToString();
            if (String.Equals(strValue, "ERR"))
            {
                return new SolidColorBrush(Colors.Red);
            }
            else
            {
                return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
