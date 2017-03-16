namespace MVP.App.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using GalaSoft.MvvmLight.Ioc;

    using Microsoft.Practices.ServiceLocation;

    using MVP.Api;
    using MVP.Api.Models;
    using MVP.App.Common;
    using MVP.App.Events;
    using MVP.App.Models.Common;
    using MVP.App.Services.MvpApi;
    using MVP.App.Services.MvpApi.DataContainers;

    using Windows.UI.Popups;
    using Windows.UI.Xaml;

    using WinUX.Common;
    using WinUX.Diagnostics.Tracing;
    using WinUX.Messaging.Dialogs;

    public class EditableContributionFlyoutViewModel : ItemCustomFlyoutViewModel<ContributionViewModel>, IValidate
    {
        private DateTimeOffset maxDateOfActivity;

        private readonly ApiClient client;

        public EditableContributionFlyoutViewModel()
            : this(
                ServiceLocator.Current.GetInstance<ApiClient>(),
                ServiceLocator.Current.GetInstance<IContributionAreaDataContainer>(),
                ServiceLocator.Current.GetInstance<IContributionTypeDataContainer>())
        {
        }

        [PreferredConstructor]
        public EditableContributionFlyoutViewModel(
            ApiClient client,
            IContributionAreaDataContainer areaContainer,
            IContributionTypeDataContainer typeContainer)
        {
            this.client = client;

            this.Areas =
                areaContainer.GetAllAreas()
                    .SelectMany(awardContribution => awardContribution.Areas)
                    .GroupBy(x => x.AwardName)
                    .Select(g => g.First())
                    .SelectMany(a => a.Items)
                    .ToList();
            this.Types = typeContainer.GetAllTypes();
            this.Visibilities = ContributionVisibilities.GetItemVisibilities();
            this.VisibilityValues = this.Visibilities.Select(x => x.Description);
        }

        public IEnumerable<ContributionType> Types { get; }

        public IEnumerable<ActivityTechnology> Areas { get; }

        public IEnumerable<ItemVisibility> Visibilities { get; }

        public IEnumerable<string> VisibilityValues { get; }

        public override async void Delete()
        {
            if (this.Item?.Id != null)
            {
                this.MessengerInstance.Send(new UpdateBusyIndicatorMessage(true, "Deleting...", true));

                bool deleted = false;
                bool isAuthenticated = true;

                try
                {
                    deleted = await this.client.DeleteContributionAsync(this.Item.Id.Value);
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

                if (deleted)
                {
                    this.Close();
                }

                this.MessengerInstance.Send(new UpdateBusyIndicatorMessage(false));

                if (deleted)
                {
                    this.MessengerInstance.Send(new RefreshDataMessage(RefreshDataMode.Contributions));
                }
            }
        }

        public void ShowNew()
        {
            this.Title = "Add new contribution";

            this.IsInEdit = true;
            this.CanDelete = false;
            this.CanEdit = false;

            var contributionViewModel = new ContributionViewModel();
            contributionViewModel.Populate(this.Types.FirstOrDefault(), this.Areas.FirstOrDefault(), this.Visibilities.FirstOrDefault());
            this.Show(contributionViewModel);
        }

        public void ShowEdit(Contribution model)
        {
            var contributionViewModel = new ContributionViewModel();
            contributionViewModel.Populate(model);

            this.Title = contributionViewModel.Title;

            this.IsInEdit = false;

            // The ID being checked here is for PGIs which don't appear to be editable in the portal.
            this.CanEdit = contributionViewModel.Type != null
                           && contributionViewModel.Type.Id
                           != ParseHelper.SafeParseGuid("f36464de-179a-e411-bbc8-6c3be5a82b68");

            this.CanDelete = contributionViewModel.Type != null
                             && contributionViewModel.Type.Id
                             != ParseHelper.SafeParseGuid("f36464de-179a-e411-bbc8-6c3be5a82b68");

            this.Show(contributionViewModel);
        }

        public void ShowNewForEdit(ContributionViewModel viewModel)
        {
            this.Title = "Add new contribution";

            this.IsInEdit = true;
            this.CanDelete = false;

            viewModel.Type = this.Types.FirstOrDefault();
            viewModel.Technology = this.Areas.FirstOrDefault();

            this.Show(viewModel);
        }

        /// <inheritdoc />
        public bool IsValid()
        {
            return this.Item != null && this.Item.IsValid();
        }
    }
}