namespace MVP.App.Views
{
    using MVP.App.ViewModels;

    using Windows.UI.Xaml.Controls;

    using WinUX.Mvvm.Services;
    using WinUX.Xaml.Controls;

    public sealed partial class AppShellPage : Page
    {
        public AppShellPage()
        {
            Current = this;
            this.InitializeComponent();

            this.AppMenu.NavigationService = NavigationService.Current;
        }

        public static AppShellPage Current { get; set; }

        public static AppMenu Menu => Current.AppMenu;

        public AppShellPageViewModel ViewModel => this.DataContext as AppShellPageViewModel;
    }
}