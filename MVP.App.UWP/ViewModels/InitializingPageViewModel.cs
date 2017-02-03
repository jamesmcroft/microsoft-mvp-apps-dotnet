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
        public override void OnPageNavigatedTo(NavigationEventArgs args)
        {
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