using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Notes.Properties;

namespace Notes.Dialogs
{
    [Serializable]
    public sealed class HelpDialog : IDialog<IMessageActivity>
    {
        private static readonly List<string> subCommandWords = new List<string>
        {
            Resources.NOTE,
            Resources.SHOW,
            Resources.DELETE
        };

#pragma warning disable 1998
        public async Task StartAsync(IDialogContext context)
#pragma warning restore 1998
        {
            context.Wait(MessageReceivedAsync);
        }

        private static async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var message = await activity as Activity;

            var userInput = (message.Text != null ? message.Text : "").Split(new[] { ' ' }, 2);
            var command = userInput[0];
            var subCommand = userInput.Length < 2 ? "" : userInput[1];

            // Create a reply.
            IMessageActivity reply = context.MakeMessage();

            if (command.Equals(Resources.HELP, StringComparison.CurrentCultureIgnoreCase) && subCommand.Equals(""))
            {
                // Provide generic help.
                reply.Attachments.Add(GetHelpCard(true).ToAttachment());
            }
            else if (command.Equals(Resources.HELP, StringComparison.CurrentCultureIgnoreCase) && IsValidSubCommand(subCommand))
            {
                if (subCommand.Equals(Resources.NOTE, StringComparison.CurrentCultureIgnoreCase))
                {
                    reply.Text = Resources.NOTE_HELP_TEXT;
                }
                else if (subCommand.Equals(Resources.SHOW, StringComparison.CurrentCultureIgnoreCase))
                {
                    reply.Text = Resources.SHOW_HELP_TEXT;
                }
                else if (subCommand.Equals(Resources.DELETE, StringComparison.CurrentCultureIgnoreCase))
                {
                    reply.Text = Resources.DELETE_HELP_TEXT;
                }
                else
                {
                    // This part should never be reached.
                }
            }
            else
            {
                // Provide "I didn't understand", or, non-generic help.
                reply.Attachments.Add(GetHelpCard(false).ToAttachment());
            }

            await context.PostAsync(reply);
            context.Done<object>(null);
        }


        private static bool IsValidSubCommand(string subCommand)
        {
            return subCommandWords.Any(word => word.Equals(subCommand, StringComparison.CurrentCultureIgnoreCase));
        }

        private static HeroCard GetHelpCard(bool isGeneric)
        {
            return new HeroCard
            {
                Title = isGeneric ? Resources.HELP_TITLE : Resources.NOT_UNDERSTOOD,
                Text = Resources.HELP_CARD_TEXT,
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, Resources.NOTE, value: Resources.HELP_NOTE),
                    new CardAction(ActionTypes.ImBack, Resources.SHOW, value: Resources.HELP_SHOW),
                    new CardAction(ActionTypes.ImBack, Resources.DELETE, value: Resources.HELP_DELETE)
                }
            };
        }
    }
}