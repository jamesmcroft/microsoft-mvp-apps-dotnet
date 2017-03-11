namespace MVP.App.ViewModels
{
    using Android.OS;
    using Android.Views;

    using MVP.Api;
    using MVP.App.Common;
    using MVP.App.Services.Initialization;
    using MVP.App.Services.MvpApi.DataContainers;

    public class InitializingActivityViewModel : BaseActivityViewModel
    {
        private readonly IAppInitializer initializer;

        private ApiClient apiClient;

        private IProfileDataContainer profileData;

        private ViewStates loadingState;

        private string loadingProgress;

        private ViewStates loadedState;

        public InitializingActivityViewModel(IAppInitializer initializer, ApiClient apiClient, IProfileDataContainer profileData)
        {
            this.initializer = initializer;
            this.apiClient = apiClient;
            this.profileData = profileData;

            this.MessengerInstance.Register<AppInitializerMessage>(
                this,
                msg =>
                    {
                        this.LoadingProgress = !string.IsNullOrWhiteSpace(msg?.Message) ? msg.Message : "Loading...";
                    });
        }

        /// <summary>
        /// Gets or sets a value indicating whether the view is loading.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the progress message for loading.
        /// </summary>
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

        public override async void OnActivityCreated(Bundle bundle)
        {
            this.LoadingState = ViewStates.Visible;

            var initializeSuccess = await this.initializer.InitializeAsync();
            if (initializeSuccess)
            {
                this.NavigateToHome();
            }

            this.LoadingState = ViewStates.Invisible;
        }

        private void NavigateToHome()
        {
            // Navigate to the home page with the current profile from profileData.Profile.
        }
    }
}