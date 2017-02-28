namespace MVP.App.Services.MvpApi.DataContainers
{
    using System;

    using MVP.Api.Models;
    using MVP.Api.Models.MicrosoftAccount;

    public class ProfileDataContainerWrapper
    {
        public MSACredentials Account { get; set; }

        public MVPProfile Profile { get; set; }

        public string ProfileImage { get; set; }

        public DateTime LastDateChecked { get; set; }
    }
}