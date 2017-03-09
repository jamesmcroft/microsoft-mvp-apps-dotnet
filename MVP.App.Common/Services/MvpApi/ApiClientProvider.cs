namespace MVP.App.Services.MvpApi
{
    using MVP.Api;

    public static class ApiClientProvider
    {
        private static ApiClient client;

        /// <summary>
        /// Defines the ID of the Live app.
        /// </summary>
        internal const string ClientId = "ClientId";

        /// <summary>
        /// Defines the secret of the Live app.
        /// </summary>
        internal const string ClientSecret = "ClientSecret";

        /// <summary>
        /// Defines the subscription key for the MVP API.
        /// </summary>
        internal const string SubscriptionKey = "SubscriptionKey";

        /// <summary>
        /// Defines a value indicating whether the client ID & secret are for an older Live SDK app.
        /// </summary>
        internal const bool IsLiveSdkApp = false;

        /// <summary>
        /// Gets an instance of the MVP API client that can be used across the application.
        /// </summary>
        /// <returns>
        /// When this method completes, it returns an <see cref="ApiClient"/> object.
        /// </returns>
        public static ApiClient GetClient()
        {
            return client ?? (client = new ApiClient(ClientId, ClientSecret, SubscriptionKey, IsLiveSdkApp));
        }
    }
}