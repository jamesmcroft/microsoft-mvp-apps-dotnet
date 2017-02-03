namespace MVP.App.Services.Initialization
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines an interface for initializing an application.
    /// </summary>
    public interface IAppInitializer
    {
        /// <summary>
        /// Initializes the application asynchronously.
        /// </summary>
        /// <returns>
        /// Returns true if initialized; else false.
        /// </returns>
        Task<bool> InitializeAsync();
    }
}