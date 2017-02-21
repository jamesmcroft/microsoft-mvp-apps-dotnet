namespace MVP.App.Models
{
    using System;
    using System.Collections.Generic;

    using MVP.Api.Models;

    public class ContributionTypeContainerWrapper
    {
        public ContributionTypeContainerWrapper()
        {
            this.ContributionTypes = new List<ContributionType>();
        }

        public List<ContributionType> ContributionTypes { get; set; }

        public DateTime LastDateChecked { get; set; }
    }
}