namespace MVP.App.Services.Initialization
{
    using System;
    using System.Threading.Tasks;

    using GalaSoft.MvvmLight.Messaging;

    using MVP.Api.Models;
    using MVP.Api.Models.MicrosoftAccount;

    using Windows.Storage;

    /// <summary>
    /// Defines a service for initializing an application.
    /// </summary>
    public class AppInitializer : IAppInitializer
    {
        private const string AuthFileName = "init.data";

        private readonly IMessenger messenger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppInitializer"/> class.
        /// </summary>
        /// <param name="messenger">
        /// The MvvmLight messenger.
        /// </param>
        public AppInitializer(IMessenger messenger)
        {
            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger), "The MvvmLight messenger cannot be null");
            }

            this.messenger = messenger;
        }

        /// <inheritdoc />
        public async Task<bool> InitializeAsync()
        {
            bool isSuccess = true;

            this.SendLoadingProgress("Attempting login...");
            if (!await this.AttemptAuthenticationAsync())
            {
                isSuccess = false;
            }

            this.SendLoadingProgress("Done!");
            return isSuccess;
        }

        private async Task<bool> AttemptAuthenticationAsync()
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
                return false;
            }

            MSACredentials credentials;

            try
            {
                credentials = await file.GetDataAsync<MSACredentials>();
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
                return false;
            }

            Locator.ApiClient.Credentials = credentials;

            var profile = await this.TestApiEndpointAsync();
            if (profile == null)
            {
                // Attempt refresh token exchange.
                var exchangeErrored = false;

                try
                {
                    await Locator.ApiClient.ExchangeRefreshTokenAsync();
                }
                catch (Exception ex)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif

                    exchangeErrored = true;
                }

                if (exchangeErrored)
                {
                    await Locator.ApiClient.LogOutAsync();
                    return false;
                }

                profile = await this.TestApiEndpointAsync();
                if (profile == null)
                {
                    await Locator.ApiClient.LogOutAsync();
                    return false;
                }
            }

            return true;
        }

        private async Task<MVPProfile> TestApiEndpointAsync()
        {
            MVPProfile profile = null;
            try
            {
                profile = await Locator.ApiClient.GetMyProfileAsync();
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }

            return profile;
        }

        private void SendLoadingProgress(string message)
        {
            this.messenger.Send(new AppInitializerMessage(message));
        }
    }
}