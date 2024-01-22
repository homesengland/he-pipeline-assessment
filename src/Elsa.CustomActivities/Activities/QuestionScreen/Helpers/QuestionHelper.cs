using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services.Models;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class QuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;

        public QuestionHelper(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        public async Task<string> GetAnswer(string correlationId, string workflowName, string activityName, string questionId)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);

            return GetAnswer(question);
        }

        public async Task<IEnumerable<char>> GetAnswer(string correlationId, int dataDictionaryId)
        {
            var question = await _elsaCustomRepository.GetQuestionByDataDictionary(correlationId,
                dataDictionaryId, CancellationToken.None);

            return GetAnswer(question);
        }

        private static string GetAnswer(CustomModels.Question? question)
        {
            if (question?.Answers != null)
            {
                return string.Join(',', question.Answers.Select(x => x.AnswerText));
            }

            return string.Empty;
        }

        public Task<string> WriteJournalData(string key, string message, ActivityExecutionContext context)
        {
            context.JournalData.Add(key, message);

            return Task.FromResult(string.Empty);
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("questionGetAnswer", (Func<string, string, string, string?>)((workflowName, activityName, questionId) => GetAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));
            engine.SetValue("writeJournalData", (Func<string, string, string>)((key, message) => WriteJournalData(key, message, activityExecutionContext).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function questionGetAnswer(workflowName: string, activityName:string, questionId:string ): string;");
            output.AppendLine("declare function writeJournalData(key: string, message:string): string;");
            return Task.CompletedTask;
        }
    }
}
