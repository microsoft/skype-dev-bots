using System.Collections.Generic;

namespace QnaBot.QnAService
{
    public class RootResponse
    {
        public List<Answer> answers { get; set; }

        public bool IsValid()
        {
            var response = this;
            return response.answers != null &&
                response.answers.Count >= 1 && response.answers[0].score > 0;
        }
    }
}