namespace MVP.App
{
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.ServiceLocation;

    using MVP.Api;
    using MVP.App.Data;
    using MVP.App.Services.Initialization;
    using MVP.App.ViewModels;

    /// <summary>
    /// Defines a locator for application view-models and services.
    /// </summary>
    public class Locator
    {
        static Locator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            RegisterServices();
            RegisterViewModels();
        }

        public InitializingPageViewModel InitializingPageViewModel
            => ServiceLocator.Current.GetInstance<InitializingPageViewModel>();

        public AppShellPageViewModel AppShellPageViewModel
            => ServiceLocator.Current.GetInstance<AppShellPageViewModel>();

        public MainPageViewModel MainPageViewModel => ServiceLocator.Current.GetInstance<MainPageViewModel>();

        private static void RegisterServices()
        {
            SimpleIoc.Default.Register<IMessenger, Messenger>();
            SimpleIoc.Default.Register(() => new ApiClient("ClientId", "ClientSecret", "SubscriptionKey"));
            SimpleIoc.Default.Register<IAppData, AppData>();
            SimpleIoc.Default.Register<IAppInitializer, AppInitializer>();
        }

        private static void RegisterViewModels()
        {
            SimpleIoc.Default.Register<InitializingPageViewModel>();
            SimpleIoc.Default.Register<AppShellPageViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();
        }
    }
}