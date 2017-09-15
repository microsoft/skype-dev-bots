using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using DrCodeBot.Utils;

namespace DrCodeBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            var connectorClient = new ConnectorClient(new Uri(activity.ServiceUrl));

            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else
            {
                HandleSystemMessage(activity, connectorClient);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private void HandleSystemMessage(Activity message, ConnectorClient connectorClient)
        {
            if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // send a welcome message when the user adds the bot
                if (message.Action == ContactRelationUpdateActionTypes.Add)
                {
                    Activity welcome = message.CreateReply();
                    welcome.Attachments.Add(CardFactory.getWelcomeCard().ToAttachment());
                    connectorClient.Conversations.ReplyToActivity(welcome);
                }
            }
        }
    }
}