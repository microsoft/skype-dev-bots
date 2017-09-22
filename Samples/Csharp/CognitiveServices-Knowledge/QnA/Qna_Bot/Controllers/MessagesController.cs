using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System;

namespace QnaBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private static IConnectorClient connectorClient;
        
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            var responseMessage = string.Empty;
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else
            {
                responseMessage = await HandleSystemMessage(activity);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK, responseMessage);
            return response;
        }

        private static async Task<string> HandleSystemMessage(Activity activity)
        {
            switch (activity.Type)
            {
                case ActivityTypes.ContactRelationUpdate:
                    return await OnContactRelationUpdate(activity);
            }

            return string.Empty;
        }

        private static async Task<string> OnContactRelationUpdate(Activity activity)
        {
            switch (activity.Action)
            {
                case ContactRelationUpdateActionTypes.Add:
                    var welcomeCard = Dialogs.WelcomeCard.get(activity);
                    return await SendMessageWithAttachments(activity, new List<Attachment> { welcomeCard.ToAttachment() });
                case ContactRelationUpdateActionTypes.Remove:
                    // No action.
                    break;
            }

            return null;
        }

        private static async Task<string> SendMessageWithAttachments(Activity activity,
            IList<Attachment> attachments)
        {
            var reply = activity.CreateReply();
            reply.Attachments = attachments;
            if (connectorClient == null)
            {
                connectorClient = new ConnectorClient(new Uri(activity.ServiceUrl));
            }

            var resourceResponse = await connectorClient.Conversations.SendToConversationAsync(reply);
            return resourceResponse.Id;
        }
    }
}