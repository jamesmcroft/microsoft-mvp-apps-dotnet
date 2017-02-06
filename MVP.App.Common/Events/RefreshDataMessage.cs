namespace MVP.App.Events
{
    public class RefreshDataMessage
    {
        public RefreshDataMessage(RefreshDataMode mode)
        {
            this.Mode = mode;
        }

        public RefreshDataMode Mode { get; }
    }
}
