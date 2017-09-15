using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Scorables.Internals;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using DrCodeBot.Properties;

namespace DrCodeBot.Dialogs
{
    public class HelpScorable : ScorableBase<IActivity, string, double>
    {
        private readonly IDialogTask task;

        public HelpScorable(IDialogTask task)
        {
            SetField.NotNull(out this.task, nameof(task), task);

        }

        protected override Task DoneAsync(IActivity item, string state, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        protected override double GetScore(IActivity item, string state)
        {
            return 1.0;
        }

        protected override bool HasScore(IActivity item, string state)
        {
            return state != null;
        }
        protected override async Task<string> PrepareAsync(IActivity item, CancellationToken token)
        {
            var message = item as IMessageActivity;

            if (message != null && !string.IsNullOrWhiteSpace(message.Text))
            {
                if (message.Text.Equals(Resources.HELP_TRIGGER_TEXT, StringComparison.InvariantCultureIgnoreCase))
                {
                    return message.Text;
                }
            }
            return null;
        }
        
        protected override async Task PostAsync(IActivity item, string state, CancellationToken token)
        {
            var message = item as IMessageActivity;
            if (message != null)
            {
                var helpDialog = new HelpDialog();
                var interruption = helpDialog.Void<object, IMessageActivity>();
                this.task.Call(interruption, null);

                await this.task.PollAsync(token);
            }
        }
    }
}