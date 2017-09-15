using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using DrCodeBot.Utils;
using DrCodeBot.Properties;

namespace DrCodeBot.Dialogs
{
    [Serializable]
    public class HelpButtonClickDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(Resources.HELP_INTRO);

            // send the welcome message again
            IMessageActivity helpMessage = context.MakeMessage();
            helpMessage.Attachments.Add(CardFactory.getHelpCard().ToAttachment());
            await context.PostAsync(helpMessage);

            await context.PostAsync(Resources.HELP_TRY_MSG);

            // code formatting
            IMessageActivity codeMessage = context.MakeMessage();
            codeMessage.Text = Resources.HELP_CODE;
            codeMessage.TextFormat = "xml";
            await context.PostAsync(codeMessage);

            await context.PostAsync(Resources.HELP_EXPLAIN);
            await context.PostAsync(Resources.HELP_MORE);

            context.Done<object>(null);
        }
    }
}