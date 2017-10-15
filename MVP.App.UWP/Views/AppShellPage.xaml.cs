namespace MVP.App.Views
{
    using MVP.App.ViewModels;

    public sealed partial class AppShellPage
    {
        public AppShellPage()
        {
            this.InitializeComponent();
        }

        public AppShellPageViewModel ViewModel => this.DataContext as AppShellPageViewModel;
    }
}