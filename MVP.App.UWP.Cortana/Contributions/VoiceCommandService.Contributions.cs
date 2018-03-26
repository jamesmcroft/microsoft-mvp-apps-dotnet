namespace MVP.App.UWP.Cortana
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MVP.Api.Models;

    using Windows.ApplicationModel.VoiceCommands;

    public sealed partial class VoiceCommandService
    {
        private async Task HandleContributionCommand(VoiceCommand command)
        {
            // Step - Request type
            VoiceCommandDisambiguationResult typeResponse = await ReportForContributionTypeAsync();
            if (typeResponse != null)
            {
                VoiceCommandContentTile selectedType = typeResponse.SelectedItem;
                ContributionType type = selectedType?.AppContext as ContributionType;
                if (type != null)
                {
                    // Step - Request area
                    VoiceCommandDisambiguationResult areaResponse = await this.ReportForContributionAreaAsync();
                    if (areaResponse != null)
                    {
                        VoiceCommandContentTile selectedArea = areaResponse.SelectedItem;
                        ActivityTechnology area = selectedArea?.AppContext as ActivityTechnology;

                        if (area != null)
                        {
                            // Step - Request title (Blocked as this doesn't appear possible... sad times)
                            VoiceCommandDisambiguationResult titleResponse = await this.ReportForContributionTitleAsync();
                            if (titleResponse != null)
                            {

                                await this.ReportResultAsync(
                                    VoiceReportResult.Success,
                                    "Your contribution has been submitted!");
                                return;
                            }
                        }
                    }
                }
            }

            await this.ReportResultAsync(
                VoiceReportResult.Fail,
                "Failed to find contribution types. Open the app to update them.");
        }

        private async Task<VoiceCommandDisambiguationResult> ReportForContributionTitleAsync()
        {
            VoiceCommandUserMessage userPrompt = new VoiceCommandUserMessage();
            userPrompt.DisplayMessage = userPrompt.SpokenMessage = "What is the title of this contribution?";

            VoiceCommandUserMessage repeatPrompt = new VoiceCommandUserMessage();
            repeatPrompt.DisplayMessage = repeatPrompt.SpokenMessage = "Sorry, what would you like the title to be?";

            VoiceCommandResponse response = VoiceCommandResponse.CreateResponseForPrompt(userPrompt, repeatPrompt);

            return await voiceServiceConnection.RequestDisambiguationAsync(response);
        }

        private async Task<VoiceCommandDisambiguationResult> ReportForContributionAreaAsync()
        {
            List<ActivityTechnology> allAreas = this.areaContainer.GetMyAreaTechnologies().ToList();
            List<ActivityTechnology> areas = allAreas.Count > 10 ? allAreas.Take(10).ToList() : allAreas;

            if (areas.Any())
            {
                VoiceCommandUserMessage userPrompt = new VoiceCommandUserMessage();
                userPrompt.DisplayMessage = userPrompt.SpokenMessage = "Which area is it?";

                VoiceCommandUserMessage repeatPrompt = new VoiceCommandUserMessage();
                repeatPrompt.DisplayMessage = repeatPrompt.SpokenMessage = "Sorry, which area is it?";

                List<VoiceCommandContentTile> areaTiles = areas.Select(area => new VoiceCommandContentTile
                {
                    ContentTileType = VoiceCommandContentTileType.TitleOnly,
                    AppContext = area,
                    Title = area.Name
                }).ToList();

                VoiceCommandResponse response = VoiceCommandResponse.CreateResponseForPrompt(userPrompt, repeatPrompt, areaTiles);

                return await voiceServiceConnection.RequestDisambiguationAsync(response);
            }

            return null;
        }

        private async Task<VoiceCommandDisambiguationResult> ReportForContributionTypeAsync()
        {
            List<ContributionType> types = this.typeContainer.GetCommonTypes().ToList();

            if (types.Any())
            {
                VoiceCommandUserMessage userPrompt = new VoiceCommandUserMessage();
                userPrompt.DisplayMessage = userPrompt.SpokenMessage = "What type of contribution is it?";

                VoiceCommandUserMessage repeatPrompt = new VoiceCommandUserMessage();
                repeatPrompt.DisplayMessage = repeatPrompt.SpokenMessage = "Sorry, what type of contribution is it?";

                List<VoiceCommandContentTile> typeTiles = types.Select(type => new VoiceCommandContentTile
                {
                    ContentTileType = VoiceCommandContentTileType.TitleOnly,
                    AppContext = type,
                    Title = type.Name
                }).ToList();

                VoiceCommandResponse response = VoiceCommandResponse.CreateResponseForPrompt(userPrompt, repeatPrompt, typeTiles);

                return await voiceServiceConnection.RequestDisambiguationAsync(response);
            }

            return null;
        }
    }
}