namespace MVP.App.Common.Networking
{
    using Android.Content;
    using Android.Net;

    public class NetworkStatusManager
    {
        private readonly ConnectivityManager connectivityManager;

        public NetworkStatusManager(Context context)
        {
            this.connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
        }

        public bool IsConnected()
        {
            var activeNetwork = this.connectivityManager.ActiveNetworkInfo;
            return activeNetwork != null && activeNetwork.IsConnected;
        }
    }
}