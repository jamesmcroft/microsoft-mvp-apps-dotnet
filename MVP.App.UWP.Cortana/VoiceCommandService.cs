namespace MVP.App.UWP.Cortana
{
    using System;
    using System.Threading.Tasks;

    using MVP.Api;
    using MVP.App.Services.Data;
    using MVP.App.Services.MvpApi;
    using MVP.App.Services.MvpApi.DataContainers;

    using Windows.ApplicationModel.AppService;
    using Windows.ApplicationModel.Background;
    using Windows.ApplicationModel.VoiceCommands;

    public sealed partial class VoiceCommandService : IBackgroundTask
    {
        private VoiceCommandServiceConnection voiceServiceConnection;

        private BackgroundTaskDeferral serviceDeferral;

        private ApiClient apiClient;

        private DataContainerManager containerManager;
        private ContributionTypeContainer typeContainer;
        private ContributionAreaContainer areaContainer;
        private ContributionSubmissionService contributionSubmissionService;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            this.serviceDeferral = taskInstance.GetDeferral();
            taskInstance.Canceled += this.OnTaskCanceled;

            await this.InitializeServicesAsync();

            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            if (triggerDetails != null && triggerDetails.Name == nameof(VoiceCommandService))
            {
                try
                {
                    this.voiceServiceConnection =
                        VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);

                    this.voiceServiceConnection.VoiceCommandCompleted += this.OnVoiceCommandCompleted;

                    var voiceCommand = await this.voiceServiceConnection.GetVoiceCommandAsync();

                    if (voiceCommand.CommandName.ToLower().Contains("contribution"))
                    {
                        await this.LaunchAppForVoiceAsync("contribution");

                        // This doesn't seem possible at the moment due to lack of communication logic with Cortana (sad times)
                        // await this.HandleContributionCommand(voiceCommand);
                    }
                    else
                    {
                        await this.LaunchAppForVoiceAsync();
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
#endif
                }
            }
        }

        private async Task InitializeServicesAsync()
        {
            if (this.apiClient == null)
            {
                this.apiClient = ApiClientProvider.GetClient();
            }

            if (this.typeContainer == null)
            {
                this.typeContainer = new ContributionTypeContainer(this.apiClient);
            }

            if (this.areaContainer == null)
            {
                this.areaContainer = new ContributionAreaContainer(this.apiClient);
            }

            if (contributionSubmissionService == null)
            {
                contributionSubmissionService = new ContributionSubmissionService(this.apiClient);
            }

            if (this.containerManager == null)
            {
                this.containerManager = new DataContainerManager(
                    this.typeContainer,
                    this.areaContainer);
            }

            try
            {
                await this.containerManager.LoadAsync();
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }
        }

        private async Task LaunchAppForVoiceAsync()
        {
            await this.LaunchAppForVoiceAsync(string.Empty);
        }

        private async Task LaunchAppForVoiceAsync(string v)
        {
            var message = new VoiceCommandUserMessage {SpokenMessage = "Opening the MVP community app"};
            var response = VoiceCommandResponse.CreateResponse(message);
            response.AppLaunchArgument = v;

            await this.voiceServiceConnection.RequestAppLaunchAsync(response);
        }

        private void OnVoiceCommandCompleted(VoiceCommandServiceConnection sender, VoiceCommandCompletedEventArgs args)
        {
            this.serviceDeferral?.Complete();
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            this.serviceDeferral?.Complete();
        }
    }
}