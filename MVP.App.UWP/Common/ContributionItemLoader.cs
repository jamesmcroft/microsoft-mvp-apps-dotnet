namespace MVP.App.Common
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using CommonServiceLocator;

    using GalaSoft.MvvmLight.Ioc;

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
        [PreferredConstructor]
        public ContributionItemLoader(ApiClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Gets more items from the data source asynchronously.
        /// </summary>
        /// <param name="offset">
        /// The initial offset (index) point to get data from.
        /// </param>
        /// <param name="limit">
        /// The limit of items to retrieve.
        /// </param>
        /// <param name="ct">
        /// A cancellation token to be used if required.
        /// </param>
        /// <returns>
        /// When this method completes, it returns a collection of <see cref="Contribution"/> objects.
        /// </returns>
        public async Task<IEnumerable<Contribution>> GetMoreItemsAsync(
            uint offset,
            uint limit,
            CancellationToken ct = new CancellationToken())
        {
            Contributions contributions = await this.client.GetContributionsAsync(
                                              (int)offset,
                                              (int)limit,
                                              CancellationTokenSource.CreateLinkedTokenSource(ct));

            return contributions?.Items ?? new List<Contribution>();
        }
    }
}