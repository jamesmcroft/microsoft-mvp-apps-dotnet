namespace MVP.App.Data
{
    using System.Threading.Tasks;

    using MVP.Api.Models;
    using MVP.Api.Models.MicrosoftAccount;

    public interface IProfileData
    {
        MSACredentials CurrentAccount { get; }

        MVPProfile CurrentProfile { get; }

        string CurrentProfileImage { get; }

        Task UpdateAccountAsync(MSACredentials credentials);

        Task UpdateProfileAsync(MVPProfile profile);

        Task UpdateProfileImageAsync(string profileImage);

        Task LoadAsync();

        Task SaveAsync();
    }
}