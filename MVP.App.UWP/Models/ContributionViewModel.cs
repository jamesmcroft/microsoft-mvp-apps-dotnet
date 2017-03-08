namespace MVP.App.Models
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    using MVP.Api.Models;
    using MVP.App.Models.Common;
    using MVP.App.Services.MvpApi;

    using WinUX;
    using WinUX.Common;

    public class ContributionViewModel : ItemViewModelBase<Contribution>
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

        private bool isTechnologyInvalid;

        private bool isStartDateInvalid;

        private bool isTitleInvalid;

        private bool isAnnualQuantityInvalid;

        private string annualQuantityValue;

        private string secondAnnualQuantityValue;

        private bool isSecondAnnualQuantityInvalid;

        private string annualReachValue;

        private bool isAnnualReachInvalid;

        private bool isReferenceUrlInvalid;

        private bool isVisibilityInvalid;

        public ContributionViewModel()
        {
            this.PropertyChanged += this.OnPropertyChanged;
        }

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

        public ContributionType Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.Set(() => this.Type, ref this.type, value);
            }
        }

        public ActivityTechnology Technology
        {
            get
            {
                return this.technology;
            }

            set
            {
                this.Set(() => this.Technology, ref this.technology, value);
            }
        }

        public bool IsTechnologyInvalid
        {
            get
            {
                return this.isTechnologyInvalid;
            }

            set
            {
                this.Set(() => this.IsTechnologyInvalid, ref this.isTechnologyInvalid, value);
            }
        }

        public DateTime? StartDate
        {
            get
            {
                return this.startDate;
            }

            set
            {
                this.Set(() => this.StartDate, ref this.startDate, value);
            }
        }

        public bool IsStartDateInvalid
        {
            get
            {
                return this.isStartDateInvalid;
            }

            set
            {
                this.Set(() => this.IsStartDateInvalid, ref this.isStartDateInvalid, value);
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.Set(() => this.Title, ref this.title, value);
            }
        }

        public bool IsTitleInvalid
        {
            get
            {
                return this.isTitleInvalid;
            }

            set
            {
                this.Set(() => this.IsTitleInvalid, ref this.isTitleInvalid, value);
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.Set(() => this.Description, ref this.description, value);
            }
        }

        public int? AnnualQuantity { get; set; }

        public string AnnualQuantityValue
        {
            get
            {
                return this.annualQuantityValue;
            }

            set
            {
                this.Set(() => this.AnnualQuantityValue, ref this.annualQuantityValue, value);
            }
        }

        public bool IsAnnualQuantityInvalid
        {
            get
            {
                return this.isAnnualQuantityInvalid;
            }

            set
            {
                this.Set(() => this.IsAnnualQuantityInvalid, ref this.isAnnualQuantityInvalid, value);
            }
        }

        public int? SecondAnnualQuantity { get; set; }

        public string SecondAnnualQuantityValue
        {
            get
            {
                return this.secondAnnualQuantityValue;
            }
            set
            {
                this.Set(() => this.SecondAnnualQuantityValue, ref this.secondAnnualQuantityValue, value);
            }
        }

        public bool IsSecondAnnualQuantityInvalid
        {
            get
            {
                return this.isSecondAnnualQuantityInvalid;
            }
            set
            {
                this.Set(() => this.IsSecondAnnualQuantityInvalid, ref this.isSecondAnnualQuantityInvalid, value);
            }
        }

        public int? AnnualReach { get; set; }

        public string AnnualReachValue
        {
            get
            {
                return this.annualReachValue;
            }
            set
            {
                this.Set(() => this.AnnualReachValue, ref this.annualReachValue, value);
            }
        }

        public bool IsAnnualReachInvalid
        {
            get
            {
                return this.isAnnualReachInvalid;
            }
            set
            {
                this.Set(() => this.IsAnnualReachInvalid, ref this.isAnnualReachInvalid, value);
            }
        }

        public string ReferenceUrl
        {
            get
            {
                return this.referenceUrl;
            }

            set
            {
                this.Set(() => this.ReferenceUrl, ref this.referenceUrl, value);
            }
        }

        public bool IsReferenceUrlInvalid
        {
            get
            {
                return this.isReferenceUrlInvalid;
            }
            set
            {
                this.Set(() => this.IsReferenceUrlInvalid, ref this.isReferenceUrlInvalid, value);
            }
        }

        public ItemVisibility Visibility
        {
            get
            {
                return this.visibility;
            }

            set
            {
                this.Set(() => this.Visibility, ref this.visibility, value);
            }
        }

        public bool IsVisibilityInvalid
        {
            get
            {
                return this.isVisibilityInvalid;
            }
            set
            {
                this.Set(() => this.IsVisibilityInvalid, ref this.isVisibilityInvalid, value);
            }
        }

        public void Populate(ContributionType contributionType, ActivityTechnology contributionTechnology, ItemVisibility visibility)
        {
            this.Populate(default(Contribution));

            this.Type = contributionType;
            this.Technology = contributionTechnology;
            this.Visibility = visibility;
        }

        /// <inheritdoc />
        public override void Populate(Contribution model)
        {
            var visibilities = ContributionVisibilities.GetItemVisibilities();

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

                this.AnnualQuantityValue = model.AnnualQuantity == null ? string.Empty : model.AnnualQuantity.ToString();
                this.SecondAnnualQuantityValue = model.SecondAnnualQuantity == null ? string.Empty : model.SecondAnnualQuantity.ToString();
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
                var date = ParseHelper.SafeParseDateTime(activationProtocolUri.ExtractQueryValue("date"));
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

                this.AnnualQuantityValue = this.AnnualQuantity == null ? string.Empty : this.AnnualQuantity.ToString();
                this.SecondAnnualQuantityValue = this.SecondAnnualQuantity == null ? string.Empty : this.SecondAnnualQuantity.ToString();
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

            var contribution = new Contribution
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
                Visibility = this.Visibility
            };

            return contribution;
        }

        /// <inheritdoc />
        public override bool IsValid()
        {
            return !this.IsAnnualQuantityInvalid && !this.IsAnnualReachInvalid && !this.IsReferenceUrlInvalid
                   && !this.IsSecondAnnualQuantityInvalid && !this.IsStartDateInvalid && !this.IsTechnologyInvalid
                   && !this.IsTitleInvalid && !this.IsVisibilityInvalid;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propName = e.PropertyName;

            if (propName == nameof(this.IsAnnualQuantityInvalid))
            {
                this.AnnualQuantity = !this.IsAnnualQuantityInvalid
                                          ? (int?)ParseHelper.SafeParseInt(this.AnnualQuantityValue)
                                          : null;
            }

            if (propName == nameof(this.IsAnnualReachInvalid))
            {
                this.AnnualReach = !this.IsAnnualReachInvalid
                                       ? (int?)ParseHelper.SafeParseInt(this.AnnualReachValue)
                                       : null;
            }

            if (propName == nameof(this.IsSecondAnnualQuantityInvalid))
            {
                this.SecondAnnualQuantity = !this.IsSecondAnnualQuantityInvalid
                                                ? (int?)ParseHelper.SafeParseInt(this.SecondAnnualQuantityValue)
                                                : null;
            }
        }


    }
}