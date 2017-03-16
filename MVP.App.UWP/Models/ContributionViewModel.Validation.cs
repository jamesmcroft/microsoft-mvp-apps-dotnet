namespace MVP.App.Models
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    using WinUX;
    using WinUX.Common;

    public partial class ContributionViewModel
    {
        private bool isTechnologyInvalid;

        private bool isStartDateInvalid;

        private bool isTitleInvalid;

        private bool isAnnualQuantityInvalid;

        private bool isSecondAnnualQuantityInvalid;

        private bool isAnnualReachInvalid;

        private bool isReferenceUrlInvalid;

        private bool isVisibilityInvalid;

        private string annualQuantityTitle;

        private string annualReachTitle;

        private string secondAnnualQuantityTitle;

        private bool isSecondAnnualQuantityVisible;

        private bool isAnnualQuantityMandatory;

        private bool isReferenceUrlMandatory;

        private bool isSecondAnnualQuantityMandatory;

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

        public bool IsReferenceUrlMandatory
        {
            get
            {
                return this.isReferenceUrlMandatory;
            }
            set
            {
                this.Set(() => this.IsReferenceUrlMandatory, ref this.isReferenceUrlMandatory, value);
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

        public bool IsAnnualQuantityMandatory
        {
            get
            {
                return this.isAnnualQuantityMandatory;
            }
            set
            {
                this.Set(() => this.IsAnnualQuantityMandatory, ref this.isAnnualQuantityMandatory, value);
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

        public bool IsSecondAnnualQuantityMandatory
        {
            get
            {
                return this.isSecondAnnualQuantityMandatory;
            }
            set
            {
                this.Set(() => this.IsSecondAnnualQuantityMandatory, ref this.isSecondAnnualQuantityMandatory, value);
            }
        }

        public bool IsSecondAnnualQuantityVisible
        {
            get
            {
                return this.isSecondAnnualQuantityVisible;
            }
            set
            {
                this.Set(() => this.IsSecondAnnualQuantityVisible, ref this.isSecondAnnualQuantityVisible, value);
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

        public string AnnualQuantityTitle
        {
            get
            {
                return this.annualQuantityTitle;
            }
            set
            {
                this.Set(() => this.AnnualQuantityTitle, ref this.annualQuantityTitle, value);
            }
        }

        public string SecondAnnualQuantityTitle
        {
            get
            {
                return this.secondAnnualQuantityTitle;
            }
            set
            {
                this.Set(() => this.SecondAnnualQuantityTitle, ref this.secondAnnualQuantityTitle, value);
            }
        }

        public string AnnualReachTitle
        {
            get
            {
                return this.annualReachTitle;
            }
            set
            {
                this.Set(() => this.AnnualReachTitle, ref this.annualReachTitle, value);
            }
        }

        /// <inheritdoc />
        public override bool IsValid()
        {
            var isValid = true;

            if (this.IsSecondAnnualQuantityVisible)
            {
                if (this.IsSecondAnnualQuantityInvalid)
                {
                    isValid = false;
                }
            }

            if (isValid)
            {
                isValid = !this.IsAnnualQuantityInvalid && !this.IsAnnualReachInvalid && !this.IsReferenceUrlInvalid
                          && !this.IsStartDateInvalid && !this.IsTechnologyInvalid && !this.IsTitleInvalid
                          && !this.IsVisibilityInvalid;
            }

            return isValid;
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

            if (propName == nameof(this.Type))
            {
                if (this.Type != null)
                {
                    var name = this.Type.Name ?? string.Empty;

                    if (name.Equals("Article", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of articles";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Number of views";

                        this.IsReferenceUrlMandatory = false;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Equals("Blog Site Posts", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of posts";
                        this.SecondAnnualQuantityTitle = "Number of subscribers";
                        this.AnnualReachTitle = "Annual unique visitors";

                        this.IsReferenceUrlMandatory = true;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = true;
                        this.IsSecondAnnualQuantityMandatory = false;
                    }
                    else if (name.Contains("Book", CompareOptions.IgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of books";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Copies sold";

                        this.IsReferenceUrlMandatory = false;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Contains("Code Project", CompareOptions.IgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of projects";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Number of downloads";

                        this.IsReferenceUrlMandatory = true;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Equals("Code Samples", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of samples";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Number of downloads";

                        this.IsReferenceUrlMandatory = true;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Contains("Conference", CompareOptions.IgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of conferences";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Number of visitors";

                        this.IsReferenceUrlMandatory = false;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Equals("Forum Moderator", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of threads";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Annual reach";

                        this.IsReferenceUrlMandatory = false;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Contains("3rd Party forums", CompareOptions.IgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of answers";
                        this.SecondAnnualQuantityTitle = "Number of posts";
                        this.AnnualReachTitle = "Views of answers";

                        this.IsReferenceUrlMandatory = true;
                        this.IsAnnualQuantityMandatory = false;
                        this.IsSecondAnnualQuantityVisible = true;
                        this.IsSecondAnnualQuantityMandatory = true;
                    }
                    else if (name.Contains("Microsoft Forums", CompareOptions.IgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of answers";
                        this.SecondAnnualQuantityTitle = "Number of posts";
                        this.AnnualReachTitle = "Views of answers";

                        this.IsReferenceUrlMandatory = true;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = true;
                        this.IsSecondAnnualQuantityMandatory = false;
                    }
                    else if (name.Equals("Mentorship", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of mentees";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Annual reach";

                        this.IsReferenceUrlMandatory = false;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Contains("Open Source", CompareOptions.IgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of projects";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Commits";

                        this.IsReferenceUrlMandatory = false;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Equals("Other", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Annual quantity";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Annual reach";

                        this.IsReferenceUrlMandatory = false;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Equals("Product Group Feedback", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of events";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Number of feedbacks";

                        this.IsReferenceUrlMandatory = false;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Equals("Site Owner", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Posts";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Visitors";

                        this.IsReferenceUrlMandatory = false;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Contains("Speaking", CompareOptions.IgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Talks";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Number of attendees";

                        this.IsReferenceUrlMandatory = false;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Contains("Technical Social Media", CompareOptions.IgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of posts";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Number of followers";

                        this.IsReferenceUrlMandatory = true;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Contains("Translation Review", CompareOptions.IgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Annual quantity";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Annual reach";

                        this.IsReferenceUrlMandatory = false;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Equals("User Group Owner", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of meetings";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Members";

                        this.IsReferenceUrlMandatory = false;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Equals("Video", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of videos";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Number of views";

                        this.IsReferenceUrlMandatory = true;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Equals("Webcast", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of videos";
                        this.SecondAnnualQuantityTitle = string.Empty;
                        this.AnnualReachTitle = "Number of views";

                        this.IsReferenceUrlMandatory = false;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = false;
                        this.IsSecondAnnualQuantityMandatory = false;

                        this.SecondAnnualQuantity = null;
                        this.SecondAnnualQuantityValue = null;
                    }
                    else if (name.Equals("WebSite Posts", StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.AnnualQuantityTitle = "Number of posts";
                        this.SecondAnnualQuantityTitle = "Number of subscribers";
                        this.AnnualReachTitle = "Annual unique visitors";

                        this.IsReferenceUrlMandatory = true;
                        this.IsAnnualQuantityMandatory = true;
                        this.IsSecondAnnualQuantityVisible = true;
                        this.IsSecondAnnualQuantityMandatory = false;
                    }
                }
            }
        }
    }
}