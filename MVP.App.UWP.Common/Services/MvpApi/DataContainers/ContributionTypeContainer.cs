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

    public class ContributionTypeContainer : IContributionTypeDataContainer
    {
        private const string FileName = "ContributionTypes.mvp";

        private readonly SemaphoreSlim fileAccessSemaphore = new SemaphoreSlim(1, 1);

        private readonly ApiClient client;

        private ContributionTypeContainerWrapper contributionTypes;

        public ContributionTypeContainer(ApiClient client)
        {
            this.client = client;
        }

        /// <inheritdoc />
        public TimeSpan TimeBetweenUpdates => TimeSpan.FromDays(30);

        /// <inheritdoc />
        public DateTime LastDateChecked { get; set; }

        /// <inheritdoc />
        public bool Loaded { get; private set; }

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
            // ToDo - Android, check network is connected.
#endif

            if (this.LastDateChecked < DateTime.UtcNow - this.TimeBetweenUpdates || forceUpdate)
            {
                IEnumerable<ContributionType> serviceTypes = null;

                bool isAuthenticated = true;

                try
                {
                    serviceTypes = await this.client.GetContributionTypesAsync();
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
                    // ToDo - Android, log exception.
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
                    // ToDo - Android, show unauthorized message and exit.
                    return;
#endif
                }

                if (serviceTypes != null)
                {
                    if (this.contributionTypes == null)
                    {
                        this.contributionTypes = new ContributionTypeContainerWrapper();
                    }

                    this.LastDateChecked = DateTime.UtcNow;
                    this.contributionTypes.LastDateChecked = this.LastDateChecked;

                    this.contributionTypes.ContributionTypes.Clear();
                    this.contributionTypes.ContributionTypes.AddRange(serviceTypes);

                    await this.SaveAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task LoadAsync()
        {
            if (this.contributionTypes != null || this.Loaded)
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

                this.contributionTypes = await file.GetDataAsync<ContributionTypeContainerWrapper>();
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

            if (this.contributionTypes == null)
            {
                this.contributionTypes = new ContributionTypeContainerWrapper { LastDateChecked = DateTime.MinValue };

                await this.SaveAsync();
            }

            this.LastDateChecked = this.contributionTypes.LastDateChecked;
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
                    await file.SaveDataAsync(this.contributionTypes);
                }
            }
            finally
            {
                this.fileAccessSemaphore.Release();
            }
        }

        public async Task ClearAsync()
        {
            await this.LoadAsync();

            if (this.contributionTypes == null)
            {
                this.contributionTypes = new ContributionTypeContainerWrapper();
            }

            this.contributionTypes.ContributionTypes = new List<ContributionType>();

            this.LastDateChecked = DateTime.MinValue;
            this.contributionTypes.LastDateChecked = this.LastDateChecked;

            await this.SaveAsync();
        }

        /// <inheritdoc />
        public IEnumerable<ContributionType> GetAllTypes()
        {
            return this.contributionTypes?.ContributionTypes;
        }

        public IEnumerable<ContributionType> GetCommonTypes()
        {
            var items = new List<ContributionType>();

            items.AddRange(this.contributionTypes.ContributionTypes.Where(x => x.Name.Contains("Blog Site")));
            items.AddRange(this.contributionTypes.ContributionTypes.Where(x => x.Name.Contains("Code")));
            items.AddRange(this.contributionTypes.ContributionTypes.Where(x => x.Name.Contains("Forum")));
            items.AddRange(this.contributionTypes.ContributionTypes.Where(x => x.Name.Contains("Open Source")));
            items.AddRange(this.contributionTypes.ContributionTypes.Where(x => x.Name.Contains("Speaking")));
            items.AddRange(this.contributionTypes.ContributionTypes.Where(x => x.Name.Contains("Video")));

            return items.Count > 10 ? items.Take(10) : items;
        }
    }
}