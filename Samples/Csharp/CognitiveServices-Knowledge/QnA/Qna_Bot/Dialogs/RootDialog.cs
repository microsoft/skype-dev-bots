using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using QnaBot.QnAService;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System.Linq;
using System.Text;
using System.Threading;
using QnaBot.Properties;

namespace QnaBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {

        private static readonly List<string> commandWords = new List<string>
        {
            Resources.HELP_TEXT,
            Resources.HELP_TEXT_ASK_QUESTION
        };

        private static readonly List<String> questionPhrases = new List<string>
        {
            Resources.Q_WHAT_IS,
            Resources.Q_HOW,
            Resources.Q_DOES,
            Resources.Q_DO,
            Resources.Q_IS_THERE,
            Resources.Q_WHERE
        };

        static QnAService.Client qnaClient = new QnAService.Client(AppSettings.QnABaseUrl, AppSettings.QnAKnowledgebaseId, AppSettings.QnASubscriptionKey);

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var message = await activity;

            var command = "";
            var fullString = "";
            var individualWords = new List<String>();
            var isQuestion = false;
            if (!string.IsNullOrEmpty(message.Text))
            {
                fullString = message.Text;
                individualWords = fullString.Split(' ')
                    .Where(word => !string.IsNullOrEmpty(word))
                    .Select(word => word.Replace(" ", string.Empty))
                    .ToList();
                command = GetValidCommand(individualWords.First());
                isQuestion = CheckIsValidQuestion(fullString);
            }

            if (!string.IsNullOrEmpty(command) && !isQuestion)
            {
                if (command.Equals(Resources.HELP_TEXT))
                {
                    await ShowHelp(context);
                } 
                else if (command.Equals(Resources.HELP_TEXT_ASK_QUESTION))
                {
                    await ShowAskQuestionHelpResponse(context);
                }
            }
            else if (isQuestion)
            {
                await HandleQuestionAsked(context, fullString);
            }
            else
            {
                await ShowHelp(context);
            }
            
            context.Wait(MessageReceivedAsync);
        }

        private async Task HandleQuestionAsked(IDialogContext context, string question)
        {
            var response = await qnaClient.GetAnswersAsync(question, AppSettings.Max_Answers);
            if (response != null && response.IsValid())
            {
                var titleText = string.Format(Resources.TOP_ANSWERS_TITLE, response.answers.Count);
                await OnShowFaqAnswers(context, response.answers, titleText);
            }
            else
            {
                await ShowNoAnswer(context);
                var suggestionReponse = await qnaClient.GetSuggestedFaqs(AppSettings.Max_Answers);
                if (suggestionReponse != null && suggestionReponse.IsValid())
                {
                    await OnShowFaqAnswers(context, suggestionReponse.answers, Resources.SUGGESTED_ANSWERS_TITLE);
                }
                else
                {
                    await DialogUtils.SendMessageAsync(context, Resources.ERROR);
                }
            }
        }

        private static async Task OnShowFaqAnswers(IBotContext context, List<Answer> answers, string title)
        {
            List<HeroCard> answerCards = new List<HeroCard>();
            foreach (var answer in answers)
            {
                var card = GetAnswerCard(answer);
                answerCards.Add(card);
            }
            
            await DialogUtils.SendMessageAsync(context, answerCards, title);
        }

        private static async Task ShowWelcome(IBotContext context)
        {
            var card = WelcomeCard.get(context.Activity);
            await DialogUtils.SendMessageAsync(context, card.ToAttachment());
        }

        private static async Task ShowHelp(IBotContext context)
        {
            var card = GetHelpCard();
            await DialogUtils.SendMessageAsync(context, card.ToAttachment());
        }

        private static async Task ShowNoAnswer(IBotContext context)
        {
            await DialogUtils.SendMessageAsync(context, Resources.NO_ANSWER_FOUND);
        }

        private static async Task ShowAskQuestionHelpResponse(IBotContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string question in questionPhrases)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(", ");
                }
                stringBuilder.Append(question);
            }
            string text = string.Format(Resources.HELP_ASK_QUESTION_RESPONSE, stringBuilder.ToString());
            await DialogUtils.SendMessageAsync(context, text);
        }

        private static HeroCard GetAnswerCard(Answer answer)
        {
            return new HeroCard
            {
                Subtitle = answer.questions.First(),
                Text = answer.answer,
                Tap = new CardAction(ActionTypes.OpenUrl, value: AppSettings.DirectFaqUrl)
            };
        }

        private static HeroCard GetHelpCard()
        {
            return new HeroCard
            {
                Title = Resources.HELP_TEXT,
                Subtitle = Resources.HELP_CARD_SUBTITLE,
                Buttons = new List<CardAction>
                    {
                        new CardAction(ActionTypes.PostBack, Resources.HELP_TEXT_ASK_QUESTION, value: Resources.HELP_TEXT_ASK_QUESTION),
                        new CardAction(ActionTypes.OpenUrl, Resources.HELP_TEXT_CHECK_FAQ, value: AppSettings.DirectFaqUrl)
                    }
            };
        }

        private string GetValidCommand(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            var matchedCommand = commandWords.Find(s => s.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0);
            return matchedCommand;
        }

        private static Boolean CheckIsValidQuestion(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            Boolean result = false;
            foreach (string phrase in questionPhrases) {
                Boolean matchesPhrase = text.StartsWith(phrase, StringComparison.OrdinalIgnoreCase);
                result = matchesPhrase && CheckDoesEndInQuestion(text);
                if (result) 
                {
                    break;
                }
            }

            return result;
        }

        private static Boolean CheckDoesEndInQuestion(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            return text.EndsWith("?");
        }
    }
}