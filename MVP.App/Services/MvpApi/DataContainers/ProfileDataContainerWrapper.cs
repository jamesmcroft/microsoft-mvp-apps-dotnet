namespace MVP.App.Services.MvpApi.DataContainers
{
    using System;

    using MVP.Api.Models;
    using MVP.Api.Models.MicrosoftAccount;

    /// <summary>
    /// Defines a wrapper object for the data from the <see cref="IProfileDataContainer"/>.
    /// </summary>
    public class ProfileDataContainerWrapper
    {
        /// <summary>
        /// Gets or sets the user's Microsoft account.
        /// </summary>
        public MSACredentials Account { get; set; }

        /// <summary>
        /// Gets or sets the user's MVP profile.
        /// </summary>
        public MVPProfile Profile { get; set; }

        /// <summary>
        /// Gets or sets the user's MVP profile image.
        /// </summary>
        public string ProfileImage { get; set; }

        /// <summary>
        /// Gets or sets the date that the container was last checked for an update.
        /// </summary>
        public DateTime LastDateChecked { get; set; }
    }
}