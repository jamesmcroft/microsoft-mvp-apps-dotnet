namespace MVP.App.Services.Initialization
{
    using System;
    using System.Threading.Tasks;

    using MVP.App.Common;

    using Windows.ApplicationModel.Activation;

    using WinUX.Input.Speech;

    public static class ActivationLauncher
    {
        public static async Task<bool> RunActivationProcedureAsync(ActivationArgs activationArgs)
        {
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
                return true;
            }

            return false;
        }
    }
}