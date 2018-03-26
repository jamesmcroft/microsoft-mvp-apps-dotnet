namespace MVP.App.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines an interface for a <see cref="IDataContainer"/> manager.
    /// </summary>
    public interface IDataContainerManager
    {
        /// <summary>
        /// Gets the collection of containers registered with the manager.
        /// </summary>
        IReadOnlyList<IDataContainer> Containers { get; }

        /// <summary>
        /// Gets a value indicating whether any container requires an update.
        /// </summary>
        bool RequiresUpdate { get; }

        /// <summary>
        /// Loads each container's data asynchronously.
        /// </summary>
        /// <returns>
        /// An object that is used to manage the asynchronous operation.
        /// </returns>
        Task LoadAsync();

        /// <summary>
        /// Updates each container's data asynchronously.
        /// </summary>
        /// <returns>
        /// An object that is used to manage the asynchronous operation.
        /// </returns>
        Task UpdateAsync();
    }
}