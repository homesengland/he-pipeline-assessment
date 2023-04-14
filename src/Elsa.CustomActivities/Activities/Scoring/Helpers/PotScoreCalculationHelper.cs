﻿using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.Persistence;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elsa.CustomActivities.Activities.Scoring.Helpers
{
    public class PotScoreCalculationHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<PotScoreCalculationHelper> _logger;

        public PotScoreCalculationHelper(IElsaCustomRepository elsaCustomRepository, ILogger<PotScoreCalculationHelper> logger)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }

        public async Task<double> GetTotalPotValue(string workflowInstanceId, string potValue)
        {
            try
            {
                double failedResult = -1;
                List<double> totalSelectedScores = new List<double>();

                var workflowQuestions = await _elsaCustomRepository.GetQuestions(workflowInstanceId, CancellationToken.None);
                if (workflowQuestions.Count > 0)
                {
                    foreach (var question in workflowQuestions)
                    {
                        if(question != null && TryGetMatchingPotScoreWeighting(question, potValue, out double potScoreWeighting))
                        {
                            totalSelectedScores.Add(potScoreWeighting);
                        }
                    }
                    return totalSelectedScores.Sum();
                }
                return failedResult;
            }
            catch(Exception e)
            {
                _logger.LogError(string.Format("Error whilst retrieving Pot Value: {0}", potValue), e);
                throw (new Exception(e.Message, e.InnerException));
            }

        }

        public async Task<string> GetPotScore(string workflowInstanceId)
        {
            string failedResult = string.Empty;

            QuestionWorkflowInstance? workflow = await _elsaCustomRepository.GetQuestionWorkflowInstance(workflowInstanceId, CancellationToken.None);
            if(workflow != null && workflow.Score != null)
            {
                return workflow.Score;
            }
            else
            {
                return failedResult;
            }
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("getTotalPotValue", (Func<string, double>)((potValueName) => GetTotalPotValue(activityExecutionContext.WorkflowInstance.Id, potValueName).Result));
            engine.SetValue("getPotScore", (Func<string>)(() => GetPotScore(activityExecutionContext.WorkflowInstance.Id).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function getTotalPotValue(potValueName:string ): number;");
            output.AppendLine("declare function getPotScore(): string;");
            return Task.CompletedTask;
        }

        private bool TryGetMatchingPotScoreWeighting(Question question, string potToMatch, out double potScoreWeighting)
        {
            potScoreWeighting = 0;
            if (question.Answers?.Any() ?? false)
            {
                QuestionChoice? selectedChoice = question.Answers?.FirstOrDefault()!.Choice;
                if(selectedChoice != null && selectedChoice.PotScoreCategory != null)
                {
                    if(selectedChoice.PotScoreCategory.ToLower() == potToMatch.ToLower() && question.Weighting != null)
                    {
                        potScoreWeighting = question.Weighting.Value;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
