namespace MVP.App
{
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.ServiceLocation;

    using MVP.Api;
    using MVP.App.Services.Data;
    using MVP.App.Services.Initialization;
    using MVP.App.Services.Input;
    using MVP.App.Services.MvpApi;
    using MVP.App.Services.MvpApi.DataContainers;
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

        public AboutPageViewModel AboutPageViewModel => ServiceLocator.Current.GetInstance<AboutPageViewModel>();

        public ContributionsPageViewModel ContributionsPageViewModel
            => ServiceLocator.Current.GetInstance<ContributionsPageViewModel>();

        private static void RegisterServices()
        {
            SimpleIoc.Default.Register<IMessenger, Messenger>();
            SimpleIoc.Default.Register<KeyboardCharacterService>();
            SimpleIoc.Default.Register(ApiClientProvider.GetClient);
            SimpleIoc.Default.Register<IAppInitializer, AppInitializer>();
            SimpleIoc.Default.Register<IProfileDataContainer, ProfileDataContainer>();
            SimpleIoc.Default.Register<IContributionTypeDataContainer, ContributionTypeContainer>();
            SimpleIoc.Default.Register<IContributionAreaDataContainer, ContributionAreaContainer>();
            SimpleIoc.Default.Register<IDataContainerManager, DataContainerManager>();
            SimpleIoc.Default.Register<IContributionSubmissionService, ContributionSubmissionService>();
        }

        private static void RegisterViewModels()
        {
            SimpleIoc.Default.Register<InitializingPageViewModel>();
            SimpleIoc.Default.Register<AppShellPageViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<ContributionsPageViewModel>();
            SimpleIoc.Default.Register<AboutPageViewModel>();
        }
    }
}