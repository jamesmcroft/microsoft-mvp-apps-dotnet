namespace MVP.App.Converters
{
    using System;

    using Windows.UI.Xaml.Data;

    using WinUX.Common;

    public class IntToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null ? 0d : ParseHelper.SafeParseDouble(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value == null ? 0 : ParseHelper.SafeParseInt(value);
        }
    }
}