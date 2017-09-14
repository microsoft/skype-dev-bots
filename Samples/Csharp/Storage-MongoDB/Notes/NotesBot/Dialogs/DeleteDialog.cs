using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Notes.Properties;

namespace Notes.Dialogs
{
    [Serializable]
    public sealed class DeleteDialog : IDialog<IMessageActivity>
    {
        public async Task StartAsync(IDialogContext context)
        {
            // Create a reply and send it.
            IMessageActivity reply = context.MakeMessage();
            reply.Attachments.Add(GetDeleteCard().ToAttachment());
            await context.PostAsync(reply);

            context.Done<object>(null);
        }

        private static HeroCard GetDeleteCard()
        {
            return new HeroCard
            {
                Title = Resources.DELETE_ASK_CARD_TITLE,
                Text = Resources.DELETE_ASK_CARD_TEXT,
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, Resources.DELETE_ASK_CARD_YES, value: Resources.DELETE_FORCE),
                    new CardAction(ActionTypes.ImBack, Resources.DELETE_ASK_CARD_NO, value: Resources.SHOW)
                }
            };
        }
    }
}