using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace QnaBot.Dialogs
{
    public class DialogUtils
    {
        public static async Task SendMessageAsync(IBotToUser context, Attachment attachment)
        {
            var message = context.MakeMessage();
            message.Attachments.Add(attachment);
            await context.PostAsync(message);
        }

        public static async Task SendMessageAsync(IBotToUser context, List<HeroCard> cards, string title)
        {
            var message = context.MakeMessage();
            message.Text = title;
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            message.Attachments = cards.Select(card => card.ToAttachment()).ToList();
            await context.PostAsync(message);
        }

        public static async Task SendMessageAsync(IBotToUser context, string text)
        {
            var message = context.MakeMessage();
            message.Text = text;
            await context.PostAsync(message);
        }
    }
}