using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using DrCodeBot.Properties;

namespace DrCodeBot.Dialogs
{
    [Serializable]
    public class TypingButtonClickDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(Resources.TYPING_INTRO);
            await context.PostAsync(Resources.TYPING_TRY_MSG_1);

            // Send the typing indicator for few seconds (10 sec)
            IMessageActivity typingIndicatorMessage = context.MakeMessage();
            typingIndicatorMessage.Type = ActivityTypes.Typing;
            typingIndicatorMessage.Text = null;
            await context.PostAsync(typingIndicatorMessage);
            await Task.Delay(10000);

            await context.PostAsync(Resources.TYPING_TRY_MSG_2);

            // code formatting
            IMessageActivity codeMessage = context.MakeMessage();
            codeMessage.Text = Resources.TYPING_CODE;
            codeMessage.TextFormat = "xml";
            await context.PostAsync(codeMessage);

            await context.PostAsync(Resources.TYPING_EXPLAIN);
            await context.PostAsync(Resources.TYPING_MORE);

            context.Done<object>(null);
        }
    }
}