namespace MVP.App
{
    using Android.App;
    using Android.OS;
    using Android.Views;

    using MVP.App.Common;
    using MVP.App.ViewModels;

    [Activity(Label = "MVP.App.Droid", MainLauncher = true, Icon = "@drawable/Icon")]
    public partial class InitializingActivity : BaseActivity<InitializingActivityViewModel>
    {
        public override InitializingActivityViewModel ViewModel => App.Application.Locator.InitializingActivityViewModel
        ;

        public override void OnCreated(Bundle bundle)
        {
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            this.SetContentView(Resource.Layout.Initializing);

            this.SetupBindings();
        }
    }
}