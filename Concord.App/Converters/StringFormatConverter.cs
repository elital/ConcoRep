using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Concord.App.Converters
{
    public class StringFormatConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!values.Any())
                    return string.Empty;

                var formatString = (string) values[0];

                if (values.Length == 1)
                    return formatString;

                var parameters = new object[values.Length - 1];

                for (var i = 1; i < values.Length; i++)
                    parameters[i - 1] = values[i];

                return string.Format(formatString, parameters);
            }
            catch
            {
                return string.Empty;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}