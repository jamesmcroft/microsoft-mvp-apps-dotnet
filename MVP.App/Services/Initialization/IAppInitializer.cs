namespace MVP.App.Services.Initialization
{
    using System.Threading.Tasks;

    using MVP.App.Events;

    /// <summary>
    /// Defines an interface for initializing an application.
    /// </summary>
    public interface IAppInitializer
    {
        /// <summary>
        /// Initializes the application asynchronously.
        /// </summary>
        /// <returns>
        /// When this method completes, it returns true if initialized successfully; else false.
        /// </returns>
        Task<bool> InitializeAsync();

        /// <summary>
        /// Authenticates the user with the application asynchronously.
        /// </summary>
        /// <returns>
        /// When this method completes, it returns true if authenticated successfully; else false.
        /// </returns>
        Task<AuthenticationMessage> AuthenticateAsync();
    }
}