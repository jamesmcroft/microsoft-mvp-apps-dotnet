namespace MVP.App.Common
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines an interface for an item loader to be used with the <see cref="LazyLoadItemCollection{TItem,TDataContainer}"/>.
    /// </summary>
    /// <typeparam name="TItem">
    /// The type of item that will be loaded.
    /// </typeparam>
    public interface IItemLoader<TItem>
    {
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
        /// When this method completes, it returns a collection of <see cref="TItem"/> objects.
        /// </returns>
        Task<IEnumerable<TItem>> GetMoreItemsAsync(
            uint offset,
            uint limit,
            CancellationToken ct = default(CancellationToken));
    }
}