using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using AuthBot;
using AuthBot.Dialogs;
using AuthBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace MVP.App.Bot.Dialogs
{
    [Serializable]
    [LuisModel("3bd30c5a-4049-4825-bf2d-b594db13f044", "4400bf5e7b6140718188f4d25c3af592")]
    public class RootDialog : AppBaseDialog<string>
    {
        private string receivedMessage;

        protected override async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var message = await item;

            receivedMessage = message.Text.ToLowerInvariant();

            if (receivedMessage.Contains("help") || message.Type != ActivityTypes.Message)
            {
                await base.MessageReceived(context, item);
                return;
            }

            var accessToken = await context.GetAccessToken(AuthSettings.Scopes);

            if (string.IsNullOrEmpty(accessToken))
            {
                if (receivedMessage.Contains("login"))
                {
                    await context.Forward(new AzureAuthDialog(AuthSettings.Scopes, "Sign in with MVP account"), this.AuthCompleteAsync, message, CancellationToken.None);
                }
                else
                {
                    await this.Help(context, new LuisResult());
                }
            }
            else
            {
                // User authenticated
                // Get profile
                // Next steps
            }

        }

        [LuisIntent("Help")]
        private async Task Help(IDialogContext context, LuisResult luisResult)
        {
            var message = $"Hey MVP!\n\n";
            var accessToken = await context.GetAccessToken(AuthSettings.Scopes);

            if (string.IsNullOrEmpty(accessToken))
            {
                message += $"Before I can help you, I need you to login.";
            }
            else
            {
                message += "I can help you with: \n";
                message += "* List you recent contributions\n";
                message += "* Add new contributions\n";
            }

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Logout")]
        public async Task Logout(IDialogContext context, LuisResult result)
        {
            context.UserData.Clear();
            await context.Logout();

            context.Wait(this.MessageReceived);
        }

        private async Task AuthCompleteAsync(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;
            await context.PostAsync(message);

            // Get profile
        }
    }
}