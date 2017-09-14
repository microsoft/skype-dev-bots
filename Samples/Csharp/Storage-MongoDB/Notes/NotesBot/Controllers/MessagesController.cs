using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MongoDB.Driver;
using Notes.Dialogs;
using Notes.Properties;

namespace Notes.Controllers
{
    [Serializable]
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private static IConnectorClient connectorClient;

        [HttpPost]
        [ResponseType(typeof(string))]
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            var responseMessage = string.Empty;

            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new RootDialog());
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
                    // Get a welcome card.
                    var name = activity.From.Name;
                    var welcomeCard = GetWelcomeCard(name);

                    // send a message with the welcome card
                    return await SendMessageWithAttachments(activity, new List<Attachment> {welcomeCard.ToAttachment()});
                case ContactRelationUpdateActionTypes.Remove:
                    // No action.
                    break;
            }

            return null;
        }
        
        private static HeroCard GetWelcomeCard(string name)
        {
            return new HeroCard
            {
                Title = String.Format(Resources.WELCOME_CARD_TITLE, name),
                Text = Resources.WELCOME_CARD_TEXT,
                Images = new List<CardImage>
                {
                    new CardImage
                    {
                        Url = Resources.WELCOME_CARD_LINK
                    }
                },
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, Resources.HELP_TITLE, value: Resources.HELP)
                }
            };

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