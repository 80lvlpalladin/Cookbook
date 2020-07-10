using System;
using System.Globalization;
using System.Windows.Data;

namespace Cookbook.Client.Utils.Converters
{
    /// <summary>Converts bool to !bool</summary>
    public class BoolNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => 
            throw new NotImplementedException();
    }
}
