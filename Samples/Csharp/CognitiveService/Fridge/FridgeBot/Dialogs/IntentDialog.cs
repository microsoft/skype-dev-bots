using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using FridgeBot.Utils;

namespace FridgeBot.Dialogs
{
    [LuisModel("$MicrosoftLUISAppId$", "$MicrosoftLUISSubscriptionKey$")]
    [Serializable]
    public class IntentDialog : LuisDialog<object>
    {
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(Text.NotYetSupported);
            context.Wait(MessageReceived);
        }

        [LuisIntent("add")]
        public async Task Add(IDialogContext context, LuisResult result)
        {
            EntityRecommendation itemToAdd;
            if (!result.TryFindEntity("item", out itemToAdd))
            {
                await context.PostAsync(Text.NoItemToAdd);
                context.Wait(MessageReceived);
            }
            else
            {
                Util.AddToFridge(context, itemToAdd.Entity);
                await context.PostAsync(string.Format(Text.Added, itemToAdd.Entity));
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("remove")]
        public async Task Remove(IDialogContext context, LuisResult result)
        {
            string removeText;
            EntityRecommendation itemToRemove;
            if (!result.TryFindEntity("item", out itemToRemove))
            {
                removeText = Text.NoItemToRemove;
            }
            else
            {
                bool removed = Util.RemoveFromFridge(context, itemToRemove.Entity);
                removeText = string.Format(removed ? Text.RemovedSucc : Text.RemovedErr, itemToRemove.Entity);
            }

            await context.PostAsync(removeText);
            context.Wait(MessageReceived);

        }

        [LuisIntent("show")]
        public async Task Show(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(string.Format(Text.Show, Util.GetAllFromFridgeToString(context)));
            context.Wait(MessageReceived);
        }

        [LuisIntent("help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(Text.Help);
            context.Wait(MessageReceived);
        }

        [LuisIntent("clear")]
        public virtual async Task Clear(IDialogContext context, LuisResult result)
        {
            PromptDialog.Text(context, AfterClear, Text.Clear);
        }

        public async Task AfterClear(IDialogContext context, IAwaitable<string> argument)
        {
            var confirm = await argument;
            string clearText = Text.ClearNo;

            if (confirm == Text.Yes)
            {
                Util.RemoveAllFromFridge(context);
                clearText = Text.ClearYes;
            }

            await context.PostAsync(clearText);
            context.Wait(MessageReceived);
        }
    }
}