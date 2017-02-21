namespace MVP.App.Models
{
    using System.Collections.ObjectModel;

    using MVP.Api.Models;
    using MVP.App.Models.Common;

    public class ContributionViewModel : ItemViewModelBase<Contribution>
    {
        private int? id;

        private string typeName;

        public int? Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.Set(() => this.Id, ref this.id, value);
            }
        }

        public string TypeName
        {
            get
            {
                return this.typeName;
            }
            set
            {
                this.Set(() => this.TypeName, ref this.typeName, value);
            }
        }

        public override void Populate(Contribution model)
        {
            if (model != null)
            {
                this.Id = model.Id;
            }
        }

        public override bool IsValid()
        {
            // ToDo; validation logic.
            return true;
        }
    }
}