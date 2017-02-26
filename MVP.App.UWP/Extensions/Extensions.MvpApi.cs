namespace MVP.App
{
    using System.Linq;

    using Microsoft.Practices.ServiceLocation;

    using MVP.Api.Models;
    using MVP.App.Data;

    public static partial class Extensions
    {
        public static ContributionTechnology ToContributionTechnology(this ActivityTechnology technology)
        {
            var contributionTechnology = new ContributionTechnology
                                             {
                                                 Id = technology.Id,
                                                 AwardCategory = technology.AwardCategory,
                                                 AwardName = technology.AwardName,
                                                 Name = technology.Name
                                             };
            return contributionTechnology;
        }

        public static ActivityTechnology ToActivityTechnology(this ContributionTechnology technology)
        {
            var areaContainer = ServiceLocator.Current.GetInstance<IContributionAreaContainer>();
            var areas =
                areaContainer.GetAllAreas()
                    .SelectMany(awardContribution => awardContribution.Areas)
                    .GroupBy(x => x.AwardName)
                    .Select(g => g.First())
                    .SelectMany(a => a.Items)
                    .ToList();

            return areas.FirstOrDefault(x => x.Id == technology.Id);
        }
    }
}