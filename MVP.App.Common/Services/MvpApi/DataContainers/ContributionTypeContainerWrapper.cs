namespace MVP.App.Services.MvpApi.DataContainers
{
    using System;
    using System.Collections.Generic;

    using MVP.Api.Models;

    /// <summary>
    /// Defines a wrapper object for the data from the <see cref="IContributionTypeDataContainer"/>.
    /// </summary>
    public class ContributionTypeContainerWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContributionTypeContainerWrapper"/> class.
        /// </summary>
        public ContributionTypeContainerWrapper()
        {
            this.ContributionTypes = new List<ContributionType>();
        }

        /// <summary>
        /// Gets or sets the list of stored contribution types.
        /// </summary>
        public List<ContributionType> ContributionTypes { get; set; }

        /// <summary>
        /// Gets or sets the date that the container was last checked for an update.
        /// </summary>
        public DateTime LastDateChecked { get; set; }
    }
}