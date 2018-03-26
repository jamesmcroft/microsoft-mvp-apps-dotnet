namespace MVP.App.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MVP.Api.Models;
    using MVP.App.Models.Common;
    using MVP.App.Services.MvpApi;

    using WinUX;
    using WinUX.Common;

    public partial class ContributionViewModel : ItemViewModelBase<Contribution>
    {
        private int? id;

        private string typeName;

        private ContributionType type;

        private ActivityTechnology technology;

        private DateTime? startDate;

        private string title;

        private string description;

        private string referenceUrl;

        private ItemVisibility visibility;

        private string annualQuantityValue;

        private string secondAnnualQuantityValue;

        private string annualReachValue;

        private string visibilityValue;

        public ContributionViewModel()
        {
            this.PropertyChanged += this.OnPropertyChanged;
        }

        public int? Id
        {
            get => this.id;

            set => this.Set(() => this.Id, ref this.id, value);
        }

        public string TypeName
        {
            get => this.typeName;

            set => this.Set(() => this.TypeName, ref this.typeName, value);
        }

        public ContributionType Type
        {
            get => this.type;

            set => this.Set(() => this.Type, ref this.type, value);
        }

        public ActivityTechnology Technology
        {
            get => this.technology;

            set => this.Set(() => this.Technology, ref this.technology, value);
        }


        public DateTime? StartDate
        {
            get => this.startDate;

            set => this.Set(() => this.StartDate, ref this.startDate, value);
        }

        public string Title
        {
            get => this.title;

            set => this.Set(() => this.Title, ref this.title, value);
        }

        public string Description
        {
            get => this.description;

            set => this.Set(() => this.Description, ref this.description, value);
        }

        public int? AnnualQuantity { get; set; }

        public string AnnualQuantityValue
        {
            get => this.annualQuantityValue;

            set
            {
                this.Set(() => this.AnnualQuantityValue, ref this.annualQuantityValue, value);

                if (value != null)
                {
                    int val;
                    if (int.TryParse(value, out val))
                    {
                        this.AnnualQuantity = ParseHelper.SafeParseInt(value);
                    }
                }
            }
        }

        public int? SecondAnnualQuantity { get; set; }

        public string SecondAnnualQuantityValue
        {
            get => this.secondAnnualQuantityValue;
            set
            {
                this.Set(() => this.SecondAnnualQuantityValue, ref this.secondAnnualQuantityValue, value);

                if (value != null)
                {
                    int val;
                    if (int.TryParse(value, out val))
                    {
                        this.SecondAnnualQuantity = ParseHelper.SafeParseInt(value);
                    }
                }
            }
        }

        public int? AnnualReach { get; set; }

        public string AnnualReachValue
        {
            get => this.annualReachValue;
            set
            {
                this.Set(() => this.AnnualReachValue, ref this.annualReachValue, value);

                if (value != null)
                {
                    int val;
                    if (int.TryParse(value, out val))
                    {
                        this.AnnualReach = ParseHelper.SafeParseInt(value);
                    }
                }
            }
        }

        public string ReferenceUrl
        {
            get => this.referenceUrl;

            set => this.Set(() => this.ReferenceUrl, ref this.referenceUrl, value);
        }

        public string VisibilityValue
        {
            get => this.visibilityValue;
            set => this.Set(() => this.VisibilityValue, ref this.visibilityValue, value);
        }

        public ItemVisibility Visibility
        {
            get => this.visibility;

            set => this.Set(() => this.Visibility, ref this.visibility, value);
        }

        public void Populate(
            ContributionType contributionType,
            ActivityTechnology contributionTechnology,
            ItemVisibility visibility)
        {
            this.Populate(default(Contribution));

            this.Type = contributionType;
            this.Technology = contributionTechnology;
            this.VisibilityValue = visibility?.Description;
        }

        /// <inheritdoc />
        public override void Populate(Contribution model)
        {
            IEnumerable<ItemVisibility> visibilities = ContributionVisibilities.GetItemVisibilities();

            if (model != null)
            {
                this.Id = model.Id;
                this.TypeName = model.TypeName;
                this.Type = model.Type;
                this.Technology = model.Technology.ToActivityTechnology();
                this.StartDate = model.StartDate;
                this.Title = model.Title;
                this.Description = model.Description;
                this.AnnualQuantity = model.AnnualQuantity;
                this.SecondAnnualQuantity = model.SecondAnnualQuantity;
                this.AnnualReach = model.AnnualReach;
                this.ReferenceUrl = model.ReferenceUrl;
                this.Visibility = model.Visibility == null
                                      ? visibilities.FirstOrDefault()
                                      : visibilities.FirstOrDefault(x => x.Id.Equals(model.Visibility.Id));
                this.VisibilityValue = this.Visibility?.Description;

                this.AnnualQuantityValue = model.AnnualQuantity == null ? string.Empty : model.AnnualQuantity.ToString();
                this.SecondAnnualQuantityValue = model.SecondAnnualQuantity == null
                                                     ? string.Empty
                                                     : model.SecondAnnualQuantity.ToString();
                this.AnnualReachValue = model.AnnualReach == null ? string.Empty : model.AnnualReach.ToString();
            }
            else
            {
                this.Id = 0;
                this.TypeName = string.Empty;
                this.Type = null;
                this.Technology = null;
                this.StartDate = DateTime.UtcNow;
                this.Title = string.Empty;
                this.Description = string.Empty;
                this.AnnualQuantity = null;
                this.SecondAnnualQuantity = null;
                this.AnnualReach = null;
                this.ReferenceUrl = string.Empty;
                this.Visibility = visibilities.FirstOrDefault();
                this.VisibilityValue = this.Visibility?.Description;

                this.AnnualQuantityValue = string.Empty;
                this.SecondAnnualQuantityValue = string.Empty;
                this.AnnualReachValue = string.Empty;
            }
        }

        public void Populate(Uri activationProtocolUri)
        {
            if (activationProtocolUri == null)
            {
                this.Populate(default(Contribution));
            }
            else
            {
                DateTime date = ParseHelper.SafeParseDateTime(activationProtocolUri.ExtractQueryValue("date"));
                if (date.GreaterThan(DateTime.UtcNow) || date == DateTime.MinValue)
                {
                    date = DateTime.UtcNow;
                }

                this.Id = 0;
                this.TypeName = string.Empty;
                this.Type = null;
                this.Technology = null;
                this.StartDate = date;
                this.Title = activationProtocolUri.ExtractQueryValue("title");
                this.Description = activationProtocolUri.ExtractQueryValue("description");
                this.AnnualQuantity = ParseHelper.SafeParseInt(activationProtocolUri.ExtractQueryValue("quantity"));
                this.SecondAnnualQuantity = null;
                this.AnnualReach = ParseHelper.SafeParseInt(activationProtocolUri.ExtractQueryValue("reach"));
                this.ReferenceUrl = activationProtocolUri.ExtractQueryValue("url");
                this.Visibility = ContributionVisibilities.Public;
                this.VisibilityValue = this.Visibility?.Description;

                this.AnnualQuantityValue = this.AnnualQuantity == null ? string.Empty : this.AnnualQuantity.ToString();
                this.SecondAnnualQuantityValue = this.SecondAnnualQuantity == null
                                                     ? string.Empty
                                                     : this.SecondAnnualQuantity.ToString();
                this.AnnualReachValue = this.AnnualReach == null ? string.Empty : this.AnnualReach.ToString();
            }
        }

        /// <inheritdoc />
        public override Contribution Save()
        {
            if (!this.IsValid())
            {
                return null;
            }

            IEnumerable<ItemVisibility> visibilities = ContributionVisibilities.GetItemVisibilities();

            Contribution contribution = new Contribution
                                   {
                                       Id = this.Id,
                                       TypeName = this.TypeName,
                                       Type = this.Type,
                                       Technology = this.Technology.ToContributionTechnology(),
                                       StartDate = this.StartDate,
                                       Title = this.Title,
                                       Description = this.Description,
                                       AnnualQuantity = this.AnnualQuantity,
                                       SecondAnnualQuantity = this.SecondAnnualQuantity,
                                       AnnualReach = this.AnnualReach,
                                       ReferenceUrl = this.ReferenceUrl,
                                       Visibility =
                                           visibilities.FirstOrDefault(
                                               x =>
                                                   x.Description.Equals(
                                                       this.VisibilityValue,
                                                       StringComparison.CurrentCultureIgnoreCase))
                                   };

            return contribution;
        }
    }
}