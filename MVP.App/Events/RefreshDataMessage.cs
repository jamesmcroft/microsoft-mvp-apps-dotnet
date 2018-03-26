namespace MVP.App.Events
{
    /// <summary>
    /// Defines a message to cause a refresh to occur.
    /// </summary>
    public class RefreshDataMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshDataMessage"/> class.
        /// </summary>
        /// <param name="mode">
        /// The context which is expected to be refreshed.
        /// </param>
        public RefreshDataMessage(RefreshDataMode mode)
        {
            this.Mode = mode;
        }

        /// <summary>
        /// Gets the expected refresh context.
        /// </summary>
        public RefreshDataMode Mode { get; }
    }
}