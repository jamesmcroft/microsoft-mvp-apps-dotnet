namespace MVP.App
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CommonServiceLocator;

    using MVP.Api.Models;
    using MVP.App.Services.MvpApi.DataContainers;

    public static partial class Extensions
    {
        public static ContributionTechnology ToContributionTechnology(this ActivityTechnology technology)
        {
            ContributionTechnology contributionTechnology = new ContributionTechnology
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
            var areaContainer = ServiceLocator.Current.GetInstance<IContributionAreaDataContainer>();
            var areas =
                areaContainer.GetAllAreas()
                    .SelectMany(awardContribution => awardContribution.Areas)
                    .GroupBy(x => x.AwardName)
                    .Select(g => g.First())
                    .SelectMany(a => a.Items)
                    .ToList();

            return areas.FirstOrDefault(x => x.Id == technology.Id);
        }

        /// <summary>
        /// Gets a technology award area by the drill-down name of the specific award area (e.g. Windows App Development would return the Windows Development technology award area)
        /// </summary>
        /// <param name="contributions">
        /// The collection of contributions.
        /// </param>
        /// <param name="areaId">
        /// The drill-down area name.
        /// </param>
        /// <returns>
        /// Returns the activity technology if exists.
        /// </returns>
        public static ActivityTechnology GetActivityTechnologyById(
            this IEnumerable<AwardContribution> contributions,
            Guid areaId)
        {
            return contributions != null
                       ? (from contribution in contributions
                          from area in contribution.Areas
                          from item in area.Items
                          select item).FirstOrDefault(item => item.Id.Equals(areaId))
                       : null;
        }
    }
}