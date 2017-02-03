namespace MVP.App.ViewModels
{
    using MVP.App.Views;

    using Windows.UI.Xaml.Navigation;

    using WinUX.MvvmLight.Xaml.Views;

    /// <summary>
    /// Defines the view-model for the <see cref="InitializingPage"/>
    /// </summary>
    public class InitializingPageViewModel : PageBaseViewModel
    {
        private string loadingProgress;

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