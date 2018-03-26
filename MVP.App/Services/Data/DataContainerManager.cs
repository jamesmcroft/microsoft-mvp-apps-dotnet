namespace MVP.App.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using GalaSoft.MvvmLight.Ioc;

    using MVP.App.Services.MvpApi.DataContainers;

    /// <summary>
    /// Defines a manager for <see cref="IDataContainer"/> objects.
    /// </summary>
    public class DataContainerManager : IDataContainerManager
    {
        private readonly List<IDataContainer> containers;

        private readonly SemaphoreSlim containerSemaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContainerManager"/> class.
        /// </summary>
        /// <param name="newContainers">
        /// A collection of <see cref="IDataContainer"/> objects to register.
        /// </param>
        public DataContainerManager(params IDataContainer[] newContainers)
        {
            this.containers = new List<IDataContainer>();
            foreach (IDataContainer container in newContainers)
            {
                if (container != null)
                {
                    this.containers.Add(container);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContainerManager"/> class.
        /// </summary>
        /// <param name="profileContainer">
        /// The profile data container.
        /// </param>
        /// <param name="typeContainer">
        /// The contribution type data container.
        /// </param>
        /// <param name="areaContainer">
        /// The contribution area data container.
        /// </param>
        [PreferredConstructor]
        public DataContainerManager(
            IProfileDataContainer profileContainer,
            IContributionTypeDataContainer typeContainer,
            IContributionAreaDataContainer areaContainer)
        {
            this.containers = new List<IDataContainer> { profileContainer, typeContainer, areaContainer };
        }

        /// <inheritdoc />
        public IReadOnlyList<IDataContainer> Containers => this.containers;

        /// <inheritdoc />
        public bool RequiresUpdate
        {
            get
            {
                return this.Containers != null && this.Containers.Any(x => x.RequiresUpdate);
            }
        }

        /// <inheritdoc />
        public async Task LoadAsync()
        {
            await this.containerSemaphore.WaitAsync();

            try
            {
                foreach (IDataContainer container in this.Containers)
                {
                    await container.LoadAsync().ConfigureAwait(false);
                }
            }
            finally
            {
                this.containerSemaphore.Release();
            }
        }

        /// <inheritdoc />
        public async Task UpdateAsync()
        {
            if (this.RequiresUpdate)
            {
                await this.containerSemaphore.WaitAsync();

                try
                {
                    foreach (IDataContainer container in this.Containers)
                    {
                        await container.UpdateAsync().ConfigureAwait(false);
                    }
                }
                finally
                {
                    this.containerSemaphore.Release();
                }
            }
        }
    }
}