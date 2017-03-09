namespace MVP.App.Views
{
    using MVP.App.ViewModels;

    public sealed partial class InsightsPage
    {
        public InsightsPage()
        {
            this.InitializeComponent();
        }

        public InsightsPageViewModel ViewModel => this.DataContext as InsightsPageViewModel;
    }
}