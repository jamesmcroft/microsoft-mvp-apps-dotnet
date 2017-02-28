namespace MVP.App.Services.MvpApi.DataContainers
{
    using System.Threading.Tasks;

    using MVP.Api.Models;
    using MVP.Api.Models.MicrosoftAccount;
    using MVP.App.Services.Data;

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