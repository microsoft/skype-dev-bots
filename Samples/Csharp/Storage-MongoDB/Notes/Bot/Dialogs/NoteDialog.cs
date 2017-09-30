using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Notes.Properties;
using Notes.Models;
using Notes.Helpers;

namespace Notes.Dialogs
{
    [Serializable]
    public class NoteDialog : IDialog<object>
    {
#pragma warning disable 1998
        public async Task StartAsync(IDialogContext context)
#pragma warning restore 1998
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result as Activity;

            var userInput = (message.Text != null ? message.Text : "").Split(new[] { ' ' }, 2);
            var command = userInput[0];
            var content = userInput.Length < 2 ? "" : userInput[1];

            // Create a reply.
            IMessageActivity reply = context.MakeMessage();

            // When users types in "note" without the content, give them instructions.
            if (String.IsNullOrWhiteSpace(content))
            {
                reply.Text = Resources.NOTE_NO_CONTENT;
            }
            // Save the note.
            else
            {
                var userId = context.Activity.From.Id;                
                var notesCollection = DbSingleton.GetDatabase().GetCollection<Note>(AppSettings.CollectionName);
                notesCollection.InsertOne(CreateNote(userId, content));

                reply.Attachments.Add(GetNoteAddedCard(content).ToAttachment());
            }
            
            await context.PostAsync(reply);
            context.Done<object>(null);
        }

        private Note CreateNote(string userId, string content)
        {
            var ticks = DateTime.UtcNow.Ticks;

            return new Note
            {
                NoteId = userId + "_" + ticks,
                UserId = userId,
                Timestamp = ticks,
                Content = content
            };
        }
        
        private static HeroCard GetNoteAddedCard(string content)
        {
            return new HeroCard
            {
                Title = Resources.NOTE_ADDED_SUCCESSFULLY,
                Subtitle = content,
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, Resources.SHOW_NOTES, value: Resources.SHOW),
                    new CardAction(ActionTypes.ImBack, Resources.DELETE_NOTES, value: Resources.DELETE)
                }
            };
        }
    }
}