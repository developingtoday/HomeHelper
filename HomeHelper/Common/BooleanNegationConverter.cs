using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace HomeHelper.Common
{
    /// <summary>
    /// Value converter that translates true to false and vice versa.
    /// </summary>
    public sealed class BooleanNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool && (bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool && (bool)value);
        }
    }

    public class ItemCountToEmptyVisibilityConverterButtonsAdd:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var itemCount = value as int?;
            if (itemCount.HasValue)
            {
                return itemCount.Value > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ItemCountToEmptyVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var itemCount = value as int?;
            if (itemCount.HasValue)
            {
                return itemCount.Value > 0 ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
