namespace MVP.App.Views
{
    using MVP.App.ViewModels;

    using WinUX.MvvmLight.Xaml.Views;

    /// <summary>
    /// Defines a page for the main application.
    /// </summary>
    public sealed partial class MainPage : PageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets the view-model for the page.
        /// </summary>
        public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;
    }
}