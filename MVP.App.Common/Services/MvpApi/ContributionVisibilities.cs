namespace MVP.App.Services.MvpApi
{
    using System.Collections.Generic;

    using MVP.Api.Models;

    /// <summary>
    /// Defines the common models for the <see cref="ItemVisibility"/> that can be associated with MVP contributions.
    /// </summary>
    public class ContributionVisibilities
    {
        /// <summary>
        /// Gets the public item visibility.
        /// </summary>
        public static ItemVisibility Public
            => new ItemVisibility { Description = "Everyone", Id = 299600000, LocalizeKey = "PublicVisibilityText" };

        /// <summary>
        /// Gets the Microsoft item visibility.
        /// </summary>
        public static ItemVisibility Microsoft
            => new ItemVisibility { Description = "Microsoft", Id = 100000000, LocalizeKey = "MicrosoftVisibilityText" }
        ;

        /// <summary>
        /// Ges the MVP item visibility.
        /// </summary>
        public static ItemVisibility MVP
            => new ItemVisibility { Description = "MVP Community", Id = 100000001, LocalizeKey = "MVPVisibilityText" };

        /// <summary>
        /// Gets a list of the currently available contribution item visibilities.
        /// </summary>
        /// <returns>
        /// When this method completes, it returns a collection of <see cref="ItemVisibility"/> objects.
        /// </returns>
        public static IEnumerable<ItemVisibility> GetItemVisibilities()
        {
            return new List<ItemVisibility> { Public, Microsoft, MVP };
        }
    }
}