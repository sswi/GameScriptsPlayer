using NZ_Auto8.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NZ_Auto8.Converters
{
    public class MouseModeToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
          return (MouseMode)value== MouseMode.Move? Visibility.Visible: Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return MouseMode.RightClick;
        }
    }
}
