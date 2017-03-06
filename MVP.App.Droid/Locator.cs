namespace MVP.App
{
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.ServiceLocation;

    using MVP.Api;
    using MVP.App.Services.MvpApi;
    using MVP.App.Services.MvpApi.DataContainers;

    public class Locator
    {
        public Locator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            this.RegisterServices();
            this.RegisterViewModels();
        }

        private void RegisterServices()
        {
            SimpleIoc.Default.Register<IMessenger, Messenger>();
            SimpleIoc.Default.Register(ApiClientProvider.GetClient);
            SimpleIoc.Default.Register<IProfileDataContainer, ProfileDataContainer>();
        }

        private void RegisterViewModels()
        {

        }
    }
}