namespace MVP.App.Services.Initialization
{
    using System;
    using System.Threading.Tasks;

    using GalaSoft.MvvmLight.Messaging;

    using MVP.Api;
    using MVP.Api.Models;
    using MVP.Api.Models.MicrosoftAccount;

    using Windows.Storage;

    using WinUX.Diagnostics.Tracing;
    using WinUX.Networking;

    /// <summary>
    /// Defines a service for initializing an application.
    /// </summary>
    public class AppInitializer : IAppInitializer
    {
        private const string AuthFileName = "init.data";

        private readonly IMessenger messenger;

        private readonly ApiClient apiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppInitializer"/> class.
        /// </summary>
        /// <param name="messenger">
        /// The MvvmLight messenger.
        /// </param>
        /// <param name="apiClient">
        /// The MVP API client.
        /// </param>
        public AppInitializer(IMessenger messenger, ApiClient apiClient)
        {
            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger), "The MvvmLight messenger cannot be null");
            }

            if (apiClient == null)
            {
                throw new ArgumentNullException(nameof(apiClient), "The MVP API client cannot be null");
            }

            this.messenger = messenger;
            this.apiClient = apiClient;
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

        public async Task SaveCredentialsAsync()
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
                await file.SaveDataAsync(this.apiClient.Credentials);
            }
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

            this.apiClient.Credentials = credentials;

            // Check network status.
            if (NetworkStatusManager.Current.CurrentConnectionType != NetworkConnectionType.Disconnected
                || NetworkStatusManager.Current.CurrentConnectionType != NetworkConnectionType.Unknown)
            {
                var profile = await this.TestApiEndpointAsync();
                if (profile == null)
                {
                    // Attempt refresh token exchange.
                    var exchangeErrored = false;

                    try
                    {
                        await this.apiClient.ExchangeRefreshTokenAsync();
                    }
                    catch (ApiException aex)
                    {
                        EventLogger.Current.WriteWarning(aex.ToString());
                        exchangeErrored = true;
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
                        await this.apiClient.LogOutAsync();
                        return false;
                    }

                    profile = await this.TestApiEndpointAsync();
                    if (profile == null)
                    {
                        await this.apiClient.LogOutAsync();
                        return false;
                    }
                }
            }

            await this.SaveCredentialsAsync();

            return true;
        }

        private async Task<MVPProfile> TestApiEndpointAsync()
        {
            MVPProfile profile = null;
            try
            {
                profile = await this.apiClient.GetMyProfileAsync();
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