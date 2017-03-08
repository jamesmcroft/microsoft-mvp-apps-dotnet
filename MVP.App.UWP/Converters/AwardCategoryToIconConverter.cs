namespace MVP.App.Converters
{
    using System;
    using System.Collections.Generic;

    using GalaSoft.MvvmLight.Ioc;

    using MVP.Api.Models;
    using MVP.App.Services.MvpApi.DataContainers;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media.Imaging;

    public class AwardCategoryToIconConverter : IValueConverter
    {
        private IContributionAreaDataContainer areaContainer;

        public IContributionAreaDataContainer AreaContainer
            => this.areaContainer ?? (this.areaContainer = SimpleIoc.Default.GetInstance<IContributionAreaDataContainer>());

        public IEnumerable<AwardContribution> Areas => this.AreaContainer.GetAllAreas();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var technology = value as ContributionTechnology;

            string iconName = "MVP";

            if (this.AreaContainer != null && technology?.Id != null)
            {
                var area = this.Areas.GetActivityTechnologyById(technology.Id.Value);
                if (area != null)
                {
                    if (area.AwardName.Equals("Access"))
                    {
                        iconName = "Access";
                    }
                    else if (area.AwardName.Equals("Business Solutions"))
                    {
                        iconName = "BusinessSolutions";
                    }
                    else if (area.AwardName.Equals("Cloud and Datacenter Management"))
                    {
                        iconName = "CloudDataManagement";
                    }
                    else if (area.AwardName.Equals("Data Platform"))
                    {
                        iconName = "DataPlatform";
                    }
                    else if (area.AwardName.Equals("Enterprise Mobility"))
                    {
                        iconName = "EnterpriseMobility";
                    }
                    else if (area.AwardName.Equals("Excel"))
                    {
                        iconName = "Excel";
                    }
                    else if (area.AwardName.Equals("Microsoft Azure"))
                    {
                        iconName = "Azure";
                    }
                    else if (area.AwardName.Equals("Office Development"))
                    {
                        iconName = "OfficeDev";
                    }
                    else if (area.AwardName.Equals("Office Servers and Services"))
                    {
                        iconName = "OfficeServer";
                    }
                    else if (area.AwardName.Equals("OneNote"))
                    {
                        iconName = "OneNote";
                    }
                    else if (area.AwardName.Equals("Outlook"))
                    {
                        iconName = "Outlook";
                    }
                    else if (area.AwardName.Equals("PowerPoint"))
                    {
                        iconName = "PowerPoint";
                    }
                    else if (area.AwardName.Equals("Visio"))
                    {
                        iconName = "Visio";
                    }
                    else if (area.AwardName.Equals("Visual Studio and Development Technologies"))
                    {
                        iconName = "VisualStudio";
                    }
                    else if (area.AwardName.Equals("Windows and Devices for IT"))
                    {
                        iconName = "WindowsDevices";
                    }
                    else if (area.AwardName.Equals("Windows Development"))
                    {
                        iconName = "WindowsDevelopment";
                    }
                    else if (area.AwardName.Equals("Word"))
                    {
                        iconName = "Word";
                    }
                }
            }

            return new BitmapImage(new Uri($"ms-appx:///Assets/Icons/{iconName}.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}