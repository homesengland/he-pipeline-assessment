using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using Elsa.Services.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.LoadCheckYourAnswersScreen
{
    public class LoadCheckYourAnswersRequestHandler : IRequestHandler<LoadCheckYourAnswersRequest, OperationResult<LoadCheckYourAnswersScreenResponse>>
    {
        private readonly IQuestionInvoker _questionInvoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public LoadCheckYourAnswersRequestHandler(IWorkflowInstanceStore workflowInstanceStore, IElsaCustomRepository elsaCustomRepository, IQuestionInvoker questionInvoker)
        {
            _workflowInstanceStore = workflowInstanceStore;
            _elsaCustomRepository = elsaCustomRepository;
            _questionInvoker = questionInvoker;
        }

        public async Task<OperationResult<LoadCheckYourAnswersScreenResponse>> Handle(LoadCheckYourAnswersRequest activityRequest, CancellationToken cancellationToken)
        {
            var result = new OperationResult<LoadCheckYourAnswersScreenResponse>
            {
                Data = new LoadCheckYourAnswersScreenResponse
                {
                    WorkflowInstanceId = activityRequest.WorkflowInstanceId,
                    ActivityId = activityRequest.ActivityId
                }
            };
            try
            {
                var dbActivity =
                    await _elsaCustomRepository.GetCustomActivityNavigation(activityRequest.ActivityId, activityRequest.WorkflowInstanceId, cancellationToken);

                if (dbActivity != null)
                {
                    IEnumerable<CollectedWorkflow> workflows = await _questionInvoker.FindWorkflowsAsync(activityRequest.ActivityId, dbActivity.ActivityType, activityRequest.WorkflowInstanceId, cancellationToken);

                    var collectedWorkflow = workflows.FirstOrDefault();
                    if (collectedWorkflow != null)
                    {
                        var workflowSpecification =
                            new WorkflowInstanceIdSpecification(collectedWorkflow.WorkflowInstanceId);
                        var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, cancellationToken: cancellationToken);
                        if (workflowInstance != null)
                        {

                            if (!workflowInstance.ActivityData.ContainsKey(activityRequest.ActivityId))
                            {
                                result.ErrorMessages.Add(
                                    $"Cannot find activity Id {activityRequest.ActivityId} in the workflow activity data dictionary");
                            }
                            else
                            {
                                var activityDataDictionary =
                                    workflowInstance.ActivityData
                                        .FirstOrDefault(a => a.Key == activityRequest.ActivityId).Value;

                                result.Data.ActivityType = dbActivity.ActivityType;
                                result.Data.PreviousActivityId = dbActivity.PreviousActivityId;
                                result.Data.PreviousActivityType = dbActivity.PreviousActivityType;

                                var questionScreenAnswers = await _elsaCustomRepository
                                    .GetQuestionScreenAnswers(workflowInstance.Id, cancellationToken);

                                result.Data.QuestionScreenAnswers = questionScreenAnswers;
                                //var model = new SummaryScreenModel
                                //{
                                //    AssessmentQuestions = assessmentQuestions.ToList(),
                                //    FooterText = activityDataDictionary["FooterText"]?.ToString(),
                                //    FooterTitle = activityDataDictionary["FooterTitle"]?.ToString()
                                //};
                                //result.Data.QuestionActivityData.SummaryScreen = model;
                            }
                        }
                        else
                        {
                            result.ErrorMessages.Add(
                                $"Unable to find workflow instance with Id: {activityRequest.WorkflowInstanceId} in Elsa database");
                        }
                    }
                    else
                    {
                        result.ErrorMessages.Add(
                            $"Unable to progress workflow instance Id {activityRequest.WorkflowInstanceId}. No collected workflows");
                    }
                }
                else
                {
                    result.ErrorMessages.Add(
                        $"Unable to find workflow instance with Id: {activityRequest.WorkflowInstanceId} and Activity Id: {activityRequest.ActivityId} in Pipeline Assessment database");
                }
            }
            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }
    }
}
