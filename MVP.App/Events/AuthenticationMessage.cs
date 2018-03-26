namespace MVP.App.Events
{
    /// <summary>
    /// Defines a message for authentication success.
    /// </summary>
    public class AuthenticationMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationMessage"/> class.
        /// </summary>
        /// <param name="isSuccess">
        /// A value indicating whether the authentication was successful.
        /// </param>
        /// <param name="errorMessage">
        /// The associated error message if the authentication was not successful.
        /// </param>
        public AuthenticationMessage(bool isSuccess, string errorMessage)
        {
            this.IsSuccess = isSuccess;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets a value indicating whether the authentication was successful.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets the associated error message if the authentication was not successful.
        /// </summary>
        public string ErrorMessage { get; }
    }
}