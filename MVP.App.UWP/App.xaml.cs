namespace MVP.App
{
    using System.Threading.Tasks;

    using Microsoft.Practices.ServiceLocation;

    using MVP.App.Services.Input;
    using MVP.App.Views;

    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    using WinUX.ApplicationModel.Lifecycle;
    using WinUX.Diagnostics;
    using WinUX.Networking;
    using WinUX.Xaml;

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
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
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();

                await this.InitializeServicesAsync();

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(InitializingPage), e.Arguments);
                }

                Window.Current.Activate();
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