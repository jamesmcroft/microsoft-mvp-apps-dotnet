namespace MVP.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using MVP.Api;
    using MVP.Api.Models;
    using MVP.App.Events;
    using MVP.App.Models;
    using MVP.App.Services.MvpApi.DataContainers;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Navigation;

    using WinUX;
    using WinUX.Diagnostics.Tracing;
    using WinUX.MvvmLight.Xaml.Views;
    using WinUX.Networking;

    public class InsightsPageViewModel : PageBaseViewModel
    {
        #region Fields

        private readonly ApiClient apiClient;

        private readonly IProfileDataContainer profileData;

        private int contributionsToRetrieve = 20;

        private string selectedGroupByType = "Contribution Area";

        private bool isConfigurationPanelVisible = true;

        #endregion

        public InsightsPageViewModel(
            ApiClient apiClient,
            IProfileDataContainer profileData)
        {
            this.apiClient = apiClient;
            this.profileData = profileData;

            this.Contributions = new ObservableCollection<Contribution>();
            this.GroupedContributionsData = new ObservableCollection<ChartDataItemViewModel>();
            this.GroupByTypes = new ObservableCollection<string> { "Contribution Type", "Technology Name" };
            this.SelectedGroupByType = this.GroupByTypes.FirstOrDefault();

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
                        var newProfile = await this.apiClient.GetMyProfileAsync();
                        if (newProfile != null)
                        {
                            await this.profileData.SetProfileAsync(newProfile);
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
                    await this.UpdateContributionsAsync();
                }
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
            if (!NetworkStatusManager.Current.IsConnected())
            {
                return;
            }

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
                // Show dialog, unauthorized user detected.
                Application.Current.Exit();
            }
        }

        private async Task UpdateChartDataAsync()
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
                    groupedContributions = this.Contributions.GroupBy(c => c.TypeName)
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
                    groupedContributions = this.Contributions.GroupBy(c => c.Technology.Name)
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
                EventLogger.Current.WriteError(ex.ToString());
            }
            finally
            {
                this.MessengerInstance.Send(new UpdateBusyIndicatorMessage(false));
                this.IsConfigurationPanelVisible = false;
            }
        }

        #endregion

        #region UI event handlers

        public async void UpdateChartsButton_OnClick(object sender, RoutedEventArgs e)
        {
            await this.UpdateChartDataAsync();
        }

        #endregion

        #region Navigation and lifecycle

        /// <inheritdoc />
        public override async void OnPageNavigatedTo(NavigationEventArgs args)
        {
            if (args.NavigationMode == NavigationMode.New)
            {
                await this.UpdateChartDataAsync();
            }
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