using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using DrCodeBot.Utils;
using DrCodeBot.Properties;

namespace DrCodeBot.Dialogs
{
    [Serializable]
    public class GreetButtonClickDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(Resources.GREET_INTRO);

            // send the welcome message again
            IMessageActivity welcomeMessage = context.MakeMessage();
            welcomeMessage.Attachments.Add(CardFactory.getWelcomeCard().ToAttachment());
            await context.PostAsync(welcomeMessage);

            await context.PostAsync(Resources.GREET_TRY_MSG);

            // code formatting
            IMessageActivity codeMessage = context.MakeMessage();
            codeMessage.Text = Resources.GREET_CODE;
            codeMessage.TextFormat = "xml";
            await context.PostAsync(codeMessage);

            await context.PostAsync(Resources.GREET_EXPLAIN);
            await context.PostAsync(Resources.GREET_MORE);

            context.Done<object>(null);
        }
    }
}