namespace MVP.App.Services.Initialization
{
    /// <summary>
    /// Defines a messenger event argument for messages from the <see cref="IAppInitializer"/>.
    /// </summary>
    public class AppInitializerMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppInitializerMessage"/> class.
        /// </summary>
        /// <param name="message">
        /// The message text.
        /// </param>
        public AppInitializerMessage(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets the message text.
        /// </summary>
        public string Message { get; }
    }
}