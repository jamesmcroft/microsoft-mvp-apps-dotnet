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

        public ContributionsPageViewModel ContributionsPageViewModel
            => ServiceLocator.Current.GetInstance<ContributionsPageViewModel>();

        private static void RegisterServices()
        {
            SimpleIoc.Default.Register<IMessenger, Messenger>();
            SimpleIoc.Default.Register<KeyboardCharacterService>();
            SimpleIoc.Default.Register(
                            () =>
                                new ApiClient(
                                    "00000000481CAF70",
                                    "mciGV3jyNrCnbB9anpU2moN",
                                    "151ee67f81c142f2a51edd318dc5be7d",
                                    true));
            SimpleIoc.Default.Register<IAppInitializer, AppInitializer>();
            SimpleIoc.Default.Register<IProfileDataContainer, ProfileDataContainer>();
            SimpleIoc.Default.Register<IContributionTypeContainer, ContributionTypeContainer>();
            SimpleIoc.Default.Register<IContributionAreaContainer, ContributionAreaContainer>();
            SimpleIoc.Default.Register<IServiceDataContainerManager, ServiceDataContainerManager>();
            SimpleIoc.Default.Register<IContributionSubmissionService, ContributionSubmissionService>();
        }

        private static void RegisterViewModels()
        {
            SimpleIoc.Default.Register<InitializingPageViewModel>();
            SimpleIoc.Default.Register<AppShellPageViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<ContributionsPageViewModel>();
        }
    }
}