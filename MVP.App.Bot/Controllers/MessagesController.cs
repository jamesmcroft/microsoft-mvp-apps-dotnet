using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MVP.App.Bot.Dialogs;

namespace MVP.App.Bot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity != null)
            {
                switch (activity.GetActivityType())
                {
                    case ActivityTypes.Message:
                        await Conversation.SendAsync(activity, () => new RootDialog());
                        break;
                    default:
                        Trace.TraceError($"MVP Community Bot ignored an activity. Activity type received: {activity.GetActivityType()}");
                        break;
                }
            }

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}