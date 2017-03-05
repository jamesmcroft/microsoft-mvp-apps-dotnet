namespace MVP.App.Common
{
    using System;

    using Windows.ApplicationModel.Activation;

    using WinUX.Input.Speech;

    public class ActivationArgs
    {
        public ActivationArgs(SpeechCommand speechCommand)
        {
            this.ActivationKind = ActivationKind.VoiceCommand;
            this.SpeechCommand = speechCommand;
        }

        public ActivationArgs(Uri protocolUri)
        {
            this.ActivationKind = ActivationKind.Protocol;
            this.ProtocolUri = protocolUri;
        }

        public Uri ProtocolUri { get; }

        public SpeechCommand SpeechCommand { get; }

        public ActivationKind ActivationKind { get; }
    }
}