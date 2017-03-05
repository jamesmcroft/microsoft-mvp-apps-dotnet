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
    using MVP.App.Models.Common;
    using MVP.App.Services.MvpApi.DataContainers;

    using Windows.UI.Xaml;

    using MVP.App.Events;

    using WinUX.Diagnostics.Tracing;

    public class EditableContributionFlyoutViewModel : ItemCustomFlyoutViewModel<ContributionViewModel>, IValidate
    {
        private DateTimeOffset maxDateOfActivity;

        private ApiClient client;

        public EditableContributionFlyoutViewModel()
            : this(
                ServiceLocator.Current.GetInstance<ApiClient>(),
                ServiceLocator.Current.GetInstance<IContributionAreaContainer>(),
                ServiceLocator.Current.GetInstance<IContributionTypeContainer>())
        {
        }

        [PreferredConstructor]
        public EditableContributionFlyoutViewModel(
            ApiClient client,
            IContributionAreaContainer areaContainer,
            IContributionTypeContainer typeContainer)
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
        }

        public IEnumerable<ContributionType> Types { get; }

        public IEnumerable<ActivityTechnology> Areas { get; }

        public DateTimeOffset MaxDateOfActivity
        {
            get
            {
                return this.maxDateOfActivity;
            }
            set
            {
                this.Set(() => this.MaxDateOfActivity, ref this.maxDateOfActivity, value);
            }
        }

        public override async void Delete()
        {
            if (this.Item?.Id != null)
            {
                try
                {
                    this.MessengerInstance.Send(new UpdateBusyIndicatorMessage(true, "Deleting...", true));

                    var deleted = await this.client.DeleteContributionAsync(this.Item.Id.Value);
                    if (deleted)
                    {
                        this.Close();
                    }

                    this.MessengerInstance.Send(new UpdateBusyIndicatorMessage(false));

                    this.MessengerInstance.Send(new RefreshDataMessage(RefreshDataMode.Contributions));
                }
                catch (HttpRequestException hre) when (hre.Message.Contains("401"))
                {
                    Application.Current.Exit();
                }
                catch (Exception ex)
                {
                    EventLogger.Current.WriteError(ex.ToString());
                }
            }
        }

        public void ShowNew()
        {
            this.MaxDateOfActivity = DateTimeOffset.UtcNow;

            this.Title = "Add new contribution";

            this.IsInEdit = true;
            this.CanDelete = false;

            var contributionViewModel = new ContributionViewModel();
            contributionViewModel.Populate(this.Types.FirstOrDefault(), this.Areas.FirstOrDefault());
            this.Show(contributionViewModel);
        }

        public void ShowEdit(Contribution model)
        {
            this.MaxDateOfActivity = DateTimeOffset.UtcNow;

            var contributionViewModel = new ContributionViewModel();
            contributionViewModel.Populate(model);

            this.Title = contributionViewModel.Title;

            this.IsInEdit = false;
            this.CanDelete = true;

            this.Show(contributionViewModel);
        }

        /// <inheritdoc />
        public bool IsValid()
        {
            return this.Item != null && this.Item.IsValid();
        }
    }
}