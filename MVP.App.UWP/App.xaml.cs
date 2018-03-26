namespace MVP.App
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CommonServiceLocator;

    using MVP.App.Common;
    using MVP.App.Helpers;
    using MVP.App.Services.Initialization;
    using MVP.App.Services.Input;
    using MVP.App.Views;

    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.ApplicationModel.VoiceCommands;
    using Windows.Foundation.Metadata;
    using Windows.Media.SpeechRecognition;
    using Windows.Storage;
    using Windows.UI.ViewManagement;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    using WinUX.ApplicationModel.Lifecycle;
    using WinUX.Diagnostics;
    using WinUX.Diagnostics.Tracing;
    using WinUX.Input.Speech;
    using WinUX.Networking;
    using WinUX.Xaml;

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            this.Resuming += this.OnResuming;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.
        /// </summary>
        /// <param name="e">
        /// Details about the launch request and process.
        /// </param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            await this.LaunchApplicationAsync(e.Arguments);
        }

        /// <summary>
        /// Invoked when the application is activated by an outside source, i.e. protocol or Cortana.
        /// </summary>
        /// <param name="args">
        /// Details about the activation.
        /// </param>
        protected override async void OnActivated(IActivatedEventArgs args)
        {
            ActivationArgs activationArgs = null;

            switch (args.Kind)
            {
                case ActivationKind.Protocol:
                    if (args is ProtocolActivatedEventArgs protocolArgs)
                    {
                        activationArgs = new ActivationArgs(protocolArgs.Uri);
                    }

                    break;
                case ActivationKind.VoiceCommand:
                    if (args is VoiceCommandActivatedEventArgs voiceArgs)
                    {
                        SpeechRecognitionResult result = voiceArgs.Result;
                        string voiceCommand = result.RulePath[0];

                        SpeechCommand speechCommand = new SpeechCommand(result, new List<string>());

                        // Switch on the voice command string to determine function
                        activationArgs = new ActivationArgs(speechCommand);
                    }

                    break;
            }

            if (!(Window.Current.Content is Frame))
            {
                await this.LaunchApplicationAsync(activationArgs);
            }
            else
            {
                await ActivationLauncher.RunActivationProcedureAsync(activationArgs);
            }

            base.OnActivated(args);
        }

        private async Task LaunchApplicationAsync(object launchArgs)
        {
            TitleBarHelper.ExtendToTitleBar(true);
            await SetupStatusBarAsync();
            await SetupCortanaVoiceCommandsAsync();

            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();

                await this.InitializeServicesAsync();

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(InitializingPage), launchArgs);
            }

            Window.Current.Activate();
        }

        private static async Task SetupCortanaVoiceCommandsAsync()
        {
            try
            {
                StorageFile commandFile = await Package.Current.InstalledLocation.GetFileAsync("VoiceCommands.xml");
                await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(commandFile);
            }
            catch (Exception ex)
            {
                EventLogger.Current.WriteError(ex.Message);
            }
        }

        private static async Task SetupStatusBarAsync()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    await statusBar.HideAsync();
                }
            }
        }

        private async Task InitializeServicesAsync()
        {
            UIDispatcher.Initialize();
            await AppDiagnostics.Current.StartAsync();
            NetworkStatusManager.Current.Initialize();
            ServiceLocator.Current.GetInstance<KeyboardCharacterService>().Start();
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            await AppLifecycleManager.Current.SuspendAsync(e);
        }

        private async void OnResuming(object sender, object e)
        {
            await AppLifecycleManager.Current.ResumeAsync();
        }
    }
}