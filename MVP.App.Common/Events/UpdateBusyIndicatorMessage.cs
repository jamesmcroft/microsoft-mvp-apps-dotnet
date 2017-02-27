namespace MVP.App.Events
{
    public class UpdateBusyIndicatorMessage
    {
        public UpdateBusyIndicatorMessage(bool isBusy, string busyMessage)
        {
            this.IsBusy = isBusy;
            this.BusyMessage = busyMessage;
        }

        public string BusyMessage { get; }

        public bool IsBusy { get; }
    }
}