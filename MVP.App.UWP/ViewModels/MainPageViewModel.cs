namespace MVP.App.ViewModels
{
    using MVP.App.Views;

    using Windows.UI.Xaml.Navigation;

    using WinUX.MvvmLight.Xaml.Views;

    /// <summary>
    /// Defines the view-model for the <see cref="MainPage"/>
    /// </summary>
    public class MainPageViewModel : PageBaseViewModel
    {
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