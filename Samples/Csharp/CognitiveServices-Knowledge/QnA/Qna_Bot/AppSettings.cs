namespace QnaBot
{
    using System;
    using System.Configuration;

    public class AppSettings
    {
        private static Lazy<string> _qnaBaseUrl = new Lazy<string>(() => ConfigurationManager.AppSettings["QnABaseUrl"].ToString());
        private static Lazy<string> _qnaKnowledgeBase = new Lazy<string>(() => ConfigurationManager.AppSettings["QnAKnowledgeBaseId"].ToString());
        private static Lazy<string> _qnaSubscriptionKey = new Lazy<string>(() => ConfigurationManager.AppSettings["QnASubscriptionKey"].ToString());

        public static string QnABaseUrl => _qnaBaseUrl.Value;
        public static string QnAKnowledgebaseId => _qnaKnowledgeBase.Value;
        public static string QnASubscriptionKey => _qnaSubscriptionKey.Value;

        public const int Max_Answers = 3;
        public const string DirectFaqUrl = "https://azure.microsoft.com/en-us/support/faq/";
    }
}