namespace MVP.App.Services.Data
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MVP.Api;
    using MVP.App.Data;
    using MVP.App.Models;

    using Windows.Storage;

    public class ContributionTypeContainer : IServiceDataContainer
    {
        private const string FileName = "ContributionTypes.mvp";

        private readonly SemaphoreSlim fileAccessSemaphore = new SemaphoreSlim(1, 1);

        private readonly ApiClient client;

        private ContributionTypeContainerWrapper contributionTypes;

        public ContributionTypeContainer(ApiClient client)
        {
            this.client = client;
        }

        public DateTime LastDateChecked { get; set; }

        public async Task UpdateAsync()
        {
            await this.LoadAsync();

            // ToDo; get data
        }

        public async Task LoadAsync()
        {
            if (this.contributionTypes != null)
            {
                return;
            }

            await this.fileAccessSemaphore.WaitAsync();

            try
            {
                try
                {
                    var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                                   FileName,
                                   CreationCollisionOption.OpenIfExists);

                    this.contributionTypes = await file.GetDataAsync<ContributionTypeContainerWrapper>();
                }
                catch (Exception ex)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
                }
            }
            finally
            {
                this.fileAccessSemaphore.Release();
            }

            if (this.contributionTypes == null)
            {
                this.contributionTypes = new ContributionTypeContainerWrapper { LastDateChecked = DateTime.MinValue };

                await this.SaveAsync();
            }
            else
            {
                this.LastDateChecked = this.contributionTypes.LastDateChecked;
            }
        }

        public async Task SaveAsync()
        {
            await this.fileAccessSemaphore.WaitAsync();

            try
            {
                StorageFile file = null;

                try
                {
                    file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                               FileName,
                               CreationCollisionOption.OpenIfExists);
                }
                catch (Exception ex)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
                }

                if (file != null)
                {
                    await file.SaveDataAsync(this.contributionTypes);
                }
            }
            finally
            {
                this.fileAccessSemaphore.Release();
            }
        }
    }
}