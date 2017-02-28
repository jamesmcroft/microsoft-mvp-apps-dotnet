namespace MVP.App.Services.MvpApi.DataContainers
{
#if WINDOWS_UWP
    using Windows.Storage
    using WinUX.Networking;
    using Windows.UI.Xaml;
#elif ANDROID
    using XamarinApiToolkit.Storage;
#endif
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    
    using GalaSoft.MvvmLight.Messaging;

    using MVP.Api;
    using MVP.Api.Models;
    using MVP.Api.Models.MicrosoftAccount;
    using MVP.App.Events;

    public class ProfileDataContainer : IProfileDataContainer
    {
        private const string FileName = "ProfileData.mvp";

        private readonly SemaphoreSlim fileAccessSemaphore = new SemaphoreSlim(1, 1);

        private readonly ApiClient client;

        private readonly IMessenger messenger;

        private ProfileDataContainerWrapper profileData;

        public ProfileDataContainer(IMessenger messenger, ApiClient client)
        {
            this.messenger = messenger;
            this.client = client;
        }

        /// <inheritdoc />
        public bool Loaded { get; private set; }

        /// <inheritdoc />
        public TimeSpan TimeBetweenUpdates => TimeSpan.FromDays(1);

        /// <inheritdoc />
        public DateTime LastDateChecked { get; set; }

        public bool RequiresUpdate => this.LastDateChecked < DateTime.UtcNow - this.TimeBetweenUpdates;

        public MSACredentials Account => this.profileData?.Account;

        public MVPProfile Profile => this.profileData?.Profile;

        public string ProfileImage => this.profileData?.ProfileImage;

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
            // Check network connectivity
#endif

            if (this.LastDateChecked < DateTime.UtcNow - this.TimeBetweenUpdates || forceUpdate)
            {
                MVPProfile profile = null;
                string profileImage = string.Empty;

                try
                {
                    profile = await this.client.GetMyProfileAsync();
                    profileImage = await this.client.GetMyProfileImageAsync();
                }
                catch (HttpRequestException hre) when (hre.Message.Contains("401"))
                {
                    // Show dialog, unauthorized user detected.
#if WINDOWS_UWP
                    Application.Current.Exit();
#endif
                }

                if (profile != null || !string.IsNullOrWhiteSpace(profileImage))
                {
                    if (this.profileData == null)
                    {
                        this.profileData = new ProfileDataContainerWrapper();
                    }

                    this.LastDateChecked = DateTime.UtcNow;

                    this.profileData.Profile = profile;
                    this.profileData.ProfileImage = profileImage;

                    await this.SaveAsync();
                }

                this.messenger.Send(new ProfileUpdatedMessage(this.Profile));

                await this.SaveAsync();
            }
        }

        /// <inheritdoc />
        public async Task LoadAsync()
        {
            if (this.profileData != null || this.Loaded)
            {
                return;
            }

            await this.fileAccessSemaphore.WaitAsync();

            this.Loaded = true;

            try
            {
#if WINDOWS_UWP
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                               FileName,
                               CreationCollisionOption.OpenIfExists);
#elif ANDROID || iOS
                var file = await AppData.Current.LocalFolder.CreateFileAsync(
                               FileName,
                               FileStoreCreationOption.OpenIfExists);
#endif

                this.profileData = await file.GetDataAsync<ProfileDataContainerWrapper>();
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

            if (this.profileData == null)
            {
                this.profileData = new ProfileDataContainerWrapper { LastDateChecked = DateTime.MinValue };

                await this.SaveAsync();
            }

            this.LastDateChecked = this.profileData.LastDateChecked;
        }

        /// <inheritdoc />
        public async Task SaveAsync()
        {
            await this.fileAccessSemaphore.WaitAsync();

            try
            {
#if WINDOWS_UWP
                StorageFile file = null;
#elif ANDROID || iOS
                IAppFile file = null;
#endif

                try
                {
#if WINDOWS_UWP
                    file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                               FileName,
                               CreationCollisionOption.OpenIfExists);
#elif ANDROID || iOS
                    file = await AppData.Current.LocalFolder.CreateFileAsync(
                               FileName,
                               FileStoreCreationOption.OpenIfExists);
#endif
                }
                catch (Exception ex)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
                }

                if (file != null)
                {
                    await file.SaveDataAsync(this.profileData);
                }
            }
            finally
            {
                this.fileAccessSemaphore.Release();
            }
        }

        public async Task SetProfileAsync(MVPProfile profile)
        {
            await this.LoadAsync();

            if (this.profileData == null)
            {
                this.profileData = new ProfileDataContainerWrapper();
            }

            this.profileData.Profile = profile;
            this.messenger.Send(new ProfileUpdatedMessage(this.Profile));

            this.LastDateChecked = DateTime.UtcNow;
            this.profileData.LastDateChecked = this.LastDateChecked;

            await this.SaveAsync();
        }

        public async Task SetAccountAsync(MSACredentials account)
        {
            await this.LoadAsync();

            if (this.profileData == null)
            {
                this.profileData = new ProfileDataContainerWrapper();
            }

            this.profileData.Account = account;

            this.LastDateChecked = DateTime.UtcNow;
            this.profileData.LastDateChecked = this.LastDateChecked;

            await this.SaveAsync();
        }

        public async Task SetProfileImageAsync(string image)
        {
            await this.LoadAsync();

            if (this.profileData == null)
            {
                this.profileData = new ProfileDataContainerWrapper();
            }

            this.profileData.ProfileImage = image;

            this.LastDateChecked = DateTime.UtcNow;
            this.profileData.LastDateChecked = this.LastDateChecked;

            await this.SaveAsync();
        }
    }
}