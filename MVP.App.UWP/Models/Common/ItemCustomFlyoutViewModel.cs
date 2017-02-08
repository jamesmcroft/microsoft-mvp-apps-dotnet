namespace MVP.App.Models.Common
{
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.ServiceLocation;

    public abstract class ItemCustomFlyoutViewModel<TItem> : CustomFlyoutViewModel
    {
        private TItem item;

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

        public void Show(TItem itemToShow)
        {
            this.Close();

            if (itemToShow != null)
            {
                this.Item = itemToShow;
                this.Show();
            }
        }

        public override void Close()
        {
            base.Close();

            this.Item = default(TItem);
        }
    }
}