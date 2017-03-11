namespace MVP.App
{
    using Android.App;
    using Android.Content.PM;
    using Android.OS;
    using Android.Views;

    using MVP.App.Common;
    using MVP.App.ViewModels;

    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : BaseActivity<MainActivityViewModel>
    {
        public override MainActivityViewModel ViewModel => App.Application.Locator.MainActivityViewModel;

        public override void OnCreated(Bundle bundle)
        {
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            this.SetContentView(Resource.Layout.Main);
        }
    }
}