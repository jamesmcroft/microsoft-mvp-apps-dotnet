namespace MVP.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using MVP.Api;
    using MVP.Api.Models.MicrosoftAccount;
    using MVP.App.Data;
    using MVP.App.Services.Initialization;
    using MVP.App.Views;

    using Windows.Security.Authentication.Web;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    using WinUX;
    using WinUX.Diagnostics.Tracing;
    using WinUX.Messaging.Dialogs;
    using WinUX.MvvmLight.Xaml.Views;
    using WinUX.Networking;

    /// <summary>
    /// Defines the view-model for the <see cref="InitializingPage"/>
    /// </summary>
    public class InitializingPageViewModel : PageBaseViewModel
    {
        private readonly IAppInitializer initializer;

        private readonly ApiClient apiClient;

        private string loadingProgress;

        private bool isLoading;

        private IAppData data;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializingPageViewModel"/> class.
        /// </summary>
        /// <param name="initializer">
        /// The application initializer.
        /// </param>
        /// <param name="apiClient">
        /// The MVP API client.
        /// </param>
        public InitializingPageViewModel(IAppInitializer initializer, ApiClient apiClient, IAppData data)
        {
            if (initializer == null)
            {
                throw new ArgumentNullException(nameof(initializer), "The application initializer cannot be null.");
            }

            if (apiClient == null)
            {
                throw new ArgumentNullException(nameof(apiClient), "The MVP API client cannot be null.");
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "The app data cannot be null.");
            }

            this.initializer = initializer;
            this.apiClient = apiClient;
            this.data = data;

            this.SigninCommand = new RelayCommand(async () => await this.SignInAsync());

            this.MessengerInstance.Register<AppInitializerMessage>(
                this,
                msg =>
                    {
                        this.LoadingProgress = !string.IsNullOrWhiteSpace(msg?.Message) ? msg.Message : "Loading...";
                    });
        }

        /// <summary>
        /// Gets the command for signing into an MVP account using Live ID.
        /// </summary>
        public ICommand SigninCommand { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the view is loading.
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }
            set
            {
                this.Set(() => this.IsLoading, ref this.isLoading, value);
            }
        }

        /// <summary>
        /// Gets or sets the progress message for loading.
        /// </summary>
        public string LoadingProgress
        {
            get
            {
                return this.loadingProgress;
            }
            set
            {
                this.Set(() => this.LoadingProgress, ref this.loadingProgress, value);
            }
        }

        /// <inheritdoc />
        public override async void OnPageNavigatedTo(NavigationEventArgs args)
        {
            this.IsLoading = true;

            var initializeSuccess = await this.initializer.InitializeAsync();
            if (initializeSuccess)
            {
                this.NavigateToHome();
            }

            this.IsLoading = false;
        }

        /// <inheritdoc />
        public override void OnPageNavigatedFrom(NavigationEventArgs args)
        {
        }

        /// <inheritdoc />
        public override void OnPageNavigatingFrom(NavigatingCancelEventArgs args)
        {
        }

        private async Task SignInAsync()
        {
            if (!NetworkStatusManager.Current.IsConnected())
            {
                // Dialog
                return;
            }

            this.LoadingProgress = "Signing in...";
            this.IsLoading = true;

            var success = true;
            var errorMessage = string.Empty;

            try
            {
                var scopes = new List<MSAScope>
                                 {
                                     MSAScope.Basic,
                                     MSAScope.Emails,
                                     MSAScope.OfflineAccess,
                                     MSAScope.SignIn
                                 };

                var authUri = this.apiClient.RetrieveAuthenticationUri(scopes);

                var result = await WebAuthenticationBroker.AuthenticateAsync(
                                 WebAuthenticationOptions.None,
                                 new Uri(authUri),
                                 new Uri(ApiClient.RedirectUri));

                if (result.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    if (!string.IsNullOrWhiteSpace(result.ResponseData))
                    {
                        var responseUri = new Uri(result.ResponseData);
                        if (responseUri.LocalPath.StartsWith("/oauth20_desktop.srf", StringComparison.OrdinalIgnoreCase))
                        {
                            var error = responseUri.ExtractQueryValue("error");

                            if (string.IsNullOrWhiteSpace(error))
                            {
                                var authCode = responseUri.ExtractQueryValue("code");

                                var msa = await this.apiClient.ExchangeAuthCodeAsync(authCode);
                                if (msa != null)
                                {
                                    await this.data.UpdateAccountAsync(msa);
                                }
                            }
                            else
                            {
                                errorMessage = error;
                            }
                        }
                    }
                }
                else
                {
                    if (result.ResponseStatus != WebAuthenticationStatus.UserCancel)
                    {
                        errorMessage = "Sign in was not successful. Please try again.";
                    }

                    success = false;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Current.WriteWarning(ex.ToString());
                success = false;
            }

            if (success)
            {
                var mvpProfile = await this.apiClient.GetMyProfileAsync();
                if (mvpProfile != null)
                {
                    await this.data.UpdateProfileAsync(mvpProfile);
                    this.NavigateToHome();
                }
                else
                {
                    // Show error
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    await MessageDialogManager.Current.ShowAsync("Sign in error", errorMessage);
                }
            }

            this.IsLoading = false;
        }

        private void NavigateToHome()
        {
            // Get profile and pass onto main page.
            this.NavigationService.Navigate(typeof(MainPage), this.data.CurrentProfile);

            var rootFrame = Window.Current.Content as Frame;
            rootFrame?.Navigate(typeof(AppShellPage));
        }
    }
}