using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MVP.App.ViewModels;

namespace MVP.App.Views
{
    public sealed partial class InsightsPage : Page
    {
        InsightsPageViewModel ViewModel => this.DataContext as InsightsPageViewModel;

        public InsightsPage()
        {
            InitializeComponent();
        }
    }
}
