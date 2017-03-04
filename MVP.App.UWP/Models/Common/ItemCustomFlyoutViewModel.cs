namespace MVP.App.Models.Common
{
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.ServiceLocation;

    using Windows.System;
    using Windows.UI.Core;

    using WinUX;

    public abstract class ItemCustomFlyoutViewModel<TItem> : CustomFlyoutViewModel
    {
        private TItem item;

        private string title;

        private bool isInEdit;

        private bool canDelete;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCustomFlyoutViewModel{TItem}"/> class.
        /// </summary>
        protected ItemCustomFlyoutViewModel()
            : this(ServiceLocator.Current.GetInstance<IMessenger>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCustomFlyoutViewModel{TItem}"/> class.
        /// </summary>
        /// <param name="messenger">
        /// The MvvmLight messenger.
        /// </param>
        [PreferredConstructor]
        protected ItemCustomFlyoutViewModel(IMessenger messenger)
            : base(messenger)
        {
            this.EditCommand = new RelayCommand(() => this.IsInEdit = true);
            this.DeleteCommand = new RelayCommand(this.Delete);
        }

        public TItem Item
        {
            get
            {
                return this.item;
            }
            set
            {
                this.Set(() => this.Item, ref this.item, value);
            }
        }

        public ICommand EditCommand { get; }

        public ICommand DeleteCommand { get; }

        /// <summary>
        /// Gets or sets the title of the fly-out.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.Set(() => this.Title, ref this.title, value);
            }
        }

        public bool IsInEdit
        {
            get
            {
                return this.isInEdit;
            }
            set
            {
                this.Set(() => this.IsInEdit, ref this.isInEdit, value);
            }
        }

        public bool CanDelete
        {
            get
            {
                return this.canDelete;
            }
            set
            {
                this.Set(() => this.CanDelete, ref this.canDelete, value);
            }
        }

        /// <summary>
        /// Shows the custom flyout.
        /// </summary>
        /// <param name="itemToShow">
        /// The item to show in the custom flyout.
        /// </param>
        public void Show(TItem itemToShow)
        {
            this.Close();

            if (itemToShow != null)
            {
                this.Item = itemToShow;
                this.Show();
            }

            this.MessengerInstance.Register<CharacterReceivedEventArgs>(
                this,
                args =>
                    {
                        if (args.VirtualKeyReceived() == VirtualKey.Escape)
                        {
                            this.Close();
                        }
                    });
        }

        public virtual void Delete()
        {
        }

        /// <inheritdoc />
        public override void Close()
        {
            base.Close();

            this.Item = default(TItem);

            this.MessengerInstance.Unregister<CharacterReceivedEventArgs>(this);
        }
    }
}