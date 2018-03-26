namespace MVP.App.Models.Common
{
    using System.Windows.Input;

    using CommonServiceLocator;

    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using WinUX.MvvmLight.Common.ViewModels;

    public abstract class CustomFlyoutViewModel : CoreViewModelBase
    {
        private bool isFlyoutVisible;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFlyoutViewModel"/> class.
        /// </summary>
        protected CustomFlyoutViewModel()
            : this(ServiceLocator.Current.GetInstance<IMessenger>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFlyoutViewModel"/> class.
        /// </summary>
        /// <param name="messenger">
        /// The MvvmLight messenger.
        /// </param>
        [PreferredConstructor]
        protected CustomFlyoutViewModel(IMessenger messenger)
            : base(messenger)
        {
            this.CloseCommand = new RelayCommand(this.Close);
        }

        /// <summary>
        /// Gets the command for closing the custom flyout.
        /// </summary>
        public ICommand CloseCommand { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the custom flyout is visible.
        /// </summary>
        public bool IsFlyoutVisible
        {
            get => this.isFlyoutVisible;
            set => this.Set(() => this.IsFlyoutVisible, ref this.isFlyoutVisible, value);
        }

        /// <summary>
        /// Shows the custom flyout.
        /// </summary>
        public virtual void Show()
        {
            this.IsFlyoutVisible = true;
        }

        /// <summary>
        /// Closes the custom flyout.
        /// </summary>
        public virtual void Close()
        {
            this.IsFlyoutVisible = false;
        }
    }
}