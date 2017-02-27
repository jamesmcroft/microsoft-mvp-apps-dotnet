namespace MVP.App.Common
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IItemLoader<TItem>
    {
        Task<IEnumerable<TItem>> GetMoreItemsAsync(
            uint offset,
            uint limit,
            CancellationToken ct = default(CancellationToken));
    }
}