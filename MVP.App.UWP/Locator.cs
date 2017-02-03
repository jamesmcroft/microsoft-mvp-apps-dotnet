namespace MVP.App
{
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.ServiceLocation;

    using MVP.Api;
    using MVP.App.ViewModels;

    /// <summary>
    /// Defines a locator for application view-models and services.
    /// </summary>
    public class Locator
    {
        public Locator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            RegisterServices();
            RegisterViewModels();
        }

        public InitializingPageViewModel InitializingPageViewModel
            => ServiceLocator.Current.GetInstance<InitializingPageViewModel>();

        public MainPageViewModel MainPageViewModel => ServiceLocator.Current.GetInstance<MainPageViewModel>();

        private static void RegisterServices()
        {
            SimpleIoc.Default.Register<IMessenger, Messenger>();

            SimpleIoc.Default.Register<ApiClient>();
        }

        private static void RegisterViewModels()
        {
            SimpleIoc.Default.Register<InitializingPageViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();
        }
    }
}