using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Android.OS;
using Android.Views;

using MVP.Api;
using MVP.Api.Models;
using MVP.App.Common;
using MVP.App.Common.Charting;
using MVP.App.Events;
using MVP.App.Services.MvpApi.DataContainers;

using WinUX;

using Exception = System.Exception;

namespace MVP.App.ViewModels
{
    public class InsightsActivityViewModel : BaseActivityViewModel
    {
        private readonly ApiClient apiClient;

        private readonly IProfileDataContainer profileData;

        private ViewStates loadingState;

        private string loadingProgress;

        private ViewStates loadedState;

        private int contributionsToRetrieve = 20;

        private string selectedGroupByType = "Contribution Area";

        private bool isConfigurationPanelVisible = true;

        private bool? isPieChartVisible;

        private bool? isBarChartVisible;

        public InsightsActivityViewModel(ApiClient apiClient, 
            IProfileDataContainer profileData)
        {
            this.apiClient = apiClient;
            this.profileData = profileData;

            this.Contributions = new ObservableCollection<Contribution>();
            this.GroupedContributionsData = new ObservableCollection<ChartDataItemViewModel>();
            this.GroupByTypes = new ObservableCollection<string> { "Contribution Type", "Technology Name", "Week", "Month", "Year" };
            this.SelectedGroupByType = this.GroupByTypes.FirstOrDefault();

            this.IsPieChartVisible = true;
            this.IsBarChartVisible = true;
            
            this.MessengerInstance.Register<ProfileUpdatedMessage>(
                this,
                args =>
                {
                    if (args != null)
                    {
                        this.OnProfileUpdated(args.Profile);
                    }
                });

            this.MessengerInstance.Register<RefreshDataMessage>(this, this.RefreshProfileData);
        }
        
        public ViewStates LoadingState
        {
            get
            {
                return this.loadingState;
            }
            set
            {
                this.Set(() => this.LoadingState, ref this.loadingState, value);
                this.LoadedState = value == ViewStates.Visible ? ViewStates.Invisible : ViewStates.Visible;
            }
        }

        public ViewStates LoadedState
        {
            get
            {
                return this.loadedState;
            }
            set
            {
                this.Set(() => this.LoadedState, ref this.loadedState, value);
            }
        }
        
        public string LoadingProgress
        {
            get
            {
                return this.loadingProgress;
            }
            set
            {
                this.Set(() => this.LoadingProgress, ref this.loadingProgress, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the pie chart control is visible.
        /// </summary>
        public bool? IsPieChartVisible
        {
            get
            {
                return this.isPieChartVisible;
            }
            set
            {
                this.Set(() => this.IsPieChartVisible, ref this.isPieChartVisible, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the bar chart control is visible.
        /// </summary>
        public bool? IsBarChartVisible
        {
            get
            {
                return this.isBarChartVisible;
            }
            set
            {
                this.Set(() => this.IsBarChartVisible, ref this.isBarChartVisible, value);
            }
        }

        /// <summary>
        /// Holds the activities of the current MVP profile within the selected range of ContributionsToRetrieve
        /// </summary>
        public ObservableCollection<Contribution> Contributions { get; }

        /// <summary>
        /// The binding source for the donut series chart
        /// </summary>
        public ObservableCollection<ChartDataItemViewModel> GroupedContributionsData { get; }

        /// <summary>
        /// Binding source for the "GroupBy" ComboBox
        /// </summary>
        public ObservableCollection<string> GroupByTypes { get; }

        /// <summary>
        /// Sets the category to group by and render in the charts for contribution data
        /// </summary>
        public string SelectedGroupByType
        {
            get
            {
                return this.selectedGroupByType;
            }

            set
            {
                this.Set(() => this.SelectedGroupByType, ref this.selectedGroupByType, value);
            }
        }

        /// <summary>
        /// Sets the maximum number of contributions to retrieve for data visualization
        /// </summary>
        public int ContributionsToRetrieve
        {
            get
            {
                return this.contributionsToRetrieve;
            }

            set
            {
                this.Set(() => this.ContributionsToRetrieve, ref this.contributionsToRetrieve, value);
            }
        }

        /// <summary>
        /// Toggles the visibility of the data filtering and fetch configuration panel
        /// </summary>
        public bool IsConfigurationPanelVisible
        {
            get
            {
                return this.isConfigurationPanelVisible;
            }

            set
            {
                this.Set(() => this.IsConfigurationPanelVisible, ref this.isConfigurationPanelVisible, value);
            }
        }

        public override async void OnActivityCreated(Bundle bundle)
        {
            await this.UpdateChartDataAsync();
        }

        private async void RefreshProfileData(RefreshDataMessage obj)
        {
            // TODO NetworkManager implementation
            //if (!NetworkStatusManager.Current.IsConnected())
            //{
            //    return;
            //}

            if (obj.Mode == RefreshDataMode.All || obj.Mode == RefreshDataMode.Profile)
            {
                try
                {
                    var newProfile = await this.apiClient.GetMyProfileAsync();
                    if (newProfile != null)
                    {
                        await this.profileData.SetProfileAsync(newProfile);
                    }
                }
                catch (HttpRequestException hre) when (hre.Message.Contains("401"))
                {
                    // TODO Exit app implementation
                    // Show dialog, unauthorized user detect.
                    //Application.Current.Exit();
                }
            }
            else if (obj.Mode == RefreshDataMode.All || obj.Mode == RefreshDataMode.Contributions)
            {
                await this.UpdateContributionsAsync();
            }

            this.MessengerInstance.Send(new UpdateBusyIndicatorMessage(false, string.Empty));
        }

        private async void OnProfileUpdated(MVPProfile prof)
        {
            if (prof != null)
            {
                await this.UpdateContributionsAsync();
            }
        }

        private async Task UpdateContributionsAsync()
        {
            // TODO NetworkManager implementation
            //if (!NetworkStatusManager.Current.IsConnected())
            //{
            //    return;
            //}

            try
            {
                var periodContributions = await this.apiClient.GetContributionsAsync(0, this.ContributionsToRetrieve);
                if (periodContributions?.Items != null)
                {
                    this.Contributions.Clear();
                    this.Contributions.AddRange(periodContributions.Items);
                }
            }
            catch (HttpRequestException hre) when (hre.Message.Contains("401"))
            {
                // TODO Exit app implementation
                // Show dialog, unauthorized user detected.
                //Application.Current.Exit();
            }
        }

        public async Task UpdateChartDataAsync()
        {
            try
            {
                this.MessengerInstance.Send(new UpdateBusyIndicatorMessage(true, "Updating charts..."));

                if (this.ContributionsToRetrieve != this.Contributions.Count)
                {
                    await this.UpdateContributionsAsync();
                }

                IEnumerable<ChartDataItemViewModel> groupedContributions = null;

                if (this.selectedGroupByType == this.GroupByTypes[0])
                {
                    // "Contribution Type"
                    groupedContributions =
                        this.Contributions.GroupBy(c => c.TypeName)
                            .Select(
                                g =>
                                    new ChartDataItemViewModel
                                    {
                                        CategoryName = g.Key.ToString(),
                                        CategoryValue = g.Count()
                                    });
                }
                else if (this.selectedGroupByType == this.GroupByTypes[1])
                {
                    // "Technology Name"
                    groupedContributions =
                        this.Contributions.GroupBy(c => c.Technology.Name)
                            .Select(
                                g =>
                                    new ChartDataItemViewModel
                                    {
                                        CategoryName = g.Key.ToString(),
                                        CategoryValue = g.Count()
                                    });
                }
                else if (this.selectedGroupByType == this.GroupByTypes[2])
                {
                    // "Week" 

                    // Only use the past year's activities)
                    var lastYear = this.Contributions.GroupBy(c => c.StartDate.Value.Year).LastOrDefault();

                    if (lastYear != null)
                    {
                        groupedContributions =
                            lastYear.GroupBy(c => c.StartDate.Value.Date.AddDays(-(int)c.StartDate.Value.Date.DayOfWeek))
                                .Select(
                                    g =>
                                        new ChartDataItemViewModel
                                        {
                                            CategoryName = g.Key.ToString(),
                                            CategoryValue = g.Count()
                                        });

                    }
                }
                else if (this.selectedGroupByType == this.GroupByTypes[3])
                {
                    // "Month"

                    // Only use the past year's activities)
                    var lastYear = this.Contributions.GroupBy(c => c.StartDate.Value.Year).LastOrDefault();

                    if (lastYear != null)
                    {
                        groupedContributions =
                            lastYear.GroupBy(c => c.StartDate.Value.Month)
                                .Select(
                                    g =>
                                        new ChartDataItemViewModel
                                        {
                                            CategoryName = g.Key.ToString(),
                                            CategoryValue = g.Count()
                                        });
                    }
                }
                else if (this.selectedGroupByType == this.GroupByTypes[4])
                {
                    // "Year"
                    groupedContributions =
                        this.Contributions.GroupBy(c => c.StartDate.Value.Year)
                            .Select(
                                g =>
                                    new ChartDataItemViewModel
                                    {
                                        CategoryName = g.Key.ToString(),
                                        CategoryValue = g.Count()
                                    });
                }

                if (groupedContributions != null)
                {
                    this.GroupedContributionsData.Clear();
                    this.GroupedContributionsData.AddRange(groupedContributions);
                }
            }
            catch (Exception ex)
            {
#if WINDOWS_UWP
                EventLogger.Current.WriteWarning(aex.ToString());
#elif ANDROID
                // ToDo - Android, log out exception.
#endif
            }
            finally
            {
                this.MessengerInstance.Send(new UpdateBusyIndicatorMessage(false));
                this.IsConfigurationPanelVisible = false;
            }
        }
        
        private void NavigateToHome()
        {
            // ToDo - Android, navigate to the home page.
        }
    }
}