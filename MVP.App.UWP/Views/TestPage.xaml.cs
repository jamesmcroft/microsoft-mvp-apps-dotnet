namespace MVP.App.Views
{
    using MVP.App.ViewModels;

    using Windows.UI.Xaml.Controls;

    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets the view-model for the page.
        /// </summary>
        public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;
    }
}