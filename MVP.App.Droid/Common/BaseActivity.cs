namespace MVP.App.Common
{
    using Android.OS;
    using Android.Support.V7.App;

    public abstract class BaseActivity<TViewModel> : AppCompatActivity
        where TViewModel : BaseActivityViewModel
    {
        public abstract TViewModel ViewModel { get; }

        /// <inheritdoc />
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.OnCreated(bundle);
            this.ViewModel?.OnActivityCreated(bundle);
        }

        public abstract void OnCreated(Bundle bundle);
    }
}