using MongoDB.Driver;

namespace Notes.Helpers
{
    public sealed class DbSingleton
    {
        private static readonly DbSingleton instance;
        private static IMongoDatabase database = null;

        private DbSingleton()
        {
            database = new MongoClient(AppSettings.MongoDBConnectionString).GetDatabase(AppSettings.DbName);
        }

        static DbSingleton()
        {
            instance = new DbSingleton();
        }
        
        public static IMongoDatabase GetDatabase()
        {
            return database;
        }
    }
}