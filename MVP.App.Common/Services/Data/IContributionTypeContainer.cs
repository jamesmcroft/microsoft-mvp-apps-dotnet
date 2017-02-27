namespace MVP.App.Services.Data
{
    using System.Collections.Generic;

    using MVP.Api.Models;

    public interface IContributionTypeContainer : IServiceDataContainer
    {
        /// <summary>
        /// Gets all available contribution types cached locally.
        /// </summary>
        /// <returns>
        /// Returns a collection of <see cref="ContributionType"/>.
        /// </returns>
        IEnumerable<ContributionType> GetAllTypes();
    }
}