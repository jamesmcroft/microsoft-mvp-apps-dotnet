namespace MVP.App.Converters
{
    using System;

    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media;

    public class BackgroundToForegroundBrushConverter : IValueConverter
    {
        public SolidColorBrush LightForegroundBrush { get; set; }

        public SolidColorBrush DarkForegroundBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var color = value as Color? ?? Colors.Transparent;
            if (color == Colors.Transparent)
            {
                var solidColorBrush = value as SolidColorBrush;
                if (solidColorBrush == null)
                {
                    return this.DarkForegroundBrush;
                }

                color = solidColorBrush.Color;
            }

            var brightness = color.PerceivedBrightness();

            return brightness > 130 ? this.DarkForegroundBrush : this.LightForegroundBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}