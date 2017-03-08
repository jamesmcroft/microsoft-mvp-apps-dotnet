namespace MVP.App.Services.MvpApi.DataContainers
{
    using System.Collections.Generic;

    using MVP.Api.Models;
    using MVP.App.Services.Data;
    
    /// <summary>
    /// Defines an interface for handling MVP contribution types.
    /// </summary>
    public interface IContributionTypeDataContainer : IDataContainer
    {
        /// <summary>
        /// Gets all of the stored contribution types.
        /// </summary>
        /// <returns>
        /// When this method completes, it returns a collection of <see cref="ContributionType"/> objects.
        /// </returns>
        IEnumerable<ContributionType> GetAllTypes();

        /// <summary>
        /// Gets a subset of stored contribution types that would be the most commonly used.
        /// </summary>
        /// <returns>
        /// When this method completes, it returns a collection of 10 or less <see cref="ContributionType"/> objects.
        /// </returns>
        IEnumerable<ContributionType> GetCommonTypes();
    }
}