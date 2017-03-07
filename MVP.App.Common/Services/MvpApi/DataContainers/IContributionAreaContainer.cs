namespace MVP.App.Services.MvpApi.DataContainers
{
    using System.Collections.Generic;

    using MVP.Api.Models;
    using MVP.App.Services.Data;

    public interface IContributionAreaContainer : IServiceDataContainer
    {
        IEnumerable<AwardContribution> GetAllAreas();

        IEnumerable<ActivityTechnology> GetMyAreaTechnologies();
    }
}