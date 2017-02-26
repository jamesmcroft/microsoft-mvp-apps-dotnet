namespace MVP.App.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using MVP.Api;
    using MVP.Api.Models;
    using MVP.App.Data;
    using MVP.App.Events;
    using MVP.App.Models;

    using Windows.UI.Xaml.Navigation;

    using WinUX.MvvmLight.Xaml.Views;

    public class ContributionsPageViewModel : PageBaseViewModel
    {
        private ApiClient apiClient;

        private IProfileData data;

        private bool isContributionsVisible;

        public ContributionsPageViewModel(ApiClient apiClient, IProfileData data)
        {
            this.apiClient = apiClient;
            this.data = data;

            this.Contributions = new ObservableCollection<Contribution>();
            this.ContributionFlyoutViewModel = new ContributionFlyoutViewModel();
            this.EditableContributionFlyoutViewModel = new EditableContributionFlyoutViewModel();

            this.ContributionClickedCommand =
                new RelayCommand<Contribution>(c => this.ContributionFlyoutViewModel.Show(c));

            this.AddNewContributionCommand = new RelayCommand(() => this.EditableContributionFlyoutViewModel.ShowNew());

            this.SaveContributionCommand = new RelayCommand(
                async () => await this.SaveContributionAsync(),
                () => this.EditableContributionFlyoutViewModel.IsValid());

            this.MessengerInstance.Register<RefreshDataMessage>(this, this.RefreshContributions);
        }

        /// <summary>
        /// Gets the custom fly-out view model for the contributions.
        /// </summary>
        public ContributionFlyoutViewModel ContributionFlyoutViewModel { get; }

        public EditableContributionFlyoutViewModel EditableContributionFlyoutViewModel { get; }

        public ICommand AddNewContributionCommand { get; }

        public ICommand SaveContributionCommand { get; }

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

        private void RefreshContributions(RefreshDataMessage obj)
        {

        }

        private async Task SaveContributionAsync()
        {
            if (this.EditableContributionFlyoutViewModel != null && this.EditableContributionFlyoutViewModel.IsValid())
            {
                var contribution = this.EditableContributionFlyoutViewModel.Item.Save();
                if (contribution != null)
                {
                    if (contribution.Id == 0)
                    {
                        var added = await this.apiClient.AddContributionAsync(contribution);
                        if (added != null)
                        {
                            // ToDo;
                        }
                    }
                    else
                    {
                        var updated = await this.apiClient.UpdateContributionAsync(contribution);
                        if (updated)
                        {
                            // ToDo;
                        }
                    }
                }
            }
        }
    }
}