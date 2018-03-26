namespace MVP.App.Services.MvpApi.DataContainers
{
    using System.Collections.Generic;

    using MVP.Api.Models;
    using MVP.App.Services.Data;

    /// <summary>
    /// Defines an interface for handling MVP contribution areas.
    /// </summary>
    public interface IContributionAreaDataContainer : IDataContainer
    {
        /// <summary>
        /// Gets all of the stored award contribution areas.
        /// </summary>
        /// <returns>
        /// When this method completes, it returns a collection of <see cref="AwardContribution"/> objects.
        /// </returns>
        IEnumerable<AwardContribution> GetAllAreas();

        /// <summary>
        /// Gets all of the stored award technologies that are commonly associated with the current user's MVP profile.
        /// </summary>
        /// <returns>
        /// When this method completes, it returns a collection of <see cref="ActivityTechnology"/> objects.
        /// </returns>
        IEnumerable<ActivityTechnology> GetMyAreaTechnologies();
    }
}