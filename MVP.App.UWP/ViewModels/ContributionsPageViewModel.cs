namespace MVP.App.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using MVP.Api;
    using MVP.App.Data;

    using Windows.UI.Xaml.Navigation;

    using MVP.Api.Models;
    using MVP.App.Events;
    using MVP.App.Models;

    using WinUX.Collections.ObjectModel;
    using WinUX.MvvmLight.Xaml.Views;

    public class ContributionsPageViewModel : PageBaseViewModel
    {
        private ApiClient apiClient;

        private IProfileData data;

        private bool isContributionsVisible;

        public ContributionsPageViewModel(ApiClient apiClient, IProfileData data)
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

            this.Contributions = new ObservableCollection<Contribution>();
            this.ReadonlyContributionFlyoutViewModel = new ContributionCustomFlyoutViewModel();

            this.ContributionClickedCommand =
                new RelayCommand<Contribution>(c => this.ReadonlyContributionFlyoutViewModel.Show(c));

            this.AddNewContributionCommand = new RelayCommand(this.AddNewContribution);

            this.MessengerInstance.Register<RefreshDataMessage>(this, this.RefreshContributions);
        }

        /// <summary>
        /// Gets the custom fly-out view model for the contributions.
        /// </summary>
        public ContributionCustomFlyoutViewModel ReadonlyContributionFlyoutViewModel { get; }

        public ICommand AddNewContributionCommand { get; }

        public bool IsContributionsVisible
        {
            get
            {
                return this.isContributionsVisible;
            }
            set
            {
                this.Set(() => this.IsContributionsVisible, ref this.isContributionsVisible, value);
            }
        }

        public ICommand ContributionClickedCommand { get; }

        public ObservableCollection<Contribution> Contributions { get; }

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
            // Show dialog for edit mode.
        }

        private void RefreshContributions(RefreshDataMessage obj)
        {

        }
    }
}