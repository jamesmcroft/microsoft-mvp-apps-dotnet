using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MVP.Api;
using MVP.Api.Models;
using MVP.App.Events;
using MVP.App.Models;
using MVP.App.Services.MvpApi;
using MVP.App.Services.MvpApi.DataContainers;
using WinUX;
using WinUX.Diagnostics.Tracing;
using WinUX.Mvvm.Input;
using WinUX.MvvmLight.Xaml.Views;
using WinUX.Networking;

namespace MVP.App.ViewModels
{
    public class InsightsPageViewModel : PageBaseViewModel
    {
        #region Fields

        private readonly ApiClient apiClient;
        private readonly IContributionSubmissionService contributionService;
        private readonly IProfileDataContainer profileData;
        private int contributionsToRetrieve = 20;
        private string selectedGroupByType = "Contribution Area";
        private bool isConfigurationPanelVisible = true;

        #endregion

        public InsightsPageViewModel(ApiClient apiClient,
            IContributionSubmissionService contributionService,
            IProfileDataContainer profileData)
        {
            this.apiClient = apiClient;
            this.contributionService = contributionService;
            this.profileData = profileData;

            Contributions = new ObservableCollection<Contribution>();
            GroupedContributionsData = new ObservableCollection<ChartDataItemViewModel>();
            GroupByTypes = new ObservableCollection<string> { "Contribution Type", "Technology Name" };
            SelectedGroupByType = GroupByTypes.FirstOrDefault();

            MessengerInstance.Register<ProfileUpdatedMessage>(
                this,
                args =>
                {
                    if (args != null)
                    {
                        OnProfileUpdated(args.Profile);
                    }
                });

            MessengerInstance.Register<RefreshDataMessage>(this, RefreshProfileData);
        }

        #region Properties

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
            get { return selectedGroupByType; }
            set { Set(() => SelectedGroupByType, ref selectedGroupByType, value); }
        }

        /// <summary>
        /// Sets the maximum number of contributions to retrieve for data visualization
        /// </summary>
        public int ContributionsToRetrieve
        {
            get { return contributionsToRetrieve; }
            set { Set(() => ContributionsToRetrieve, ref contributionsToRetrieve, value); }
        }

        /// <summary>
        /// Toggles the visibility of the data filtering and fetch configuration panel
        /// </summary>
        public bool IsConfigurationPanelVisible
        {
            get { return isConfigurationPanelVisible; }
            set { Set(() => IsConfigurationPanelVisible, ref isConfigurationPanelVisible, value); }
        }

        #endregion

        #region Methods

        private async void RefreshProfileData(RefreshDataMessage obj)
        {
            if (NetworkStatusManager.Current.IsConnected())
            {
                if (obj.Mode == RefreshDataMode.All || obj.Mode == RefreshDataMode.Profile)
                {
                    try
                    {
                        var newProfile = await apiClient.GetMyProfileAsync();
                        if (newProfile != null)
                        {
                            await profileData.SetProfileAsync(newProfile);
                        }
                    }
                    catch (HttpRequestException hre) when (hre.Message.Contains("401"))
                    {
                        // Show dialog, unauthorized user detect.
                        Application.Current.Exit();
                    }
                }
                else if (obj.Mode == RefreshDataMode.All || obj.Mode == RefreshDataMode.Contributions)
                {
                    await UpdateContributionsAsync();
                }
            }

            MessengerInstance.Send(new UpdateBusyIndicatorMessage(false, string.Empty));
        }

        private async void OnProfileUpdated(MVPProfile prof)
        {
            if (prof != null)
            {
                await UpdateContributionsAsync();
            }
        }

        private async Task UpdateContributionsAsync()
        {
            if (!NetworkStatusManager.Current.IsConnected())
            {
                return;
            }

            try
            {
                var periodContributions = await apiClient.GetContributionsAsync(0, ContributionsToRetrieve);
                if (periodContributions?.Items != null)
                {
                    Contributions.Clear();
                    Contributions.AddRange(periodContributions.Items);
                }
            }
            catch (HttpRequestException hre) when (hre.Message.Contains("401"))
            {
                // Show dialog, unauthorized user detected.
                Application.Current.Exit();
            }
        }

        private async Task UpdateChartDataAsync()
        {
            try
            {
                MessengerInstance.Send(new UpdateBusyIndicatorMessage(true, "refreshing charts..."));

                if (ContributionsToRetrieve != Contributions.Count)
                {
                    await UpdateContributionsAsync();
                }

                IEnumerable<ChartDataItemViewModel> groupedContributions = null;

                if (selectedGroupByType == GroupByTypes[0]) // "Contribution Type"
                {
                    groupedContributions = Contributions.GroupBy(c => c.TypeName).Select(g => new ChartDataItemViewModel
                    {
                        CategoryName = g.Key.ToString(),
                        CategoryValue = g.Count()
                    });
                }
                else if (selectedGroupByType == GroupByTypes[1]) // "Technology Name"
                {
                    groupedContributions = Contributions.GroupBy(c => c.Technology.Name).Select(g => new ChartDataItemViewModel
                    {
                        CategoryName = g.Key.ToString(),
                        CategoryValue = g.Count()
                    });
                }

                if (groupedContributions != null)
                {
                    GroupedContributionsData.Clear();
                    GroupedContributionsData.AddRange(groupedContributions);
                }
            }
            catch (Exception ex)
            {
                EventLogger.Current.WriteError(ex.ToString());
            }
            finally
            {
                MessengerInstance.Send(new UpdateBusyIndicatorMessage(false, string.Empty));
                IsConfigurationPanelVisible = false;
            }
        }

        #endregion

        #region UI event handlers
        
        public async void UpdateChartsButton_OnClick(object sender, RoutedEventArgs e)
        {
            await UpdateChartDataAsync();
        }

        #endregion

        #region Navigation and lifecycle

        /// <inheritdoc />
        public override async void OnPageNavigatedTo(NavigationEventArgs args)
        {
            await UpdateContributionsAsync();
        }

        /// <inheritdoc />
        public override void OnPageNavigatedFrom(NavigationEventArgs args)
        {
        }

        /// <inheritdoc />
        public override void OnPageNavigatingFrom(NavigatingCancelEventArgs args)
        {
        }

        #endregion
    }
}
