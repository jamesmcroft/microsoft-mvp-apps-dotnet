namespace MVP.App.Views
{
    using MVP.App.ViewModels;

    using WinUX.MvvmLight.Xaml.Views;

    /// <summary>
    /// Defines a page for application initialization.
    /// </summary>
    public sealed partial class InitializingPage : PageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitializingPage"/> class.
        /// </summary>
        public InitializingPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets the view-model for the page.
        /// </summary>
        public InitializingPageViewModel ViewModel => this.DataContext as InitializingPageViewModel;
    }
}