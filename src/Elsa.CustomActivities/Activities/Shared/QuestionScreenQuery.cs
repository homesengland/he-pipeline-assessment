using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using Elsa.Services.Models;
using MediatR;

namespace Elsa.CustomActivities.Activities.Shared
{



    public class QuestionScreenQuery : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public QuestionScreenQuery(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;

            engine.SetValue("getQuestionAnswer", (Func<string, string, string, QuestionScreenAnswer?>)((workflowName, activityName, questionId) => GetAssessmentQuestion(activityExecutionContext, workflowName, activityName, questionId).Result));
            engine.SetValue("getAnswer", (Func<string, string, string, string?>)((workflowName, activityName, questionId) => GetAnswer(activityExecutionContext, workflowName, activityName, questionId).Result));
            engine.SetValue("hasAllAnswers", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, answers) => HasAllAnswers(activityExecutionContext, workflowName, activityName, questionId, answers).Result));
            engine.SetValue("hasAnyAnswer", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, possibleAnswers) => HasAnyAnswer(activityExecutionContext, workflowName, activityName, questionId, possibleAnswers).Result));
            engine.SetValue("hasAnswer", (Func<string, string, string, string, bool>)((workflowName, activityName, questionId, answer) => HasAnswer(activityExecutionContext, workflowName, activityName, questionId, answer).Result));
            engine.SetValue("isAnswer", (Func<string, string, string, string, bool>)((workflowName, activityName, questionId, answer) => IsAnswer(activityExecutionContext, workflowName, activityName, questionId, answer).Result));


            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;

            output.AppendLine("declare function getQuestionAnswer(workflowName: string, activityName:string, questionId:string ): QuestionScreenAnswer;");
            output.AppendLine("declare function getAnswer(workflowName: string, activityName:string, questionId:string ): string;");
            output.AppendLine("declare function hasAllAnswers(workflowName: string, activityName:string, questionId:string, answers:string[] ): boolean;");
            output.AppendLine("declare function hasAnyAnswer(workflowName: string, activityName:string, questionId:string, possibleAnswers:string[] ): boolean;");
            output.AppendLine("declare function hasAnswer(workflowName: string, activityName:string, questionId:string, answer:string ): boolean;");
            output.AppendLine("declare function isAnswer(workflowName: string, activityName:string, questionId:string, answer:string ): boolean;");

            return Task.CompletedTask;
        }

        private async Task<QuestionScreenAnswer?> GetAssessmentQuestion(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Elsa.Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var result = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            return result;

        }

        private async Task<string?> GetAnswer(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Elsa.Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var result = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            return result.Answer;

        }

        private async Task<bool> HasAnswer(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string answer)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Elsa.Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var questionInstance = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            if (questionInstance.Answer != null && questionInstance!.Answer.ToLower().Contains(answer.ToLower()))
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        private async Task<bool> IsAnswer(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string answer)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Elsa.Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var questionInstance = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            if (questionInstance.Answer != null && questionInstance!.Answer.ToLower() == answer.ToLower())
            {
                return true;
            }
            else
            {
                return false;

            }

        }

        private async Task<bool> HasAllAnswers(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string[] answersToCheck)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Elsa.Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var questionInstance = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            bool areAllAnswersIncluded = false;
            foreach (var answer in answersToCheck)
            {
                if (questionInstance.Answer != null && questionInstance!.Answer.ToLower().Contains(answer.ToLower()))
                {
                    areAllAnswersIncluded = true;
                }
                else
                {
                    areAllAnswersIncluded = false;
                    break;
                }
            }
            return areAllAnswersIncluded;

        }

        private async Task<bool> HasAnyAnswer(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string[] answersToCheck)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Elsa.Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var questionInstance = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            bool areAllAnswersIncluded = false;
            foreach (var answer in answersToCheck)
            {
                if (questionInstance.Answer != null && questionInstance!.Answer.ToLower().Contains(answer.ToLower())) //TODO: Deserialise answer to list of string
                {
                    areAllAnswersIncluded = true;
                    break;
                }
                else
                {
                    areAllAnswersIncluded = false;
                }
            }
            return areAllAnswersIncluded;

        }
    }


}

