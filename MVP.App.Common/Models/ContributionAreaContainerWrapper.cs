namespace MVP.App.Models
{
    using System;
    using System.Collections.Generic;

    using MVP.Api.Models;

    public class ContributionAreaContainerWrapper
    {
        public ContributionAreaContainerWrapper()
        {
            this.ContributionAreas = new List<AwardContribution>();
        }

        public List<AwardContribution> ContributionAreas { get; set; }

        public DateTime LastDateChecked { get; set; }
    }
}