namespace MVP.App.Services.MvpApi
{
    using MVP.Api.Models;

    public class ContributionVisibilities
    {
        public static ItemVisibility Public
            => new ItemVisibility { Description = "Everyone", Id = 299600000, LocalizeKey = "PublicVisibilityText" };

        public static ItemVisibility Microsoft
            => new ItemVisibility { Description = "Microsoft", Id = 100000000, LocalizeKey = "MicrosoftVisibilityText" }
        ;

        public static ItemVisibility MVP
            => new ItemVisibility { Description = "MVP Community", Id = 100000001, LocalizeKey = "MVPVisibilityText" };
    }
}