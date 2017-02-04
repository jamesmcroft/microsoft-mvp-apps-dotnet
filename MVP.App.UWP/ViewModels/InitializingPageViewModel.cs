namespace MVP.App.ViewModels
{
    using System;

    using MVP.App.Services.Initialization;
    using MVP.App.Views;

    using Windows.UI.Xaml.Navigation;

    using WinUX.MvvmLight.Xaml.Views;

    /// <summary>
    /// Defines the view-model for the <see cref="InitializingPage"/>
    /// </summary>
    public class InitializingPageViewModel : PageBaseViewModel
    {
        private readonly IAppInitializer initializer;

        private string loadingProgress;

        private bool isLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializingPageViewModel"/> class.
        /// </summary>
        /// <param name="initializer">
        /// The application initializer.
        /// </param>
        public InitializingPageViewModel(IAppInitializer initializer)
        {
            if (initializer == null)
            {
                throw new ArgumentNullException(nameof(initializer), "The application initializer cannot be null.");
            }

            this.initializer = initializer;

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

        /// <inheritdoc />
        public override async void OnPageNavigatedTo(NavigationEventArgs args)
        {
            this.IsLoading = true;

            var initializeSuccess = await this.initializer.InitializeAsync();
            if (initializeSuccess)
            {
                this.NavigationService.Navigate(typeof(MainPage));
            }
            else
            {
                this.IsLoading = false;
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
    }
}