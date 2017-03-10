namespace MVP.App
{
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.ServiceLocation;

    using MVP.App.Services.MvpApi;
    using MVP.App.Services.MvpApi.DataContainers;
    using MVP.App.ViewModels;

    public class Locator
    {
        public Locator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            this.RegisterServices();
            this.RegisterViewModels();
        }

        public InitializingActivityViewModel InitializingActivityViewModel
            => ServiceLocator.Current.GetInstance<InitializingActivityViewModel>();

        public MainActivityViewModel MainActivityViewModel
            => ServiceLocator.Current.GetInstance<MainActivityViewModel>();

        private void RegisterServices()
        {
            SimpleIoc.Default.Register<IMessenger, Messenger>();
            SimpleIoc.Default.Register(ApiClientProvider.GetClient);
            SimpleIoc.Default.Register<IProfileDataContainer, ProfileDataContainer>();
        }

        private void RegisterViewModels()
        {
            SimpleIoc.Default.Register<InitializingActivityViewModel>();
            SimpleIoc.Default.Register<MainActivityViewModel>();
        }
    }
}