using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Notes.Properties;

namespace Notes.Dialogs
{
    [Serializable]
    public sealed class RootDialog : IDialog<object>
    {
#pragma warning disable 1998
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task StartAsync(IDialogContext context, IAwaitable<object> activity)
#pragma warning restore 1998
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var message = await activity as Activity;

            // Get the command, or the first word, that the user typed in.
            var userInput = message.Text != null ? message.Text : "";
            var command = (userInput.Split(new[] {' '}, 2))[0];
            
            // Command is Note.
            if (command.Equals(Resources.NOTE, StringComparison.CurrentCultureIgnoreCase))
            {
                await context.Forward(new NoteDialog(), this.StartAsync, message, CancellationToken.None);
            }
            // Command is Show.
            else if (command.Equals(Resources.SHOW, StringComparison.CurrentCultureIgnoreCase))
            {
                await context.Forward(new ShowDialog(), this.StartAsync, message, CancellationToken.None);
            }
            // Command is Delete.
            else if (userInput.Equals(Resources.DELETE, StringComparison.CurrentCultureIgnoreCase))
            {
                context.Call(new DeleteDialog(), this.StartAsync);
            }
            // Command is Delete force
            else if (userInput.Equals(Resources.DELETE_FORCE, StringComparison.CurrentCultureIgnoreCase))
            {
                context.Call(new DeleteForceDialog(), this.StartAsync);
            }
            // Command is Help or unknown.
            else
            {
                await context.Forward(new HelpDialog(), this.StartAsync, message, CancellationToken.None);
            }
        }       
    }
}