namespace MVP.App.Services.Input
{
    using System;

    using GalaSoft.MvvmLight.Messaging;

    using Windows.UI.Core;
    using Windows.UI.Xaml;

    public class KeyboardCharacterService
    {
        private readonly IMessenger messenger;

        public KeyboardCharacterService(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        public void Start()
        {
            Window.Current.CoreWindow.CharacterReceived += this.OnCharacterReceived;
        }

        private void OnCharacterReceived(CoreWindow sender, CharacterReceivedEventArgs args)
        {
            this.messenger.Send(args);
        }

        public void Stop()
        {
            Window.Current.CoreWindow.CharacterReceived -= this.OnCharacterReceived;
        }
    }
}