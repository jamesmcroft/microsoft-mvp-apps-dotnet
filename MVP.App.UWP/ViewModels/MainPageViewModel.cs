namespace MVP.App.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using MVP.Api;
    using MVP.Api.Models;
    using MVP.App.Events;
    using MVP.App.Models;
    using MVP.App.Views;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.UI.Xaml.Navigation;

    using MVP.App.Services.Data;

    using WinUX;
    using WinUX.MvvmLight.Xaml.Views;
    using WinUX.Networking;

    /// <summary>
    /// Defines the view-model for the <see cref="MainPage"/>
    /// </summary>
    public class MainPageViewModel : PageBaseViewModel
    {
        private MVPProfile profile;

        private readonly ApiClient apiClient;

        private readonly IProfileDataContainer profileData;

        private BitmapSource profileImage;

        private bool isRecentActivitiesVisible;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        /// <param name="apiClient">
        /// The MVP API client.
        /// </param>
        /// <param name="profileData">
        /// The application's data.
        /// </param>
        public MainPageViewModel(ApiClient apiClient, IProfileDataContainer profileData)
        {
            this.apiClient = apiClient;
            this.profileData = profileData;

            this.RecentContributions = new ObservableCollection<Contribution>();
            this.ContributionFlyoutViewModel = new ContributionFlyoutViewModel();

            this.ActivityClickedCommand = new RelayCommand<Contribution>(c => this.ContributionFlyoutViewModel.Show(c));

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

        /// <summary>
        /// Gets the custom fly-out view model for the contributions.
        /// </summary>
        public ContributionFlyoutViewModel ContributionFlyoutViewModel { get; }

        /// <summary>
        /// Gets the recent activities of the current MVP profile.
        /// </summary>
        public ObservableCollection<Contribution> RecentContributions { get; }

        /// <summary>
        /// Gets or sets the current MVP profile.
        /// </summary>
        public MVPProfile Profile
        {
            get
            {
                return this.profile;
            }

            set
            {
                this.Set(() => this.Profile, ref this.profile, value);
            }
        }

        /// <summary>
        /// Gets or sets the current MVP profile image.
        /// </summary>
        public BitmapSource ProfileImage
        {
            get
            {
                return this.profileImage;
            }

            set
            {
                this.Set(() => this.ProfileImage, ref this.profileImage, value);
            }
        }

        /// <inheritdoc />
        public override void OnPageNavigatedTo(NavigationEventArgs args)
        {
            var mvpProfile = args.Parameter as MVPProfile;
            this.OnProfileUpdated(mvpProfile);
        }

        /// <inheritdoc />
        public override void OnPageNavigatedFrom(NavigationEventArgs args)
        {
        }

        /// <inheritdoc />
        public override void OnPageNavigatingFrom(NavigatingCancelEventArgs args)
        {
        }

        private async void RefreshProfileData(RefreshDataMessage obj)
        {
            this.ContributionFlyoutViewModel.Close();

            if (NetworkStatusManager.Current.IsConnected())
            {
                if (obj != null && (obj.Mode == RefreshDataMode.All || obj.Mode == RefreshDataMode.Profile))
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
            }

            this.MessengerInstance.Send(new RefreshDataCompleteMessage(true));
        }

        private void UpdateSectionVisibility()
        {
            this.IsRecentActivitiesVisible = this.RecentContributions != null && this.RecentContributions.Any();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the recent activities section is visible.
        /// </summary>
        public bool IsRecentActivitiesVisible
        {
            get
            {
                return this.isRecentActivitiesVisible;
            }

            set
            {
                this.Set(() => this.IsRecentActivitiesVisible, ref this.isRecentActivitiesVisible, value);
            }
        }

        public ICommand ActivityClickedCommand { get; }

        private async void OnProfileUpdated(MVPProfile profile)
        {
            if (profile != null)
            {
                this.Profile = profile;
                await this.UpdateProfilePicAsync();
                await this.UpdateRecentContributionsAsync();
            }

            this.UpdateSectionVisibility();
        }

        private async Task UpdateRecentContributionsAsync()
        {
            if (!NetworkStatusManager.Current.IsConnected())
            {
                return;
            }

            try
            {
                var recentContributions = await this.apiClient.GetContributionsAsync(0, 10);
                if (recentContributions?.Items != null)
                {
                    this.RecentContributions.Clear();
                    this.RecentContributions.AddRange(recentContributions.Items);
                }
            }
            catch (HttpRequestException hre) when (hre.Message.Contains("401"))
            {
                // Show dialog, unauthorized user detected.
                Application.Current.Exit();
            }
        }

        private async Task UpdateProfilePicAsync()
        {
            if (!string.IsNullOrWhiteSpace(this.profileData.ProfileImage))
            {
                await this.SetProfileImageSourceAsync(this.profileData.ProfileImage);
            }
            else
            {
                if (!NetworkStatusManager.Current.IsConnected())
                {
                    return;
                }

                try
                {
                    var image = await this.apiClient.GetMyProfileImageAsync();
                    await this.SetProfileImageSourceAsync(image);
                    await this.profileData.SetProfileImageAsync(image);
                }
                catch (HttpRequestException hre) when (hre.Message.Contains("401"))
                {
                    // Show dialog, unauthorized user detected.
                    Application.Current.Exit();
                }
            }
        }

        private async Task SetProfileImageSourceAsync(string base64String)
        {
            this.ProfileImage = await base64String.ToImageSourceAsync();
        }
    }
}