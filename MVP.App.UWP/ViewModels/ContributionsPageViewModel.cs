namespace MVP.App.ViewModels
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using MVP.Api;
    using MVP.Api.Models;
    using MVP.App.Common;
    using MVP.App.Events;
    using MVP.App.Models;

    using Windows.UI.Xaml.Navigation;

    using WinUX.MvvmLight.Xaml.Views;

    public class ContributionsPageViewModel : PageBaseViewModel
    {
        private readonly ApiClient apiClient;

        private bool isContributionsVisible;

        public ContributionsPageViewModel(ApiClient apiClient)
        {
            this.apiClient = apiClient;

            this.Contributions = new LazyLoadItemCollection<Contribution, ContributionItemLoader>();
            this.ContributionFlyoutViewModel = new ContributionFlyoutViewModel();
            this.EditableContributionFlyoutViewModel = new EditableContributionFlyoutViewModel();

            this.ContributionClickedCommand =
                new RelayCommand<Contribution>(c => this.ContributionFlyoutViewModel.Show(c));

            this.AddNewContributionCommand = new RelayCommand(() => this.EditableContributionFlyoutViewModel.ShowNew());

            this.SaveContributionCommand = new RelayCommand(
                async () => await this.SaveContributionAsync(),
                () => this.EditableContributionFlyoutViewModel.IsValid());

            this.MessengerInstance.Register<RefreshDataMessage>(this, this.OnRefreshMessage);

            this.Contributions.CollectionChanged +=
                (sender, args) => this.IsContributionsVisible = this.Contributions.Any();
        }

        public LazyLoadItemCollection<Contribution, ContributionItemLoader> Contributions { get; }

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

        public override void OnPageNavigatedTo(NavigationEventArgs args)
        {
            this.RefreshContributions();
        }

        public override void OnPageNavigatedFrom(NavigationEventArgs args)
        {
        }

        public override void OnPageNavigatingFrom(NavigatingCancelEventArgs args)
        {
        }

        private void OnRefreshMessage(RefreshDataMessage obj)
        {
            if (obj.Mode == RefreshDataMode.All || obj.Mode == RefreshDataMode.Contributions)
            {
                this.RefreshContributions();
            }
        }

        private async void RefreshContributions()
        {
            // ToDo
        }

        private async Task SaveContributionAsync()
        {
            if (this.EditableContributionFlyoutViewModel != null && this.EditableContributionFlyoutViewModel.IsValid())
            {
                var contribution = this.EditableContributionFlyoutViewModel.Item.Save();

                this.EditableContributionFlyoutViewModel.Close();
                if (contribution != null)
                {
                    this.MessengerInstance.Send(new UpdateBusyIndicatorMessage(true, "Sending contribution..."));

                    if (contribution.Id == 0)
                    {
                        await this.apiClient.AddContributionAsync(contribution);
                    }
                    else
                    {
                        await this.apiClient.UpdateContributionAsync(contribution);
                    }

                    this.MessengerInstance.Send(new UpdateBusyIndicatorMessage(false, string.Empty));

                    this.MessengerInstance.Send(new RefreshDataMessage(RefreshDataMode.Contributions));
                }
            }
        }
    }
}