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

    /// <summary>
    /// Defines a value converter that checks an MVP award category and returns an appropriate <see cref="SolidColorBrush"/>.
    /// </summary>
    public class AwardCategoryToColorBrushConverter : IValueConverter
    {
        private IContributionAreaDataContainer areaContainer;

        /// <summary>
        /// Gets the MVP contribution area container.
        /// </summary>
        public IContributionAreaDataContainer AreaContainer
            => this.areaContainer ?? (this.areaContainer = SimpleIoc.Default.GetInstance<IContributionAreaDataContainer>());

        /// <summary>
        /// Gets the MVP award contribution areas.
        /// </summary>
        public IEnumerable<AwardContribution> Areas => this.AreaContainer.GetAllAreas();

        /// <summary>
        /// Gets or sets the default brush to use.
        /// </summary>
        public SolidColorBrush DefaultBrush { get; set; }

        /// <summary>
        /// Gets or sets the brush to use for Access contributions.
        /// </summary>
        public SolidColorBrush AccessBrush { get; set; }

        /// <summary>
        /// Gets or sets the brush to use for Excel contributions.
        /// </summary>
        public SolidColorBrush ExcelBrush { get; set; }

        /// <summary>
        /// Gets or sets the brush to use for Office contributions.
        /// </summary>
        public SolidColorBrush OfficeBrush { get; set; }

        /// <summary>
        /// Gets or sets the brush to use for OneNote contributions.
        /// </summary>
        public SolidColorBrush OneNoteBrush { get; set; }

        /// <summary>
        /// Gets or sets the brush to use for Outlook contributions.
        /// </summary>
        public SolidColorBrush OutlookBrush { get; set; }

        /// <summary>
        /// Gets or sets the brush to use for PowerPoint contributions.
        /// </summary>
        public SolidColorBrush PowerPointBrush { get; set; }

        /// <summary>
        /// Gets or sets the brush to use for Visio contributions.
        /// </summary>
        public SolidColorBrush VisioBrush { get; set; }

        /// <summary>
        /// Gets or sets the brush to use for Visual Studio contributions.
        /// </summary>
        public SolidColorBrush VisualStudioBrush { get; set; }

        /// <summary>
        /// Gets or sets the brush to use for Word contributions.
        /// </summary>
        public SolidColorBrush WordBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ContributionTechnology technology = value as ContributionTechnology;

            if (this.AreaContainer != null && technology?.Id != null)
            {
                ActivityTechnology area = this.Areas.GetActivityTechnologyById(technology.Id.Value);
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