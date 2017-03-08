namespace MVP.App.Events
{
    /// <summary>
    /// Defines a message to update the application's busy indicator.
    /// </summary>
    public class UpdateBusyIndicatorMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateBusyIndicatorMessage"/> class.
        /// </summary>
        /// <param name="isBusy">
        /// A value indicating whether the busy indicator should be displayed.
        /// </param>
        public UpdateBusyIndicatorMessage(bool isBusy)
            : this(isBusy, string.Empty, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateBusyIndicatorMessage"/> class.
        /// </summary>
        /// <param name="isBusy">
        /// A value indicating whether the busy indicator should be displayed.
        /// </param>
        /// <param name="busyMessage">
        /// The message associated with the current busy process.
        /// </param>
        public UpdateBusyIndicatorMessage(bool isBusy, string busyMessage)
            : this(isBusy, busyMessage, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateBusyIndicatorMessage"/> class.
        /// </summary>
        /// <param name="isBusy">
        /// A value indicating whether the busy indicator should be displayed.
        /// </param>
        /// <param name="busyMessage">
        /// The message associated with the current busy process.
        /// </param>
        /// <param name="isBlocking">
        /// A value indicating whether the busy indicator should block the user from application interaction.
        /// </param>
        public UpdateBusyIndicatorMessage(bool isBusy, string busyMessage, bool isBlocking)
        {
            this.IsBusy = isBusy;
            this.BusyMessage = busyMessage;
            this.IsBlocking = isBlocking;
        }

        /// <summary>
        /// Gets a value indicating whether the busy indicator should block the user from application interaction.
        /// </summary>
        public bool IsBlocking { get; }

        /// <summary>
        /// Gets the message associated with the current busy process.
        /// </summary>
        public string BusyMessage { get; }

        /// <summary>
        /// Gets a value indicating whether the busy indicator should be displayed.
        /// </summary>
        public bool IsBusy { get; }
    }
}