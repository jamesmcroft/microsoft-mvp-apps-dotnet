namespace MVP.App.Views
{
    using MVP.App.ViewModels;

    public sealed partial class AboutPage
    {
        public AboutPage()
        {
            this.InitializeComponent();
        }

        public AboutPageViewModel ViewModel => this.DataContext as AboutPageViewModel;
    }
}
