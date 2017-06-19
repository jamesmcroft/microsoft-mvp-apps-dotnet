namespace MVP.App
{
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.ServiceLocation;

    using MVP.App.Services.Data;
    using MVP.App.Services.Initialization;
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
        {
            get
            {
                return SimpleIoc.Default.GetInstance<InitializingActivityViewModel>();
            }
        }

        public MainActivityViewModel MainActivityViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MainActivityViewModel>();
            }
        }

        public InsightsActivityViewModel InsightsActivityViewModel
        {
            get
            {
                return SimpleIoc.Default.GetInstance<InsightsActivityViewModel>();
            }
        }

        private void RegisterServices()
        {
            SimpleIoc.Default.Register<IMessenger, Messenger>();
            SimpleIoc.Default.Register(ApiClientProvider.GetClient);
            SimpleIoc.Default.Register<IProfileDataContainer, ProfileDataContainer>();
            SimpleIoc.Default.Register<IContributionTypeDataContainer, ContributionTypeContainer>();
            SimpleIoc.Default.Register<IContributionAreaDataContainer, ContributionAreaContainer>();
            SimpleIoc.Default.Register<IDataContainerManager, DataContainerManager>();
            SimpleIoc.Default.Register<IContributionSubmissionService, ContributionSubmissionService>();
            SimpleIoc.Default.Register<IAppInitializer, AppInitializer>();
        }

        private void RegisterViewModels()
        {
            SimpleIoc.Default.Register<InitializingActivityViewModel>();
            SimpleIoc.Default.Register<MainActivityViewModel>();
            SimpleIoc.Default.Register<InsightsActivityViewModel>();
        }
    }
}