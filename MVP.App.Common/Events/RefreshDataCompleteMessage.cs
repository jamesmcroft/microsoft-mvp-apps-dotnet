namespace MVP.App.Events
{
    public class RefreshDataCompleteMessage
    {
        public RefreshDataCompleteMessage(bool isComplete)
        {
            this.IsComplete = isComplete;
        }

        public bool IsComplete { get; }
    }
}