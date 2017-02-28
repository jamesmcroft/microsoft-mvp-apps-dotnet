namespace MVP.App.Services.MvpApi
{
    using System.Threading.Tasks;

    using MVP.Api.Models;

    public interface IContributionSubmissionService
    {
        Task<bool> SubmitContributionAsync(Contribution contribution);
    }
}