namespace MVP.App.Events
{
    public class UpdateBusyIndicatorMessage
    {
        public UpdateBusyIndicatorMessage(bool isBusy)
            : this(isBusy, string.Empty, false)
        {
        }

        public UpdateBusyIndicatorMessage(bool isBusy, string busyMessage)
            : this(isBusy, busyMessage, false)
        {
        }

        public UpdateBusyIndicatorMessage(bool isBusy, string busyMessage, bool isBlocking)
        {
            this.IsBusy = isBusy;
            this.BusyMessage = busyMessage;
            this.IsBlocking = isBlocking;
        }

        public bool IsBlocking { get; }

        public string BusyMessage { get; }

        public bool IsBusy { get; }
    }
}