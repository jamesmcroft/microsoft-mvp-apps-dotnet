namespace MVP.App.Services.Data
{
    using System.Collections.Generic;

    using MVP.Api.Models;

    public interface IContributionAreaContainer : IServiceDataContainer
    {
        IEnumerable<AwardContribution> GetAllAreas();
    }
}