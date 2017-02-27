namespace MVP.App.Events
{
    public class AuthenticationMessage
    {
        public AuthenticationMessage(bool isSuccess, string errorMessage)
        {
            this.IsSuccess = isSuccess;
            this.ErrorMessage = errorMessage;
        }

        public bool IsSuccess { get; }

        public string ErrorMessage { get; }
    }
}