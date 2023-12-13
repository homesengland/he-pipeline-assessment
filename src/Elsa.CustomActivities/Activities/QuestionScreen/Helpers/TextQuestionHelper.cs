using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class TextQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;

        public TextQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
        }


        public async Task<bool> AnswerEquals(string correlationId, string workflowName, string activityName, string questionId, string answerToCheck)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);

            return AnswerEquals(answerToCheck, question);
        }

        public async Task<bool> AnswerEquals(string correlationId, int dataDictionaryId, string answerToCheck)
        {
            var question = await _elsaCustomRepository.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None);

            return AnswerEquals(answerToCheck, question);
        }

        private static bool AnswerEquals(string answerToCheck, CustomModels.Question? question)
        {
            if (question != null && (question.QuestionType == QuestionTypeConstants.TextQuestion ||
                                     question.QuestionType == QuestionTypeConstants.TextAreaQuestion) &&
                question.Answers != null && question.Answers.Count == 1 &&
                question.Answers.First().AnswerText.ToLower() == answerToCheck.ToLower())
            {
                return true;
            }

            return false;
        }

        

        public async Task<bool> AnswerContains(string correlationId, string workflowName, string activityName, string questionId, string answerToCheck)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);

            return AnswerContains(answerToCheck, question);
        }

        public async Task<bool> AnswerContains(string correlationId, int dataDictionaryId, string answerToCheck)
        {
            var question = await _elsaCustomRepository.GetQuestionByDataDictionary(correlationId, dataDictionaryId, CancellationToken.None);

            return AnswerContains(answerToCheck, question);
        }

        private static bool AnswerContains(string answerToCheck, CustomModels.Question? result)
        {
            if (result != null && (result.QuestionType == QuestionTypeConstants.TextQuestion ||
                                   result.QuestionType == QuestionTypeConstants.TextAreaQuestion) &&
                result.Answers != null && result.Answers.Count == 1 &&
                result.Answers.First().AnswerText.ToLower().Contains(answerToCheck.ToLower()))
            {
                return true;
            }

            return false;
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("textQuestionAnswerEquals", (Func<string, string, string, string, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEquals(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("textQuestionAnswerEquals", (Func<int, string, bool>)((dataDictionaryId, answerToCheck) => AnswerEquals(activityExecutionContext.CorrelationId, dataDictionaryId, answerToCheck).Result));
            engine.SetValue("textQuestionAnswerContains", (Func<string, string, string, string, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerContains(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("textQuestionAnswerContains", (Func<int, string, bool>)((dataDictionaryId, answerToCheck) => AnswerContains(activityExecutionContext.CorrelationId, dataDictionaryId, answerToCheck).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function textQuestionAnswerEquals(workflowName: string, activityName:string, questionId:string, answerToCheck:string ): boolean;");
            output.AppendLine("declare function textQuestionAnswerContains(workflowName: string, activityName:string, questionId:string, answerToCheck:string  ): boolean;");
            return Task.CompletedTask;
        }
    }
}
