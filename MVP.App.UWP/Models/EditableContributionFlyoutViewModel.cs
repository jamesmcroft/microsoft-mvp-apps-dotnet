namespace MVP.App.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using GalaSoft.MvvmLight.Ioc;

    using Microsoft.Practices.ServiceLocation;

    using MVP.Api.Models;
    using MVP.App.Common;
    using MVP.App.Models.Common;
    using MVP.App.Services.MvpApi.DataContainers;

    public class EditableContributionFlyoutViewModel : ItemCustomFlyoutViewModel<ContributionViewModel>, IValidate
    {
        private DateTimeOffset maxDateOfActivity;

        public EditableContributionFlyoutViewModel()
            : this(
                ServiceLocator.Current.GetInstance<IContributionAreaContainer>(),
                ServiceLocator.Current.GetInstance<IContributionTypeContainer>())
        {
        }

        [PreferredConstructor]
        public EditableContributionFlyoutViewModel(
            IContributionAreaContainer areaContainer,
            IContributionTypeContainer typeContainer)
        {
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

        public void ShowNew()
        {
            this.MaxDateOfActivity = DateTimeOffset.UtcNow;

            this.Title = "Add new contribution";

            this.IsInEdit = true;

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

            this.Show(contributionViewModel);
        }

        /// <inheritdoc />
        public bool IsValid()
        {
            return this.Item != null && this.Item.IsValid();
        }
    }
}