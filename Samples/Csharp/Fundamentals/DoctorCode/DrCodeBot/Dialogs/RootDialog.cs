using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using DrCodeBot.Utils;
using DrCodeBot.Properties;

namespace DrCodeBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
#pragma warning disable 1998
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task StartAsync(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);
        }
#pragma warning restore 1998

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            // get the message received
            var activity = await result as Activity;
            var userResponse = activity.Text == null ? "" : activity.Text;

            if (userResponse.Equals(Resources.CHOICE_GREET, StringComparison.CurrentCultureIgnoreCase))
            {
                context.Call(new GreetButtonClickDialog(), this.StartAsync);
            }
            else if (userResponse.Equals(Resources.CHOICE_HELP, StringComparison.CurrentCultureIgnoreCase))
            {
                context.Call(new HelpButtonClickDialog(), this.StartAsync);
            }
            else if (userResponse.Equals(Resources.CHOICE_TYPING, StringComparison.CurrentCultureIgnoreCase))
            {
                context.Call(new TypingButtonClickDialog(), this.StartAsync);
            }
            else
            {
                IMessageActivity welcomeMessage = context.MakeMessage();
                welcomeMessage.Attachments.Add(CardFactory.getWelcomeCard().ToAttachment());
                await context.PostAsync(welcomeMessage);
                context.Wait(MessageReceivedAsync);
            }
        }
    }
}