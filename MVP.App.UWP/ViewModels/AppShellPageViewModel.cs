namespace MVP.App.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    using GalaSoft.MvvmLight.Messaging;

    using MVP.App.Events;
    using MVP.App.Views;

    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    using WinUX.Common;
    using WinUX.MvvmLight.Xaml.Views;

    public class AppShellPageViewModel : PageBaseViewModel
    {
        private const string MyProfileTag = "profile";

        private const string AllContributionsTag = "contributions";

        private const string AboutAppTag = "about";

        private bool isPaneOpen;

        private bool isBusyMessageVisible;

        private string busyMessage;

        private bool isBusyMessageBlocking;

        private NavigationViewItem selectedNavigationItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppShellPageViewModel"/> class.
        /// </summary>
        /// <param name="messenger">
        /// The MvvmLight messenger.
        /// </param>
        public AppShellPageViewModel(IMessenger messenger)
            : base(messenger)
        {
            this.InitializeNavigationItems();

            this.PropertyChanged += this.OnPropertyChanged;
            this.NavigationService.Frame.Navigated += this.OnFrameNavigated;

            this.MessengerInstance.Register<UpdateBusyIndicatorMessage>(
                this,
                x => this.UpdateBusyIndicator(x.IsBusy, x.BusyMessage, x.IsBlocking));
        }

        public ObservableCollection<NavigationViewItem> NavigationItems { get; private set; }

        public NavigationViewItem SelectedNavigationItem
        {
            get => this.selectedNavigationItem;
            set => this.Set(() => this.SelectedNavigationItem, ref this.selectedNavigationItem, value);
        }

        public bool IsPaneOpen
        {
            get => this.isPaneOpen;
            set => this.Set(() => this.IsPaneOpen, ref this.isPaneOpen, value);
        }

        public string BusyMessage
        {
            get => this.busyMessage;
            set => this.Set(() => this.BusyMessage, ref this.busyMessage, value);
        }

        public bool IsBusyMessageBlocking
        {
            get => this.isBusyMessageBlocking;
            set => this.Set(() => this.IsBusyMessageBlocking, ref this.isBusyMessageBlocking, value);
        }

        public bool IsBusyMessageVisible
        {
            get => this.isBusyMessageVisible;
            set => this.Set(() => this.IsBusyMessageVisible, ref this.isBusyMessageVisible, value);
        }

        public void UpdateBusyIndicator(string message)
        {
            this.UpdateBusyIndicator(true, message);
        }

        public void UpdateBusyIndicator(bool show, string message)
        {
            this.UpdateBusyIndicator(show, message, false);
        }

        public void UpdateBusyIndicator(bool show, string message, bool isBlocking)
        {
            if (show && !string.IsNullOrWhiteSpace(message))
            {
                if (this.IsBusyMessageVisible)
                {
                    return;
                }

                this.BusyMessage = message;
                this.IsBusyMessageBlocking = isBlocking;
                this.IsBusyMessageVisible = true;
            }
            else
            {
                this.IsBusyMessageVisible = false;
                this.BusyMessage = string.Empty;
                this.IsBusyMessageBlocking = false;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.SelectedNavigationItem))
            {
                this.UpdateFrame();
            }
        }

        private void UpdateFrame()
        {
            if (this.SelectedNavigationItem == null)
            {
                return;
            }

            if (this.SelectedNavigationItem.Tag.Equals(MyProfileTag))
            {
                if (this.NavigationService.CurrentPageType != typeof(MainPage))
                {
                    this.NavigationService.Navigate(typeof(MainPage));
                }
            }
            else if (this.SelectedNavigationItem.Tag.Equals(AllContributionsTag))
            {
                if (this.NavigationService.CurrentPageType != typeof(ContributionsPage))
                {
                    this.NavigationService.Navigate(typeof(ContributionsPage));
                }
            }
            else if (this.SelectedNavigationItem.Tag.Equals(AboutAppTag))
            {
                if (this.NavigationService.CurrentPageType != typeof(AboutPage))
                {
                    this.NavigationService.Navigate(typeof(AboutPage));
                }
            }
        }

        private void InitializeNavigationItems()
        {
            if (this.NavigationItems == null)
            {
                this.NavigationItems = new ObservableCollection<NavigationViewItem>
                                           {
                                               new NavigationViewItem()
                                                   {
                                                       Content
                                                           = "My profile",
                                                       Icon
                                                           = new
                                                               SymbolIcon(
                                                                   Symbol
                                                                       .Contact),
                                                       Tag
                                                           = MyProfileTag
                                                   },
                                               new NavigationViewItem()
                                                   {
                                                       Content
                                                           = "All contributions",
                                                       Icon
                                                           = new
                                                               SymbolIcon(
                                                                   Symbol
                                                                       .Library),
                                                       Tag
                                                           = AllContributionsTag
                                                   },
                                               new NavigationViewItem()
                                                   {
                                                       Content
                                                           = "About app",
                                                       Icon
                                                           = new
                                                               SymbolIcon(
                                                                   Symbol
                                                                       .Help),
                                                       Tag
                                                           = AboutAppTag
                                                   }
                                           };
            }
        }

        private void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(MainPage))
            {
                this.SelectedNavigationItem = this.NavigationItems.FirstOrDefault(x => x.Tag.Equals(MyProfileTag));
            }
            else if (e.SourcePageType == typeof(ContributionsPage))
            {
                this.SelectedNavigationItem =
                    this.NavigationItems.FirstOrDefault(x => x.Tag.Equals(AllContributionsTag));
            }
            else if (e.SourcePageType == typeof(AboutPage))
            {
                this.SelectedNavigationItem = this.NavigationItems.FirstOrDefault(x => x.Tag.Equals(AboutAppTag));
            }
            else
            {
                this.SelectedNavigationItem = null;
            }
        }

        public override void OnPageNavigatedTo(NavigationEventArgs args)
        {
            if (args.NavigationMode != NavigationMode.New && args.NavigationMode != NavigationMode.Forward)
            {
                return;
            }

            if (!(args.Parameter is bool))
            {
                return;
            }

            bool shouldNavigateToHome = ParseHelper.SafeParseBool(args.Parameter);
            if (!shouldNavigateToHome)
            {
                // First page navigation is being handled by another provider.
                return;
            }

            this.NavigationService.ClearNavigationHistory();
            this.NavigationService.Navigate(typeof(MainPage));
        }

        public override void OnPageNavigatedFrom(NavigationEventArgs args)
        {
        }

        public override void OnPageNavigatingFrom(NavigatingCancelEventArgs args)
        {
        }
    }
}