using System;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using FridgeBot.Utils;
using FridgeBot.Properties;

namespace FridgeBot.Dialogs
{
    [LuisModel("$MicrosoftLUISAppId$", "$MicrosoftLUISSubscriptionKey$")]
    [Serializable]
    public class IntentDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(Resources.NOT_YET_SUPPORTED);
            context.Wait(MessageReceived);
        }

        [LuisIntent("add")]
        public async Task Add(IDialogContext context, LuisResult result)
        {
            EntityRecommendation itemToAdd;
            if (!result.TryFindEntity("item", out itemToAdd))
            {
                await context.PostAsync(Resources.NO_ITEM_TO_ADD);
                context.Wait(MessageReceived);
            }
            else
            {
                Util.AddToFridge(context, itemToAdd.Entity);
                await context.PostAsync(string.Format(Resources.ADDED, itemToAdd.Entity));
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
                removeText = Resources.NO_ITEM_TO_REMOVE;
            }
            else
            {
                bool removed = Util.RemoveFromFridge(context, itemToRemove.Entity);
                removeText = string.Format(removed ? Resources.REMOVED_SUCC : Resources.REMOVED_ERR, itemToRemove.Entity);
            }

            await context.PostAsync(removeText);
            context.Wait(MessageReceived);

        }

        [LuisIntent("show")]
        public async Task Show(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(string.Format(Resources.SHOW, Util.GetAllFromFridgeToString(context)));
            context.Wait(MessageReceived);
        }

        [LuisIntent("help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(Resources.HELP);
            context.Wait(MessageReceived);
        }

        [LuisIntent("clear")]
        public virtual async Task Clear(IDialogContext context, LuisResult result)
        {
            PromptDialog.Text(context, AfterClear, Resources.CLEAR);
        }

        public async Task AfterClear(IDialogContext context, IAwaitable<string> argument)
        {
            var confirm = await argument;
            string clearText = Resources.CLEAR_NO;

            if (confirm == Resources.YES)
            {
                Util.RemoveAllFromFridge(context);
                clearText = Resources.CLEAR_YES;
            }

            await context.PostAsync(clearText);
            context.Wait(MessageReceived);
        }
    }
}