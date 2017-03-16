namespace MVP.App.Converters
{
    using System;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    using WinUX.Common;

    public class IntToVisibilityConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty SupportsZeroProperty =
            DependencyProperty.Register(
                nameof(SupportsZero),
                typeof(bool),
                typeof(IntToVisibilityConverter),
                new PropertyMetadata(false));

        public bool SupportsZero
        {
            get
            {
                return (bool)this.GetValue(SupportsZeroProperty);
            }

            set
            {
                this.SetValue(SupportsZeroProperty, value);
            }
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            var val = ParseHelper.SafeParseInt(value);
            return this.SupportsZero ? Visibility.Visible : (val == 0 ? Visibility.Collapsed : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}