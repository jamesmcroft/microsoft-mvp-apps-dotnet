using System;
using System.Linq;
using MVP.Api.Models;

namespace MVP.App.UWP.Cortana
{
    using System.Threading.Tasks;

    using Windows.ApplicationModel.VoiceCommands;

    public sealed partial class VoiceCommandService
    {
        private async Task HandleContributionCommand(VoiceCommand command)
        {
            // Step - Request type
            var typeResponse = await ReportForContributionTypeAsync();
            if (typeResponse != null)
            {
                var selectedType = typeResponse.SelectedItem;
                var type = selectedType?.AppContext as ContributionType;
                if (type != null)
                {
                    // Step - Request area
                    var areaResponse = await this.ReportForContributionAreaAsync();
                    if (areaResponse != null)
                    {
                        var selectedArea = areaResponse.SelectedItem;
                        var area = selectedArea?.AppContext as ActivityTechnology;

                        if (area != null)
                        {
                            // Step - Request title (Blocked as this doesn't appear possible... sad times)
                            var titleResponse = await this.ReportForContributionTitleAsync();
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
            var userPrompt = new VoiceCommandUserMessage();
            userPrompt.DisplayMessage = userPrompt.SpokenMessage = "What is the title of this contribution?";

            var repeatPrompt = new VoiceCommandUserMessage();
            repeatPrompt.DisplayMessage = repeatPrompt.SpokenMessage = "Sorry, what would you like the title to be?";

            var response = VoiceCommandResponse.CreateResponseForPrompt(userPrompt, repeatPrompt);

            return await voiceServiceConnection.RequestDisambiguationAsync(response);
        }

        private async Task<VoiceCommandDisambiguationResult> ReportForContributionAreaAsync()
        {
            var allAreas = this.areaContainer.GetMyAreaTechnologies().ToList();
            var areas = allAreas.Count > 10 ? allAreas.Take(10).ToList() : allAreas;

            if (areas.Any())
            {
                var userPrompt = new VoiceCommandUserMessage();
                userPrompt.DisplayMessage = userPrompt.SpokenMessage = "Which area is it?";

                var repeatPrompt = new VoiceCommandUserMessage();
                repeatPrompt.DisplayMessage = repeatPrompt.SpokenMessage = "Sorry, which area is it?";

                var areaTiles = areas.Select(area => new VoiceCommandContentTile
                {
                    ContentTileType = VoiceCommandContentTileType.TitleOnly,
                    AppContext = area,
                    Title = area.Name
                }).ToList();

                var response = VoiceCommandResponse.CreateResponseForPrompt(userPrompt, repeatPrompt, areaTiles);

                return await voiceServiceConnection.RequestDisambiguationAsync(response);
            }

            return null;
        }

        private async Task<VoiceCommandDisambiguationResult> ReportForContributionTypeAsync()
        {
            var types = this.typeContainer.GetCommonTypes().ToList();

            if (types.Any())
            {
                var userPrompt = new VoiceCommandUserMessage();
                userPrompt.DisplayMessage = userPrompt.SpokenMessage = "What type of contribution is it?";

                var repeatPrompt = new VoiceCommandUserMessage();
                repeatPrompt.DisplayMessage = repeatPrompt.SpokenMessage = "Sorry, what type of contribution is it?";

                var typeTiles = types.Select(type => new VoiceCommandContentTile
                {
                    ContentTileType = VoiceCommandContentTileType.TitleOnly,
                    AppContext = type,
                    Title = type.Name
                }).ToList();

                var response = VoiceCommandResponse.CreateResponseForPrompt(userPrompt, repeatPrompt, typeTiles);

                return await voiceServiceConnection.RequestDisambiguationAsync(response);
            }

            return null;
        }
    }
}