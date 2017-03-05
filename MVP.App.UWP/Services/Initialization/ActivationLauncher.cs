namespace MVP.App.Services.Initialization
{
    using System;
    using System.Threading.Tasks;

    using MVP.App.Common;

    using Windows.ApplicationModel.Activation;

    using MVP.App.Models;
    using MVP.App.Views;

    using WinUX;
    using WinUX.Input.Speech;
    using WinUX.Mvvm.Services;

    public static class ActivationLauncher
    {
        public static async Task<bool> RunActivationProcedureAsync(ActivationArgs activationArgs)
        {
            if (activationArgs == null)
            {
                return false;
            }

            switch (activationArgs.ActivationKind)
            {
                case ActivationKind.Protocol:
                    return await ActivateForProtocolAsync(activationArgs.ProtocolUri);
                case ActivationKind.VoiceCommand:
                    return await ActivateForVoiceAsync(activationArgs.SpeechCommand);
            }

            return false;
        }

        private static async Task<bool> ActivateForVoiceAsync(SpeechCommand activationSpeechCommand)
        {
            await Task.Delay(1);

            if (activationSpeechCommand != null)
            {
                return true;
            }

            return false;

        }

        private static async Task<bool> ActivateForProtocolAsync(Uri activationProtocolUri)
        {
            await Task.Delay(1);

            if (activationProtocolUri != null)
            {
                if (activationProtocolUri.Host.Equals("contribution"))
                {
                    var contribution = new ContributionViewModel();
                    contribution.Populate(activationProtocolUri);

                    return NavigationService.Current.Navigate(typeof(ContributionsPage), contribution);
                }
            }

            return false;
        }
    }
}