using System;
using System.Collections.Generic;

using Microsoft.Bot.Builder.Dialogs;

namespace FridgeBot.Utils
{
    // helper functions
    public static class Util
    {
        private static void InitFridge(IDialogContext context)
        {
            List<string> ingredients;
            bool isFirstTime = !context.UserData.TryGetValue<List<string>>("ingredients", out ingredients);
            if (isFirstTime)
            {
                context.UserData.SetValue<List<string>>("ingredients", new List<string>());
            }
        }

        public static bool IsInFridge(IDialogContext context, string item)
        {
            InitFridge(context);

            return context.UserData.GetValue<List<string>>("ingredients").Contains(item);
        }

        public static void AddToFridge(IDialogContext context, string item)
        {
            InitFridge(context);

            List<string> ingredients = context.UserData.GetValue<List<string>>("ingredients");
            if (ingredients == null)
            {
                ingredients = new List<string> { item };
            }
            else
            {
                ingredients.Add(item);
            }
            context.UserData.SetValue<List<string>>("ingredients", ingredients);
        }

        public static bool RemoveFromFridge(IDialogContext context, string item)
        {
            InitFridge(context);

            List<string> ingredients = context.UserData.GetValue<List<string>>("ingredients");
            if (ingredients == null || ingredients.Count == 0 || !ingredients.Remove(item))
            {
                return false;
            }

            context.UserData.SetValue<List<string>>("ingredients", ingredients);
            return true;
        }

        public static void RemoveAllFromFridge(IDialogContext context)
        {
            context.UserData.SetValue<List<string>>("ingredients", new List<string>());
        }

        public static List<string> GetAllFromFridge(IDialogContext context)
        {
            InitFridge(context);

            return context.UserData.GetValue<List<string>>("ingredients");
        }

        public static string GetAllFromFridgeToString(IDialogContext context)
        {
            List<string> ingredients = GetAllFromFridge(context);
            return (ingredients == null || ingredients.Count == 0) ? "nothing" : String.Join(", ", ingredients);
        }
    }
}