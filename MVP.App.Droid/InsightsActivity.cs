using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

using MVP.App.Common;
using MVP.App.ViewModels;

namespace MVP.App
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/Icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public partial class InsightsActivity : BaseActivity<InsightsActivityViewModel>
    {
        public override InsightsActivityViewModel ViewModel => App.Application.Locator.InsightsActivityViewModel;

        public override void OnCreated(Bundle bundle)
        {
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            this.SetContentView(Resource.Layout.Insights);

            this.SetupBindings();
        }
    }
}