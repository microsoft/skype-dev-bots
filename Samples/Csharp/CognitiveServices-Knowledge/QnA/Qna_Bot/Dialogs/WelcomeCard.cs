using Microsoft.Bot.Connector;
using QnaBot.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QnaBot.Dialogs
{
    public class WelcomeCard
    {
        public static HeroCard get(IActivity activity)
        {
            string username = GetUsernameFromContext(activity);
            return GetWelcomeCard(username);
        }

        private static HeroCard GetWelcomeCard(string username)
        {
            return new HeroCard
            {
                Title = string.Format(Resources.WELCOME_TITLE, username),
                Text = string.Format(Resources.WELCOME_SUBTITLE, AppSettings.DirectFaqUrl),
                Buttons = new List<CardAction>
                    {
                        new CardAction(ActionTypes.ImBack, Resources.HELP_TEXT, value: Resources.HELP_TEXT),
                    }
            };
        }

        private static string GetUsernameFromContext(IActivity context)
        {
            return !string.IsNullOrEmpty(context.From.Name) ? context.From.Name : context.From.Id;
        }
    }
}