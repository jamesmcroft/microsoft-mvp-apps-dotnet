namespace MVP.App.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using MVP.App.Common;
    using MVP.App.Events;
    using MVP.App.Services.Initialization;
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

        private ActivationArgs activationArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializingPageViewModel"/> class.
        /// </summary>
        /// <param name="initializer">
        /// The application initializer.
        /// </param>
        public InitializingPageViewModel(IAppInitializer initializer)
        {
            this.initializer = initializer;

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
            get => this.isLoading;

            set => this.Set(() => this.IsLoading, ref this.isLoading, value);
        }

        /// <summary>
        /// Gets or sets the progress message for loading.
        /// </summary>
        public string LoadingProgress
        {
            get => this.loadingProgress;

            set => this.Set(() => this.LoadingProgress, ref this.loadingProgress, value);
        }

        /// <inheritdoc />
        public override async void OnPageNavigatedTo(NavigationEventArgs args)
        {
            this.activationArgs = args.Parameter as ActivationArgs;

            this.IsLoading = true;

            bool initializeSuccess = await this.initializer.InitializeAsync();
            if (initializeSuccess)
            {
                bool activated = await ActivationLauncher.RunActivationProcedureAsync(this.activationArgs);
                NavigateToShell(!activated);
            }

            this.IsLoading = false;
        }

        private static void NavigateToShell(bool navigateToHome)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame?.Navigate(typeof(AppShellPage), navigateToHome);
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

            AuthenticationMessage authMsg = await this.initializer.AuthenticateAsync();

            if (authMsg.IsSuccess)
            {
                bool initialized = await this.initializer.InitializeAsync();
                if (initialized)
                {
                    bool activated = await ActivationLauncher.RunActivationProcedureAsync(this.activationArgs);
                    NavigateToShell(!activated);
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
    }
}