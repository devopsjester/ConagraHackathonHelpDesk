using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDeskBot.Dialogs
{
    [Serializable]
    public class BasicQnAMakerDialog : QnAMakerDialog
    {
        // Go to https://qnamaker.ai and feed data, train & publish your QnA Knowledgebase.
        public BasicQnAMakerDialog()
            : base(new QnAMakerService(
                new QnAMakerAttribute(
                    ConfigurationManager.AppSettings["QnASubscriptionKey"],
                    ConfigurationManager.AppSettings["QnAKnowledgebaseId"])))  // If you're running this bot locally, make sure you have these appSettings in youe web.config
        {
        }

        // Override to also include the knowledgebase question with the answer on confident matches
        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults results)
        {
            if (results.Answers.Count > 0)
            {
                var response = "Here is the match from FAQ:  \r\n  Q: " + results.Answers.First().Questions.First() + "  \r\n A: " + results.Answers.First().Answer;
                await context.PostAsync(response);
            }
        }

        // Override to log matched Q&A before ending the dialog
        protected override async Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults results)
        {
            Console.WriteLine("KB Question: " + results.Answers.First().Questions.First());
            Console.WriteLine("KB Answer: " + results.Answers.First().Answer);
            await base.DefaultWaitNextMessageAsync(context, message, results);
        }
    }
}