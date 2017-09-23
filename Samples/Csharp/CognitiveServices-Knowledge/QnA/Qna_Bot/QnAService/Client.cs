using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Web.Script.Serialization;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace QnaBot.QnAService
{
    public class Client
    {
        static HttpClient client = new HttpClient();
        static JavaScriptSerializer serializer = new JavaScriptSerializer();
        private string knowledgebaseId = "";
        public Client(string baseUrl, string knowledgebaseId, string subscriptionKey)
        {
            this.knowledgebaseId = knowledgebaseId;
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //ACCEPT header
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
        }

        public async Task<RootResponse> GetAnswersAsync(string question, int top)
        {
            var requestUri = $"knowledgebases/{knowledgebaseId}/generateAnswer";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            var body = $"{{\"question\": \"{question}\", \"top\": \"{top}\"}}";
            using (var content = new StringContent(body, Encoding.UTF8, "application/json"))
            {
                request.Content = content;
                var response = await MakeRequestAsync(request);
                var responseObj = new RootResponse();

                if (!string.IsNullOrEmpty(response))
                {
                    responseObj = serializer.Deserialize<RootResponse>(response);
                }
                    
                return responseObj;
            }
        }

        public async Task<RootResponse> GetSuggestedFaqs(int top)
        {
            var requestUri = $"knowledgebases/{knowledgebaseId}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await MakeRequestAsync(request);

            var result = new RootResponse();
            if (!string.IsNullOrEmpty(response))
            {
                var docUrl = (string)serializer.DeserializeObject(response);
                using (WebClient wc = new WebClient())
                {
                    var jsonData = wc.DownloadString(docUrl);
                    var faqModels = ParseRawFaqData(jsonData, top);
                    result = faqModels;
                }
            }
            return result; 
        }

        private async Task<string> MakeRequestAsync(HttpRequestMessage getRequest)
        {
            var response = await client.SendAsync(getRequest).ConfigureAwait(false);
            var responseString = string.Empty;
            try
            {
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                // empty responseString
            }

            return responseString;
        }

        private RootResponse ParseRawFaqData(string faqData, int top)
        {
            RootResponse result = new RootResponse();
            List<Answer> answers = new List<Answer>();
            var faq = faqData.Split('\t');
            
            for (int i = 0; i < faq.Length;)
            {
                var text = faq[i];
                var position = text.LastIndexOf('?');

                if (position != -1)
                {
                    var questionText = text.Substring(text.IndexOf('\n') + 1).Replace("https://azure.microsoft.com/en-us/support/faq/", string.Empty);
                    var answerText = faq[i + 1];

                    answerText = RemoveInvalidChars(answerText);
                    questionText = RemoveInvalidChars(questionText);

                    Answer answer = new Answer();
                    answer.answer = answerText;
                    answer.questions = new List<string> { questionText };
                    answers.Add(answer);
                    i += 1;
                    if (answers.Count == top)
                    {
                        break;
                    }
                }
                i++;
            }
            result.answers = answers;
            return result;
        }

        private static string RemoveInvalidChars(string text)
        {
            string from = string.Copy(text);
            from = from.Replace("\\n", string.Empty);
            return new string(from.Where(x => char.IsWhiteSpace(x) || char.IsLetterOrDig‌​it(x)).ToArray());
        }
    }
}