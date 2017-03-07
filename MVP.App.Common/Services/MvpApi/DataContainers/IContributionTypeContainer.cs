namespace MVP.App.Services.MvpApi.DataContainers
{
    using System.Collections.Generic;

    using MVP.Api.Models;
    using MVP.App.Services.Data;

    public interface IContributionTypeContainer : IServiceDataContainer
    {
        /// <summary>
        /// Gets all available contribution types cached locally.
        /// </summary>
        /// <returns>
        /// Returns a collection of <see cref="ContributionType"/>.
        /// </returns>
        IEnumerable<ContributionType> GetAllTypes();

        IEnumerable<ContributionType> GetCommonTypes();
    }
}