namespace MVP.App.Services.MvpApi.DataContainers
{
    using System.Threading.Tasks;

    using MVP.Api.Models;
    using MVP.Api.Models.MicrosoftAccount;
    using MVP.App.Services.Data;

    /// <summary>
    /// Defines an interface for handling the MVP profile.
    /// </summary>
    public interface IProfileDataContainer : IDataContainer
    {
        /// <summary>
        /// Gets the MSA credentials associated with current user.
        /// </summary>
        MSACredentials Account { get; }

        /// <summary>
        /// Gets the MVP profile associated with the current user.
        /// </summary>
        MVPProfile Profile { get; }

        /// <summary>
        /// Gets the MVP profile image associated with the current user.
        /// </summary>
        string ProfileImage { get; }

        /// <summary>
        /// Updates the MVP profile in the container asynchronously.
        /// </summary>
        /// <param name="profile">
        /// The updated MVP profile to store.
        /// </param>
        /// <returns>
        /// An object that is used to manage the asynchronous operation.
        /// </returns>
        Task SetProfileAsync(MVPProfile profile);

        /// <summary>
        /// Updates the MSA credentials in the container asynchronously.
        /// </summary>
        /// <param name="account">
        /// The updated MSA credentials to store.
        /// </param>
        /// <returns>
        /// An object that is used to manage the asynchronous operation.
        /// </returns>
        Task SetAccountAsync(MSACredentials account);

        /// <summary>
        /// Updates the MVP profile image in the container asynchronously.
        /// </summary>
        /// <param name="image">
        /// The updated MVP profile image to store.
        /// </param>
        /// <returns>
        /// An object that is used to manage the asynchronous operation.
        /// </returns>
        Task SetProfileImageAsync(string image);
    }
}