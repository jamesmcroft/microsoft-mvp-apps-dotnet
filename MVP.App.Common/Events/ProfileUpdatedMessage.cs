namespace MVP.App.Events
{
    using System;

    using MVP.Api.Models;

    public class ProfileUpdatedMessage
    {
        public ProfileUpdatedMessage(MVPProfile profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile", "The MVP profile cannot be null.");
            }
            this.Profile = profile;
        }

        public MVPProfile Profile { get; }
    }
}