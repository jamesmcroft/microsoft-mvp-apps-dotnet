namespace MVP.App.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Android.App;
    using Android.OS;
    using Android.Views;

    using GalaSoft.MvvmLight.Command;

    using MVP.Api;
    using MVP.App.Common;
    using MVP.App.Common.Networking;
    using MVP.App.Services.Initialization;
    using MVP.App.Services.MvpApi.DataContainers;

    public class InitializingActivityViewModel : BaseActivityViewModel
    {
        private readonly IAppInitializer initializer;

        private NetworkStatusManager networkStatusManager;

        private ApiClient apiClient;

        private IProfileDataContainer profileData;

        private ViewStates loadingState;

        private string loadingProgress;

        private ViewStates loadedState;

        public InitializingActivityViewModel(
            IAppInitializer initializer,
            ApiClient apiClient,
            IProfileDataContainer profileData)
        {
            this.initializer = initializer;
            this.apiClient = apiClient;
            this.profileData = profileData;

            this.SigninCommand = new RelayCommand(async () => await this.SignInAsync());

            this.MessengerInstance.Register<AppInitializerMessage>(
                this,
                msg =>
                    {
                        this.LoadingProgress = !string.IsNullOrWhiteSpace(msg?.Message) ? msg.Message : "Loading...";
                    });
        }

        /// <summary>
        /// Gets the command for signing into an MVP account using Live ID.
        /// </summary>
        public ICommand SigninCommand { get; }

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

            this.networkStatusManager = new NetworkStatusManager(Application.Context);

            var initializeSuccess = await this.initializer.InitializeAsync();
            if (initializeSuccess)
            {
                this.NavigateToHome();
            }

            this.LoadingState = ViewStates.Invisible;
        }


        private async Task SignInAsync()
        {
            if (!this.networkStatusManager.IsConnected())
            {
                return;
            }

            this.LoadingProgress = "Signing in...";
            this.LoadingState = ViewStates.Visible;

            var authMsg = await this.initializer.AuthenticateAsync();

            if (authMsg.IsSuccess)
            {
                var initialized = await this.initializer.InitializeAsync();
                if (initialized)
                {
                    this.NavigateToHome();
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(authMsg.ErrorMessage))
                {
                    // ToDo - Android, show auth error message.
                }
            }

            this.LoadingState = ViewStates.Invisible;
        }

        private void NavigateToHome()
        {
            // ToDo - Android, navigate to the home page.
        }
    }
}