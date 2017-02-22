namespace MVP.App.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using MVP.Api.Models;
    using MVP.Api.Models.MicrosoftAccount;
    using MVP.App.Events;

    using Newtonsoft.Json;

    using Windows.Storage;

    public class ProfileData : IProfileData
    {
        private readonly IMessenger messenger;

        private readonly SemaphoreSlim fileAccessSemaphore = new SemaphoreSlim(1, 1);

        private const string AuthFileName = "data.mvp";

        public ProfileData()
        {
        }

        [PreferredConstructor]
        public ProfileData(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        [JsonProperty]
        public MSACredentials CurrentAccount { get; set; }

        [JsonProperty]
        public MVPProfile CurrentProfile { get; set; }

        [JsonProperty]
        public string CurrentProfileImage { get; set; }

        public async Task LoadAsync()
        {
            await this.fileAccessSemaphore.WaitAsync();

            try
            {
                StorageFile file;

                try
                {
                    file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                               AuthFileName,
                               CreationCollisionOption.OpenIfExists);
                }
                catch (Exception ex)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
                    return;
                }

                try
                {
                    var data = await file.GetDataAsync<ProfileData>();
                    if (data != null)
                    {
                        this.CurrentAccount = data.CurrentAccount;
                        this.CurrentProfile = data.CurrentProfile;
                        this.CurrentProfileImage = data.CurrentProfileImage;
                    }
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
                               AuthFileName,
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
                    await file.SaveDataAsync(this);
                }
            }
            finally
            {
                this.fileAccessSemaphore.Release();
            }
        }

        public async Task UpdateAccountAsync(MSACredentials credentials)
        {
            this.CurrentAccount = credentials;
            await this.SaveAsync();
        }

        public async Task UpdateProfileAsync(MVPProfile profile)
        {
            this.CurrentProfile = profile;
            this.messenger.Send(new ProfileUpdatedMessage(this.CurrentProfile));
            await this.SaveAsync();
        }

        public async Task UpdateProfileImageAsync(string profileImage)
        {
            this.CurrentProfileImage = profileImage;
            await this.SaveAsync();
        }
    }
}