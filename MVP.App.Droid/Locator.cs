namespace MVP.App
{
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.ServiceLocation;

    using MVP.Api;
    using MVP.App.Services.Data;

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
            SimpleIoc.Default.Register(() => new ApiClient("ClientId", "ClientSecret", "SubscriptionKey"));

            SimpleIoc.Default.Register<IProfileDataContainer, ProfileDataContainer>();
        }

        private void RegisterViewModels()
        {

        }
    }
}