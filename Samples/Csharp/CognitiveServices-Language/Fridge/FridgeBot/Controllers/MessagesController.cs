using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using FridgeBot.Properties;

namespace FridgeBot
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
            var connectorClient = new ConnectorClient(new Uri(activity.ServiceUrl));

            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new Dialogs.IntentDialog());
            }
            else
            {
                HandleSystemMessage(activity, connectorClient);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message, ConnectorClient connectorClient)
        {
            if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // send a greeting message when added
                if (message.Action == ContactRelationUpdateActionTypes.Add)
                {
                    Activity greet = message.CreateReply(Resources.GREET);
                    connectorClient.Conversations.ReplyToActivity(greet);
                }
                // clear up the user data when removed
                else if (message.Action == ContactRelationUpdateActionTypes.Remove)
                {
                    BotStateExtensions.DeleteStateForUser(message.GetStateClient().BotState, message.ChannelId, message.From.Id);
                }
            }

            return null;
        }
    }
}