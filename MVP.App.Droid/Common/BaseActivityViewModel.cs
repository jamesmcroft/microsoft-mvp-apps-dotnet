namespace MVP.App.Common
{
    using Android.OS;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.ServiceLocation;

    public abstract class BaseActivityViewModel : ViewModelBase
    {
        protected BaseActivityViewModel()
            : this(ServiceLocator.Current.GetInstance<IMessenger>())
        {
        }

        [PreferredConstructor]
        protected BaseActivityViewModel(IMessenger messenger)
        {
            this.MessengerInstance = messenger;
        }

        public abstract void OnActivityCreated(Bundle bundle);
    }
}