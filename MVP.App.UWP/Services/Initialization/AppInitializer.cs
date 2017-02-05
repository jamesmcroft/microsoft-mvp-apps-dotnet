namespace MVP.App.Services.Initialization
{
    using System;
    using System.Threading.Tasks;

    using GalaSoft.MvvmLight.Messaging;

    using MVP.Api;
    using MVP.Api.Models;
    using MVP.App.Data;

    using WinUX.Diagnostics.Tracing;
    using WinUX.Networking;

    /// <summary>
    /// Defines a service for initializing an application.
    /// </summary>
    public class AppInitializer : IAppInitializer
    {
        private readonly IMessenger messenger;

        private readonly ApiClient apiClient;

        private IAppData data;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppInitializer"/> class.
        /// </summary>
        /// <param name="messenger">
        /// The MvvmLight messenger.
        /// </param>
        /// <param name="apiClient">
        /// The MVP API client.
        /// </param>
        public AppInitializer(IMessenger messenger, ApiClient apiClient, IAppData data)
        {
            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger), "The MvvmLight messenger cannot be null");
            }

            if (apiClient == null)
            {
                throw new ArgumentNullException(nameof(apiClient), "The MVP API client cannot be null");
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "The app data cannot be null");
            }

            this.messenger = messenger;
            this.apiClient = apiClient;
            this.data = data;
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
            await this.data.LoadAsync();

            if (this.data.CurrentAccount == null)
            {
                return false;
            }

            this.apiClient.Credentials = this.data.CurrentAccount;

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

                    await this.data.UpdateProfileAsync(profile);
                }
                else
                {
                    await this.data.UpdateProfileAsync(profile);
                }
            }

            await this.data.UpdateAccountAsync(this.apiClient.Credentials);

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