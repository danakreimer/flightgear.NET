﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FlightSimulator.Convertors
{
    class DashboardColorConverter : IValueConverter
    {
        // The function converts the color of the string "ERR" in the dashboard
        // to red, the rest is in black.
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
            return value.ToString();
        }
    }
}
