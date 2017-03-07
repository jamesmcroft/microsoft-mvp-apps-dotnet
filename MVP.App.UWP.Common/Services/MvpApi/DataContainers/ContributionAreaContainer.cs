using System.Linq;

namespace MVP.App.Services.MvpApi.DataContainers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using MVP.Api;
    using MVP.Api.Models;

    using Windows.Storage;
    using Windows.UI.Xaml;

    using WinUX.Diagnostics.Tracing;
    using WinUX.Networking;

    public class ContributionAreaContainer : IContributionAreaContainer
    {
        private const string FileName = "ContributionAreas.mvp";

        private readonly SemaphoreSlim fileAccessSemaphore = new SemaphoreSlim(1, 1);

        private readonly ApiClient client;

        private ContributionAreaContainerWrapper contributionAreas;

        public ContributionAreaContainer(ApiClient client)
        {
            this.client = client;
        }

        public bool Loaded { get; private set; }

        public TimeSpan TimeBetweenUpdates => TimeSpan.FromDays(30);

        public DateTime LastDateChecked { get; set; }

        public bool RequiresUpdate => this.LastDateChecked < DateTime.UtcNow - this.TimeBetweenUpdates;

        public Task UpdateAsync()
        {
            return this.UpdateAsync(false);
        }

        public async Task UpdateAsync(bool forceUpdate)
        {
            await this.LoadAsync();

            if (!NetworkStatusManager.Current.IsConnected())
            {
                return;
            }

            if (this.LastDateChecked < DateTime.UtcNow - this.TimeBetweenUpdates || forceUpdate)
            {
                IEnumerable<AwardContribution> serviceAreas = null;

                try
                {
                    serviceAreas = await this.client.GetContributionAreasAsync();
                }
                catch (HttpRequestException hre) when (hre.Message.Contains("401"))
                {
                    // Show dialog, unauthorized user detected.
                    Application.Current.Exit();
                }
                catch (Exception ex)
                {
                    EventLogger.Current.WriteError(ex.ToString());
                }

                if (serviceAreas != null)
                {
                    if (this.contributionAreas == null)
                    {
                        this.contributionAreas = new ContributionAreaContainerWrapper();
                    }

                    this.LastDateChecked = DateTime.UtcNow;
                    this.contributionAreas.LastDateChecked = this.LastDateChecked;

                    this.contributionAreas.ContributionAreas.Clear();
                    this.contributionAreas.ContributionAreas.AddRange(serviceAreas);

                    await this.SaveAsync();
                }
            }
        }

        public async Task LoadAsync()
        {
            if (this.contributionAreas != null || this.Loaded)
            {
                return;
            }

            await this.fileAccessSemaphore.WaitAsync();

            this.Loaded = true;

            try
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                    FileName,
                    CreationCollisionOption.OpenIfExists);

                this.contributionAreas = await file.GetDataAsync<ContributionAreaContainerWrapper>();
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }
            finally
            {
                this.fileAccessSemaphore.Release();
            }

            if (this.contributionAreas == null)
            {
                this.contributionAreas = new ContributionAreaContainerWrapper {LastDateChecked = DateTime.MinValue};

                await this.SaveAsync();
            }

            this.LastDateChecked = this.contributionAreas.LastDateChecked;
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
                    await file.SaveDataAsync(this.contributionAreas);
                }
            }
            finally
            {
                this.fileAccessSemaphore.Release();
            }
        }

        public IEnumerable<AwardContribution> GetAllAreas()
        {
            return this.contributionAreas?.ContributionAreas;
        }

        public IEnumerable<ActivityTechnology> GetMyAreaTechnologies()
        {
            var area = this.contributionAreas.ContributionAreas.FirstOrDefault();

            var technologies = area?.Areas.SelectMany(x => x.Items);
            return technologies;
        }
    }
}