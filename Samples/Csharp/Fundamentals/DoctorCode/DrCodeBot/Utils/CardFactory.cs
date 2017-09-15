using Microsoft.Bot.Connector;
using System.Collections.Generic;
using DrCodeBot.Properties;

namespace DrCodeBot.Utils
{
    public static class CardFactory
    {
        public static HeroCard getWelcomeCard()
        {
            return new HeroCard
            {
                Text = Resources.GREET,
                Images = new List<CardImage>
                {
                    new CardImage(url: Resources.FACE_IMAGE_URL)
                },
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, Resources.CHOICE_GREET, value: Resources.CHOICE_GREET),
                    new CardAction(ActionTypes.ImBack, Resources.CHOICE_HELP, value: Resources.CHOICE_HELP),
                    new CardAction(ActionTypes.ImBack, Resources.CHOICE_TYPING, value: Resources.CHOICE_TYPING)
                }
            };
        }

        public static HeroCard getHelpCard()
        {
            return new HeroCard
            {
                Title = Resources.HELP_TITLE,
                Text = Resources.HELP,
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, Resources.CHOICE_GREET, value: Resources.CHOICE_GREET),
                    new CardAction(ActionTypes.ImBack, Resources.CHOICE_HELP, value: Resources.CHOICE_HELP),
                    new CardAction(ActionTypes.ImBack, Resources.CHOICE_TYPING, value: Resources.CHOICE_TYPING)
                }
            };
        }
    }
}