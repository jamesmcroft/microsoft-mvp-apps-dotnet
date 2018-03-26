namespace MVP.App.Events
{
    using MVP.Api.Models;

    /// <summary>
    /// Defines a message for when the user's MVP profile is updated.
    /// </summary>
    public class ProfileUpdatedMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileUpdatedMessage"/> class.
        /// </summary>
        /// <param name="profile">
        /// The user's updated MVP profile.
        /// </param>
        public ProfileUpdatedMessage(MVPProfile profile)
        {
            this.Profile = profile;
        }

        /// <summary>
        /// Gets the user's updated MVP profile.
        /// </summary>
        public MVPProfile Profile { get; }
    }
}