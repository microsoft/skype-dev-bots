using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MongoDB.Driver;
using Notes.Helpers;
using Notes.Models;
using Notes.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Dialogs
{
    [Serializable]
    public class ShowDialog : IDialog<object>
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
            var searchText = userInput.Length < 2 ? "" : userInput[1];

            // Get notes and create a reply to reply back to the user.
            var userId = context.Activity.From.Id;
            List<Note> notes = GetNotesForUser(userId, searchText);

            var reply = context.MakeMessage();
            reply.TextFormat = "xml";
            reply.Text = BuildNoteText(notes, message.LocalTimestamp);

            await context.PostAsync(reply);

            context.Done<object>(null);
        }

        private static List<Note> GetNotesForUser(string userId, string searchText)
        {
            var collection = DbSingleton.GetDatabase().GetCollection<Note>(AppSettings.CollectionName);
            var filter = Builders<Note>.Filter.Where(x => x.UserId == userId);

            if (!String.IsNullOrEmpty(searchText))
            {
                filter = filter & Builders<Note>.Filter.Where(x => x.Content.Contains(searchText));
            }

            var notes = collection.Find(filter).ToList();
            return notes;
        }

        private static string BuildNoteText(List<Note> notes, DateTimeOffset? localTimestamp)
        {
            var text = "";

            var title = String.Format(Resources.NOTE_TITLE, notes.Count);
            text += title;

            foreach (var note in notes)
            {
                if (note != null)
                {
                    var dateTime = FormatTime(note.Timestamp, localTimestamp);
                    text += "\n\n<i>" + dateTime + "</i>";
                    text += "\n" + note.Content;
                }
            }

            return text;
        }

        private static String FormatTime(long timestampTicks, DateTimeOffset? localTimestamp)
        {
            var dateTimeString = "";
            var dateTime = new DateTime(timestampTicks);
            if (localTimestamp != null)
            {
                var timeOffset = dateTime.Add(((DateTimeOffset)localTimestamp).Offset);
                dateTimeString = timeOffset.ToString("MMM dd, yyyy HH:mm");
            }
            else
            {
                dateTimeString = dateTime.ToString("MMM dd, yyyy HH:mm") + " (UTC)";
            }

            return dateTimeString;
        }
    }
}