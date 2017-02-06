namespace MVP.App.ViewModels
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using MVP.Api;
    using MVP.Api.Models;
    using MVP.App.Data;
    using MVP.App.Events;
    using MVP.App.Views;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.UI.Xaml.Navigation;

    using WinUX.MvvmLight.Xaml.Views;
    using WinUX.Networking;

    /// <summary>
    /// Defines the view-model for the <see cref="MainPage"/>
    /// </summary>
    public class MainPageViewModel : PageBaseViewModel
    {
        private MVPProfile profile;

        private readonly ApiClient apiClient;

        private IAppData data;

        private BitmapSource profileImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        /// <param name="apiClient">
        /// The MVP API client.
        /// </param>
        /// <param name="data">
        /// The application's data.
        /// </param>
        public MainPageViewModel(ApiClient apiClient, IAppData data)
        {
            if (apiClient == null)
            {
                throw new ArgumentNullException(nameof(apiClient), "The MVP API client cannot be null.");
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "The app data cannot be null.");
            }

            this.apiClient = apiClient;
            this.data = data;

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
            if (!NetworkStatusManager.Current.IsConnected())
            {
                return;
            }

            if (obj != null && (obj.Mode == RefreshDataMode.All || obj.Mode == RefreshDataMode.Profile))
            {
                try
                {
                    var newProfile = await this.apiClient.GetMyProfileAsync();
                    if (newProfile != null)
                    {
                        await this.data.UpdateProfileAsync(newProfile);
                    }
                }
                catch (HttpRequestException hre) when (hre.Message.Contains("401"))
                {
                    // Show dialog, unauthorized user detect.
                    Application.Current.Exit();
                }
            }
        }

        private async void OnProfileUpdated(MVPProfile profile)
        {
            if (profile != null)
            {
                this.Profile = profile;

                if (!string.IsNullOrWhiteSpace(this.data.CurrentProfileImage))
                {
                    await this.SetProfileImageSourceAsync(this.data.CurrentProfileImage);
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
                        await this.data.UpdateProfileImageAsync(image);
                    }
                    catch (HttpRequestException hre) when (hre.Message.Contains("401"))
                    {
                        // Show dialog, unauthorized user detected.
                        Application.Current.Exit();
                    }
                }
            }
        }

        private async Task SetProfileImageSourceAsync(string image)
        {
            this.ProfileImage = await image.ToImageSourceAsync();
        }
    }
}