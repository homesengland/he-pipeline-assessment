using Auth0.ManagementApi.Models;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.PropertyDecorator;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Server.Api.Endpoints.ActivityStats;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.RegionalFigs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Elsa.CustomActivities.Activities.AssignFund
{
    [Action(
Category = "Homes England Activities",
Description = "Assign a fund to a given workflow",
Outcomes = new[] { OutcomeNames.Done }
)]
    public class AssignFund : Activity
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        public AssignFund(IElsaCustomRepository repo)
        {
            _elsaCustomRepository = repo;
        }

        [ActivityInput(Hint = "The Data Dictionary Id, or Intellisense", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public int DataDictionaryId { get;set; }

        [ActivityOutput] public string Output { get; set; } = null!;

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(context.WorkflowInstance.DefinitionId), context.WorkflowInstance.DefinitionId);

            return await Task.FromResult(Suspend());
        }


        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(DataDictionaryId), DataDictionaryId);

            var question = await _elsaCustomRepository.GetQuestionByDataDictionary(context.WorkflowInstance.CorrelationId, DataDictionaryId, CancellationToken.None);
            if (question != null)
            {
                if (question?.Answers != null)
                {
                    this.Output = string.Join(',', question.Answers.Select(x => x.AnswerText));
                    //Send a ServiceBus Message at this point.
                }
                else
                {
                    context.JournalData.Add("Error",
                        string.Format("No answer recorded for Data Dictionary: {0}, for Workflow CorrelationId: {1}", DataDictionaryId, context.WorkflowInstance.CorrelationId));
                    this.Output = string.Empty;
                }
            }
            else
            {
                context.JournalData.Add("Error", string.Format("Unable to retrieve Question using DataDictionaryId: {0}", DataDictionaryId));
            }


            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes("Done"),
                new SuspendResult()
            }));
        }
    }
}
