namespace MVP.App.Models
{
    using System;

    using MVP.Api.Models;
    using MVP.App.Data;
    using MVP.App.Models.Common;

    public class EditableContributionFlyoutViewModel : ItemCustomFlyoutViewModel<ContributionViewModel>, IValidate
    {
        private DateTimeOffset maxDateOfActivity;

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

        public void Show(Contribution model)
        {
            this.MaxDateOfActivity = DateTimeOffset.UtcNow;

            var contributionViewModel = new ContributionViewModel();
            contributionViewModel.Populate(model);
            this.Show(contributionViewModel);
        }

        /// <inheritdoc />
        public bool IsValid()
        {
            return this.Item != null && this.Item.IsValid();
        }
    }
}