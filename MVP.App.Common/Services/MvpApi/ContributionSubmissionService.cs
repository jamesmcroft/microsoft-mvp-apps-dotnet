namespace MVP.App.Services.MvpApi
{
    using System.Threading.Tasks;

    using MVP.Api;
    using MVP.Api.Models;

    /// <summary>
    /// Defines a submission service for contributions to the MVP API.
    /// </summary>
    public class ContributionSubmissionService : IContributionSubmissionService
    {
        private readonly ApiClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributionSubmissionService"/> class.
        /// </summary>
        /// <param name="client">
        /// The MVP API client.
        /// </param>
        public ContributionSubmissionService(ApiClient client)
        {
            this.client = client;
        }

        /// <inheritdoc />
        public async Task<bool> SubmitContributionAsync(Contribution contribution)
        {
            if (contribution.Id != 0)
            {
                return await this.client.UpdateContributionAsync(contribution);
            }

            var c = await this.client.AddContributionAsync(contribution);
            return c != null;
        }
    }
}