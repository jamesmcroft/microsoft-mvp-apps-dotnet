namespace MVP.App.UWP.Cortana
{
    using System.Threading.Tasks;

    using Windows.ApplicationModel.VoiceCommands;

    public sealed partial class VoiceCommandService
    {
        private async Task HandleContributionCommand(VoiceCommand command)
        {
            await this.ReportResultAsync(
                VoiceReportResult.Fail,
                $"This feature is not implemented yet. Please check back soon. More info: {this.containerManager.Containers.Count}");
        }
    }
}