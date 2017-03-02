namespace MVP.App.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using MVP.App.Services.Initialization;
    using MVP.App.Services.MvpApi.DataContainers;
    using MVP.App.Views;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    using WinUX.Messaging.Dialogs;
    using WinUX.MvvmLight.Xaml.Views;
    using WinUX.Networking;

    /// <summary>
    /// Defines the view-model for the <see cref="InitializingPage"/>
    /// </summary>
    public class InitializingPageViewModel : PageBaseViewModel
    {
        private readonly IAppInitializer initializer;

        private string loadingProgress;

        private bool isLoading;

        private readonly IProfileDataContainer profileData;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializingPageViewModel"/> class.
        /// </summary>
        /// <param name="initializer">
        /// The application initializer.
        /// </param>
        /// <param name="profileData">
        /// The profile Data.
        /// </param>
        public InitializingPageViewModel(
            IAppInitializer initializer,
            IProfileDataContainer profileData)
        {
            this.initializer = initializer;
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

        /// <inheritdoc />
        public override async void OnPageNavigatedTo(NavigationEventArgs args)
        {
            this.IsLoading = true;

            var initializeSuccess = await this.initializer.InitializeAsync();
            if (initializeSuccess)
            {
                this.NavigateToHome();
            }

            this.IsLoading = false;
        }

        /// <inheritdoc />
        public override void OnPageNavigatedFrom(NavigationEventArgs args)
        {
        }

        /// <inheritdoc />
        public override void OnPageNavigatingFrom(NavigatingCancelEventArgs args)
        {
        }

        private async Task SignInAsync()
        {
            if (!NetworkStatusManager.Current.IsConnected())
            {
                return;
            }

            this.LoadingProgress = "Signing in...";
            this.IsLoading = true;

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
                    await MessageDialogManager.Current.ShowAsync("Sign in error", authMsg.ErrorMessage);
                }
            }

            this.IsLoading = false;
        }

        private void NavigateToHome()
        {
            // Get profile and pass onto main page.
            this.NavigationService.Navigate(typeof(MainPage), this.profileData.Profile);

            var rootFrame = Window.Current.Content as Frame;
            rootFrame?.Navigate(typeof(AppShellPage));
        }
    }
}