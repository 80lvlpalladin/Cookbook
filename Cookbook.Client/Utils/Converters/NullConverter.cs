using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Cookbook.Client.Utils.Converters
{
    /// <summary>Versatile null to visibility/bool converter</summary>
    public class NullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isValueNull = Equals(value, null);
            return parameter switch
            {
                "TrueIfNull" => isValueNull,
                "FalseIfNull" => !isValueNull,
                "VisibleIfNull" => isValueNull ? Visibility.Visible : Visibility.Collapsed,
                "CollapsedIfNull" => isValueNull ? Visibility.Collapsed : Visibility.Visible,
                "HiddenIfNull" => isValueNull ? Visibility.Hidden : Visibility.Visible,
                _ => throw new Exception("This converter requires a parameter")
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
