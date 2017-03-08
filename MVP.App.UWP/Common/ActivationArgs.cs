namespace MVP.App.Common
{
    using System;

    using Windows.ApplicationModel.Activation;

    using WinUX.Input.Speech;

    /// <summary>
    /// Defines arguments for handling application activation.
    /// </summary>
    public class ActivationArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationArgs"/> class for a speech activation.
        /// </summary>
        /// <param name="speechCommand">
        /// The speech command.
        /// </param>
        public ActivationArgs(SpeechCommand speechCommand)
        {
            this.ActivationKind = ActivationKind.VoiceCommand;
            this.SpeechCommand = speechCommand;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationArgs"/> class for a protocol activation.
        /// </summary>
        /// <param name="protocolUri">
        /// The protocol uri.
        /// </param>
        public ActivationArgs(Uri protocolUri)
        {
            this.ActivationKind = ActivationKind.Protocol;
            this.ProtocolUri = protocolUri;
        }

        /// <summary>
        /// Gets the URI associated with the protocol activation.
        /// </summary>
        public Uri ProtocolUri { get; }

        /// <summary>
        /// Gets the <see cref="SpeechCommand"/> associated with the speech activation.
        /// </summary>
        public SpeechCommand SpeechCommand { get; }

        /// <summary>
        /// Gets the type of activation that occurred.
        /// </summary>
        public ActivationKind ActivationKind { get; }
    }
}