namespace MVP.App.Common
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Practices.ServiceLocation;

    using MVP.Api;
    using MVP.Api.Models;

    public class ContributionItemLoader : IItemLoader<Contribution>
    {
        private readonly ApiClient client;

        public ContributionItemLoader()
            : this(ServiceLocator.Current.GetInstance<ApiClient>())
        {
        }

        public ContributionItemLoader(ApiClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<Contribution>> GetMoreItemsAsync(
            uint offset,
            uint limit,
            CancellationToken ct = new CancellationToken())
        {
            var contributions = await this.client.GetContributionsAsync(
                                    (int)offset,
                                    (int)limit,
                                    CancellationTokenSource.CreateLinkedTokenSource(ct));

            return contributions?.Items ?? new List<Contribution>();
        }
    }
}