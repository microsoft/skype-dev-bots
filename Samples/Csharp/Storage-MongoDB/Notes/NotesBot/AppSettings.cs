namespace Notes
{
    using System;
    using System.Configuration;

    public static class AppSettings
    {
        private static Lazy<string> skypeBotId = new Lazy<string>(() => ConfigurationManager.AppSettings["SkypeBotId"].ToString());
        private static Lazy<string> skypeBotMention = new Lazy<string>(() => ConfigurationManager.AppSettings["SkypeBotAtMention"].ToString());
        private static Lazy<string> mongoDBConnectionString = new Lazy<string>(() => ConfigurationManager.ConnectionStrings["MongoDB"].ToString());
        private static Lazy<string> dbName = new Lazy<string>(() => ConfigurationManager.AppSettings["DBName"].ToString());

        public static string SkypeBotId => skypeBotId.Value;
        public static string DbName => dbName.Value;
        public static string MongoDBConnectionString => mongoDBConnectionString.Value;
    }
}