namespace MVP.App.Converters
{
    using System;

    using Windows.UI.Xaml.Data;

    using WinUX.Common;

    public class DateTimeToDateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return DateTimeOffset.MinValue;
            }

            DateTime dt = ParseHelper.SafeParseDateTime(value);
            DateTimeOffset dto = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            return dto;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return DateTime.MinValue;
            }

            DateTimeOffset dto = ParseHelper.SafeParseDateTimeOffset(value);

            return dto.Offset.Equals(TimeSpan.Zero)
                       ? dto.UtcDateTime
                       : (dto.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(dto.DateTime))
                              ? DateTime.SpecifyKind(dto.DateTime, DateTimeKind.Local)
                              : dto.DateTime);
        }
    }
}