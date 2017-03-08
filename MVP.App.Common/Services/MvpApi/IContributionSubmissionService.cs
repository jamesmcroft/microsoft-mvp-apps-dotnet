namespace MVP.App.Services.MvpApi
{
    using System.Threading.Tasks;

    using MVP.Api.Models;

    /// <summary>
    /// Defines an interface for a submission service for contributions to the MVP API.
    /// </summary>
    public interface IContributionSubmissionService
    {
        /// <summary>
        /// Submits a contribution through the MVP API for the current user's profile.
        /// </summary>
        /// <param name="contribution">
        /// The contribution to submit.
        /// </param>
        /// <returns>
        /// When this method completes, it returns true if successfully submitted; else false.
        /// </returns>
        Task<bool> SubmitContributionAsync(Contribution contribution);
    }
}