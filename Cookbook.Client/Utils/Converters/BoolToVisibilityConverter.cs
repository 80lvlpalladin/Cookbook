using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Cookbook.Client.Utils.Converters
{
    /// <summary>Converts bool to Visibility based on parameter</summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool input = (bool)value;
            return parameter switch
            {
                "CollapsedIfFalse" => input ? Visibility.Visible : Visibility.Collapsed,
                "HiddenIfFalse" => input ? Visibility.Visible : Visibility.Hidden,
                "HiddenIfTrue" => input ? Visibility.Hidden : Visibility.Visible,
                _ => throw new Exception("This converter requires a parameter")
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
