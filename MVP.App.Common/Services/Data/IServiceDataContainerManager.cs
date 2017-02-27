namespace MVP.App.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IServiceDataContainerManager
    {
        IReadOnlyList<IServiceDataContainer> Containers { get; }

        bool RequiresUpdate { get; }

        Task LoadAsync();

        Task UpdateAsync();
    }
}