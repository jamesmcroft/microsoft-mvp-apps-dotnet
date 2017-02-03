namespace MVP.App.Services.Initialization
{
    using System;
    using System.Threading.Tasks;

    using MVP.Api;

    /// <summary>
    /// Defines a service for initializing an application.
    /// </summary>
    public class AppInitializer : IAppInitializer
    {
        private readonly ApiClient apiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppInitializer"/> class.
        /// </summary>
        /// <param name="apiClient">
        /// The MVP API client.
        /// </param>
        public AppInitializer(ApiClient apiClient)
        {
            if (apiClient == null)
            {
                throw new ArgumentNullException(nameof(apiClient), "The API client cannot be null");
            }

            this.apiClient = apiClient;
        }

        /// <inheritdoc />
        public Task<bool> InitializeAsync()
        {
            return Task.FromResult(true);
        }
    }
}