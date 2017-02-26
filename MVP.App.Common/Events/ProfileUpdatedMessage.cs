namespace MVP.App.Events
{
    using MVP.Api.Models;

    public class ProfileUpdatedMessage
    {
        public ProfileUpdatedMessage(MVPProfile profile)
        {
            this.Profile = profile;
        }

        public MVPProfile Profile { get; }
    }
}