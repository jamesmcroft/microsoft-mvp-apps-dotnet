namespace MVP.App.Services.MvpApi.DataContainers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using MVP.Api;
    using MVP.Api.Models;

#if ANDROID
    using XPlat.Storage;
#elif WINDOWS_UWP
    using Windows.Storage;
    using Windows.UI.Popups;
    using Windows.UI.Xaml;

    using WinUX.Diagnostics.Tracing;
    using WinUX.Messaging.Dialogs;
    using WinUX.Networking;
#endif

    public class ContributionAreaContainer : IContributionAreaDataContainer
    {
        private const string FileName = "ContributionAreas.mvp";

        private readonly SemaphoreSlim fileAccessSemaphore = new SemaphoreSlim(1, 1);

        private readonly ApiClient client;

        private ContributionAreaContainerWrapper contributionAreas;

        public ContributionAreaContainer(ApiClient client)
        {
            this.client = client;
        }

        /// <inheritdoc />
        public bool Loaded { get; private set; }

        /// <inheritdoc />
        public TimeSpan TimeBetweenUpdates => TimeSpan.FromDays(30);

        /// <inheritdoc />
        public DateTime LastDateChecked { get; set; }

        /// <inheritdoc />
        public bool RequiresUpdate => this.LastDateChecked < DateTime.UtcNow - this.TimeBetweenUpdates;

        /// <inheritdoc />
        public Task UpdateAsync()
        {
            return this.UpdateAsync(false);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(bool forceUpdate)
        {
            await this.LoadAsync();

#if WINDOWS_UWP
            if (!NetworkStatusManager.Current.IsConnected())
            {
                return;
            }
#elif ANDROID
            // ToDo - Android, check network status is connected.
#endif

            if (this.LastDateChecked < DateTime.UtcNow - this.TimeBetweenUpdates || forceUpdate)
            {
                IEnumerable<AwardContribution> serviceAreas = null;

                bool isAuthenticated = true;

                try
                {
                    serviceAreas = await this.client.GetContributionAreasAsync();
                }
                catch (HttpRequestException hre) when (hre.Message.Contains("401"))
                {
                    isAuthenticated = false;
                }
                catch (Exception ex)
                {
#if WINDOWS_UWP
                    EventLogger.Current.WriteError(ex.ToString());
#elif ANDROID
                    // ToDo - Android, write exception to log.
#endif
                }
                
                if (!isAuthenticated)
                {
#if WINDOWS_UWP
                    await MessageDialogManager.Current.ShowAsync(
                        "Not authorized",
                        "You are no longer authenticated.",
                        new UICommand(
                            "Ok",
                            async command =>
                            {
                                await this.client.LogOutAsync();
                            }));

                    Application.Current.Exit();
#elif ANDROID
                    // ToDo - Android, exit application.
                    return;
#endif
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task SaveAsync()
        {
            await this.fileAccessSemaphore.WaitAsync();

            try
            {
#if WINDOWS_UWP
                StorageFile file = null;
#elif ANDROID
                IStorageFile file = null;
#endif

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

        /// <inheritdoc />
        public async Task ClearAsync()
        {
            await this.LoadAsync();

            if (this.contributionAreas == null)
            {
                this.contributionAreas = new ContributionAreaContainerWrapper();
            }

            this.contributionAreas.ContributionAreas = new List<AwardContribution>();

            this.LastDateChecked = DateTime.MinValue;
            this.contributionAreas.LastDateChecked = this.LastDateChecked;

            await this.SaveAsync();
        }

        /// <inheritdoc />
        public IEnumerable<AwardContribution> GetAllAreas()
        {
            return this.contributionAreas?.ContributionAreas;
        }

        /// <inheritdoc />
        public IEnumerable<ActivityTechnology> GetMyAreaTechnologies()
        {
            var area = this.contributionAreas.ContributionAreas.FirstOrDefault();

            var technologies = area?.Areas.SelectMany(x => x.Items);
            return technologies;
        }
    }
}