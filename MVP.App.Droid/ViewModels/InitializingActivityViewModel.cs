namespace MVP.App.ViewModels
{
    using Android.OS;

    using MVP.Api;
    using MVP.App.Common;
    using MVP.App.Data;
    using MVP.App.Services.Data;
    using MVP.App.Services.Initialization;

    public class InitializingActivityViewModel : BaseActivityViewModel
    {
        private readonly IAppInitializer initializer;

        private ApiClient apiClient;

        private IProfileDataContainer profileData;

        private bool isLoading;

        private string loadingProgress;

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
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }
            set
            {
                this.Set(() => this.IsLoading, ref this.isLoading, value);
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
            this.IsLoading = true;

            var initializeSuccess = await this.initializer.InitializeAsync();
            if (initializeSuccess)
            {
                this.NavigateToHome();
            }

            this.IsLoading = false;
        }

        private void NavigateToHome()
        {
            // Navigate to the home page with the current profile from profileData.Profile.
        }
    }
}