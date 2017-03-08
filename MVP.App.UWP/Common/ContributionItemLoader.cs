namespace MVP.App.Common
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Practices.ServiceLocation;

    using MVP.Api;
    using MVP.Api.Models;

    /// <summary>
    /// Defines a contribution item loader to be used with the <see cref="LazyLoadItemCollection{TItem,TDataContainer}"/>.
    /// </summary>
    public class ContributionItemLoader : IItemLoader<Contribution>
    {
        private readonly ApiClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributionItemLoader"/> class.
        /// </summary>
        public ContributionItemLoader()
            : this(ServiceLocator.Current.GetInstance<ApiClient>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributionItemLoader"/> class.
        /// </summary>
        /// <param name="client">
        /// The MVP API client.
        /// </param>
        public ContributionItemLoader(ApiClient client)
        {
            this.client = client;
        }

        /// <inheritdoc />
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