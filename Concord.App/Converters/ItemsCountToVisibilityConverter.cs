using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Concord.App.Converters
{
    public class ItemsCountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var intValue = (int) value;

                return intValue == 0 ? Visibility.Visible : Visibility.Hidden;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}