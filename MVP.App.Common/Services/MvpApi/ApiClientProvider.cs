namespace MVP.App.Services.MvpApi
{
    using MVP.Api;

    public static class ApiClientProvider
    {
        private static ApiClient client;

        internal const string ClientId = "ClientId";

        internal const string ClientSecret = "ClientSecret";

        internal const string SubscriptionKey = "SubscriptionKey";

        internal const bool IsLiveSdkApp = false;

        public static ApiClient GetClient()
        {
            return client ?? (client = new ApiClient(ClientId, ClientSecret, SubscriptionKey, IsLiveSdkApp));
        }
    }
}
