namespace MVP.App.ViewModels
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using MVP.Api;
    using MVP.Api.Models;
    using MVP.App.Common;
    using MVP.App.Events;
    using MVP.App.Models;
    using MVP.App.Services.MvpApi;

    using Windows.UI.Popups;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Navigation;

    using WinUX.Diagnostics.Tracing;
    using WinUX.Messaging.Dialogs;
    using WinUX.MvvmLight.Xaml.Views;
    using WinUX.Networking;

    public class ContributionsPageViewModel : PageBaseViewModel
    {
        private bool isContributionsVisible;

        private readonly IContributionSubmissionService contributionService;

        private readonly ApiClient client;

        public ContributionsPageViewModel(ApiClient client, IContributionSubmissionService contributionService)
        {
            this.contributionService = contributionService;

            this.Contributions = new LazyLoadItemCollection<Contribution, ContributionItemLoader>(15);
            this.EditableContributionFlyoutViewModel = new EditableContributionFlyoutViewModel();

            this.ContributionClickedCommand =
                new RelayCommand<Contribution>(c => this.EditableContributionFlyoutViewModel.ShowEdit(c));

            this.AddNewContributionCommand = new RelayCommand(() => this.EditableContributionFlyoutViewModel.ShowNew());

            this.SaveContributionCommand = new RelayCommand(async () => await this.SaveContributionAsync());

            this.MessengerInstance.Register<RefreshDataMessage>(this, this.OnRefreshMessage);

            this.Contributions.CollectionChanged +=
                (sender, args) => this.IsContributionsVisible = this.Contributions.Any();
        }

        public LazyLoadItemCollection<Contribution, ContributionItemLoader> Contributions { get; }

        public EditableContributionFlyoutViewModel EditableContributionFlyoutViewModel { get; }

        public ICommand AddNewContributionCommand { get; }

        public ICommand SaveContributionCommand { get; }

        public bool IsContributionsVisible
        {
            get => this.isContributionsVisible;
            set => this.Set(() => this.IsContributionsVisible, ref this.isContributionsVisible, value);
        }

        public ICommand ContributionClickedCommand { get; }

        public override void OnPageNavigatedTo(NavigationEventArgs args)
        {
            ContributionViewModel newContribution = args.Parameter as ContributionViewModel;
            if (newContribution != null)
            {
                this.EditableContributionFlyoutViewModel.ShowNewForEdit(newContribution);
            }
        }

        public override void OnPageNavigatedFrom(NavigationEventArgs args)
        {
            this.EditableContributionFlyoutViewModel?.Close();
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

        private void RefreshContributions()
        {
            if (!NetworkStatusManager.Current.IsConnected())
            {
                return;
            }

            this.Contributions.Reset();
        }

        private async Task SaveContributionAsync()
        {
            if (this.EditableContributionFlyoutViewModel != null && this.EditableContributionFlyoutViewModel.IsValid())
            {
                Contribution contribution = this.EditableContributionFlyoutViewModel.Item.Save();

                if (contribution != null)
                {
                    this.MessengerInstance.Send(new UpdateBusyIndicatorMessage(true, "Sending contribution...", true));

                    bool success = false;
                    bool isAuthenticated = true;

                    try
                    {
                        success = await this.contributionService.SubmitContributionAsync(contribution);
                    }
                    catch (HttpRequestException hre) when (hre.Message.Contains("401"))
                    {
                        isAuthenticated = false;
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Current.WriteError(ex.ToString());
                    }

                    if (!isAuthenticated)
                    {
                        await MessageDialogManager.Current.ShowAsync(
                            "Not authorized",
                            "You are no longer authenticated.",
                            new UICommand(
                                "Ok",
                                async command =>
                                {
                                    await this.client.LogOutAsync();
                                }));

                        Application.Current.Exit();
                    }

                    if (success)
                    {
                        this.EditableContributionFlyoutViewModel.Close();
                    }

                    this.MessengerInstance.Send(new UpdateBusyIndicatorMessage(false));

                    if (success)
                    {
                        this.MessengerInstance.Send(new RefreshDataMessage(RefreshDataMode.Contributions));
                    }
                }
            }
        }
    }
}