namespace MVP.App.Services.MvpApi.DataContainers
{
    using System;
    using System.Collections.Generic;

    using MVP.Api.Models;

    /// <summary>
    /// Defines a wrapper object for the data from the <see cref="IContributionAreaDataContainer"/>.
    /// </summary>
    public class ContributionAreaContainerWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContributionAreaContainerWrapper"/> class.
        /// </summary>
        public ContributionAreaContainerWrapper()
        {
            this.ContributionAreas = new List<AwardContribution>();
        }

        /// <summary>
        /// Gets or sets the list of stored contribution areas.
        /// </summary>
        public List<AwardContribution> ContributionAreas { get; set; }

        /// <summary>
        /// Gets or sets the date that the container was last checked for an update.
        /// </summary>
        public DateTime LastDateChecked { get; set; }
    }
}