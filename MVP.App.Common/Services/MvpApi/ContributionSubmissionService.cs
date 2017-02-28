namespace MVP.App.Services.MvpApi
{
    using System.Threading.Tasks;

    using MVP.Api;
    using MVP.Api.Models;

    public class ContributionSubmissionService : IContributionSubmissionService
    {
        private readonly ApiClient client;

        public ContributionSubmissionService(ApiClient client)
        {
            this.client = client;
        }

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