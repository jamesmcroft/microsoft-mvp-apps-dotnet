namespace MVP.App.Converters
{
    using System;
    using System.Collections.Generic;

    using GalaSoft.MvvmLight.Ioc;

    using MVP.Api.Models;
    using MVP.App.Services.MvpApi.DataContainers;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media;

    public class AwardCategoryToColorBrushConverter : IValueConverter
    {
        private IContributionAreaContainer areaContainer;

        public IContributionAreaContainer AreaContainer
            => this.areaContainer ?? (this.areaContainer = SimpleIoc.Default.GetInstance<IContributionAreaContainer>());

        public IEnumerable<AwardContribution> Areas => this.AreaContainer.GetAllAreas();

        public SolidColorBrush DefaultBrush { get; set; }

        public SolidColorBrush AccessBrush { get; set; }

        public SolidColorBrush ExcelBrush { get; set; }

        public SolidColorBrush OfficeBrush { get; set; }

        public SolidColorBrush OneNoteBrush { get; set; }

        public SolidColorBrush OutlookBrush { get; set; }

        public SolidColorBrush PowerPointBrush { get; set; }

        public SolidColorBrush VisioBrush { get; set; }

        public SolidColorBrush VisualStudioBrush { get; set; }

        public SolidColorBrush WordBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var technology = value as ContributionTechnology;

            if (this.AreaContainer != null && technology?.Id != null)
            {
                var area = this.Areas.GetActivityTechnologyById(technology.Id.Value);
                if (area != null)
                {
                    if (area.AwardName.Equals("Access"))
                    {
                        return this.AccessBrush;
                    }

                    if (area.AwardName.Equals("Excel"))
                    {
                        return this.ExcelBrush;
                    }

                    if (area.AwardName.Equals("Office Development")
                        || area.AwardName.Equals("Office Servers and Services"))
                    {
                        return this.OfficeBrush;
                    }

                    if (area.AwardName.Equals("OneNote"))
                    {
                        return this.OneNoteBrush;
                    }

                    if (area.AwardName.Equals("Outlook"))
                    {
                        return this.OutlookBrush;
                    }

                    if (area.AwardName.Equals("PowerPoint"))
                    {
                        return this.PowerPointBrush;
                    }

                    if (area.AwardName.Equals("Visio"))
                    {
                        return this.VisioBrush;
                    }

                    if (area.AwardName.Equals("Visual Studio and Development Technologies"))
                    {
                        return this.VisualStudioBrush;
                    }

                    if (area.AwardName.Equals("Word"))
                    {
                        return this.WordBrush;
                    }
                }
            }

            return this.DefaultBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}