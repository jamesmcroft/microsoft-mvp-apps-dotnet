namespace MVP.App.Views
{
    using MVP.App.ViewModels;

    public sealed partial class SettingsPage
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        public SettingsPageViewModel ViewModel => this.DataContext as SettingsPageViewModel;
    }
}
