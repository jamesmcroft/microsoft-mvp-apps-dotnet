namespace MVP.App.Views
{
    using MVP.App.ViewModels;

    using WinUX.MvvmLight.Xaml.Views;

    public sealed partial class ContributionsPage : PageBase
    {
        public ContributionsPage()
        {
            this.InitializeComponent();
        }

        public ContributionsPageViewModel ViewModel => this.DataContext as ContributionsPageViewModel;
    }
}