using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using DrCodeBot.Utils;

using System.Diagnostics;

namespace DrCodeBot.Dialogs
{
    [Serializable]
    public class HelpDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            // send the help hero card
            IMessageActivity helpMessage = context.MakeMessage();
            helpMessage.Attachments.Add(CardFactory.getHelpCard().ToAttachment());
            await context.PostAsync(helpMessage);

            context.Done<object>(null);
        }
    }
}