namespace MVP.App.ViewModels
{
    using System;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using MVP.Api;
    using MVP.App.Data;

    using Windows.UI.Xaml.Navigation;

    using WinUX.MvvmLight.Xaml.Views;

    public class ContributionsPageViewModel : PageBaseViewModel
    {
        private ApiClient apiClient;

        private IAppData data;

        public ContributionsPageViewModel(ApiClient apiClient, IAppData data)
        {
            if (apiClient == null)
            {
                throw new ArgumentNullException(nameof(apiClient), "The MVP API client cannot be null.");
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "The app data cannot be null.");
            }

            this.apiClient = apiClient;
            this.data = data;

            this.AddNewContributionCommand = new RelayCommand(this.AddNewContribution);
        }

        public ICommand AddNewContributionCommand { get; }

        public override void OnPageNavigatedTo(NavigationEventArgs args)
        {
        }

        public override void OnPageNavigatedFrom(NavigationEventArgs args)
        {
        }

        public override void OnPageNavigatingFrom(NavigatingCancelEventArgs args)
        {
        }

        private void AddNewContribution()
        {

        }
    }
}