namespace MVP.App.Services.Data
{
    using System.Threading.Tasks;

    using MVP.Api.Models;
    using MVP.Api.Models.MicrosoftAccount;

    public interface IProfileDataContainer : IServiceDataContainer
    {
        MSACredentials Account { get; }

        MVPProfile Profile { get; }

        string ProfileImage { get; }

        Task SetProfileAsync(MVPProfile profile);

        Task SetAccountAsync(MSACredentials account);

        Task SetProfileImageAsync(string image);
    }
}