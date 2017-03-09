using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace MVP.App.Converters
{
    /// <summary>
    /// Converts RadChart categorical axis labels and RadLegend labels to readable values
    /// Example: Month numbers are converted to month names. Unmatched cases will pass through and be displayed as-is
    /// </summary>
    public class CategoricalLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return "";

            // TODO - Confirm this works in other cultures, James can test en-gb
            // For "Week" charts (only passes because it's the only parse-able label)
            DateTime displayDate;
            if (DateTime.TryParse(value.ToString(), out displayDate))
            {
                return $"Week of {displayDate:d}";
            }

            // For "Month" charts
            switch (value.ToString())
            {
                case "1":
                    return "Jan";
                case "2":
                    return "Feb";
                case "3":
                    return "Mar";
                case "4":
                    return "Apr";
                case "5":
                    return "May";
                case "6":
                    return "Jun";
                case "7":
                    return "Jul";
                case "8":
                    return "Aug";
                case "9":
                    return "Sep";
                case "10":
                    return "Oct";
                case "11":
                    return "Nov";
                case "12":
                    return "Dec";
                default:
                    // All other categorical types will pass through (e.g. Contribution Types)
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
