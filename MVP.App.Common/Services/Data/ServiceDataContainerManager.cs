namespace MVP.App.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using GalaSoft.MvvmLight.Ioc;

    using Microsoft.Practices.ServiceLocation;

    using MVP.App.Services.MvpApi.DataContainers;

    public class ServiceDataContainerManager : IServiceDataContainerManager
    {
        private readonly List<IServiceDataContainer> containers;

        private readonly SemaphoreSlim containerSemaphore = new SemaphoreSlim(1, 1);

        public ServiceDataContainerManager()
            : this(
                ServiceLocator.Current.GetInstance<IProfileDataContainer>(),
                ServiceLocator.Current.GetInstance<IContributionTypeContainer>(),
                ServiceLocator.Current.GetInstance<IContributionAreaContainer>())
        {
        }

        [PreferredConstructor]
        public ServiceDataContainerManager(
            IProfileDataContainer profileContainer,
            IContributionTypeContainer typeContainer,
            IContributionAreaContainer areaContainer)
        {
            this.containers = new List<IServiceDataContainer> { profileContainer, typeContainer, areaContainer };
        }

        public IReadOnlyList<IServiceDataContainer> Containers => this.containers;

        public bool RequiresUpdate
        {
            get
            {
                return this.Containers != null && this.Containers.Any(x => x.RequiresUpdate);
            }
        }

        public async Task LoadAsync()
        {
            await this.containerSemaphore.WaitAsync();

            try
            {
                foreach (var container in this.Containers)
                {
                    await container.LoadAsync().ConfigureAwait(false);
                }
            }
            finally
            {
                this.containerSemaphore.Release();
            }
        }

        public async Task UpdateAsync()
        {
            if (this.RequiresUpdate)
            {
                await this.containerSemaphore.WaitAsync();

                try
                {
                    foreach (var container in this.Containers)
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