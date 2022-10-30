using NZ_Auto8.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace NZ_Auto8.Converters
{
    public class IntToMultiTaskMode : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (MultiTaskMode)value;
        }
    }
}
