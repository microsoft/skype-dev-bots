using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Notes.Properties;
using MongoDB.Driver;
using Notes.Models;
using Notes.Helpers;

namespace Notes.Dialogs
{
    [Serializable]
    public sealed class DeleteForceDialog : IDialog<IMessageActivity>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var userId = context.Activity.From.Id;
            var deletedCount = await DeleteNotes(userId);

            var replyString = deletedCount > 0 ?
                String.Format(Resources.DELETED_ALL_TEXT, deletedCount) :
                Resources.NO_NOTES_TO_DELETE;

            await context.PostAsync(replyString);

            context.Done<object>(null);
        }

        // Delete all notes given user ID, and returns the number of deleted notes on success.
        private static async Task<long> DeleteNotes(string userId)
        {
            var collection = DbSingleton.GetDatabase().GetCollection<Note>(AppSettings.CollectionName);
            var filter = Builders<Note>.Filter.Where(x => x.UserId == userId);

            var response = await collection.DeleteManyAsync(filter);

            return response.DeletedCount;
        }
    }
}