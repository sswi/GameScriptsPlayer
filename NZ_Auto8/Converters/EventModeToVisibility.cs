using NZ_Auto8.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NZ_Auto8.Converters
{
    public class EventModeToVisibility : IValueConverter
    {
       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            return (EventMode)value == (EventMode)System.Convert.ToInt32( parameter) ? Visibility.Visible: Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null!;
        }
    }
}
