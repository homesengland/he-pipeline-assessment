using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Models;
using Elsa.Services;
using MediatR;


namespace Elsa.Server.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandler : IRequestHandler<StartWorkflowCommand, OperationResult<StartWorkflowResponse>>
    {
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IStartsWorkflow _startsWorkflow;
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IStartWorkflowMapper _startWorkflowMapper;

        public StartWorkflowCommandHandler(IWorkflowRegistry workflowRegistry, IStartsWorkflow startsWorkflow, IElsaCustomRepository elsaCustomRepository, IStartWorkflowMapper startWorkflowMapper)
        {
            _workflowRegistry = workflowRegistry;
            _startsWorkflow = startsWorkflow;
            _elsaCustomRepository = elsaCustomRepository;
            _startWorkflowMapper = startWorkflowMapper;
        }

        public async Task<OperationResult<StartWorkflowResponse>> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<StartWorkflowResponse>();
            try
            {
                var workflow =
                    await _workflowRegistry.FindAsync(request.WorkflowDefinitionId, VersionOptions.Published, cancellationToken: cancellationToken);

                var workflowName = workflow!.Name != null ? workflow.Name : "undefined workflow";
                var runWorkflowResult = await _startsWorkflow.StartWorkflowAsync(workflow!, null, null, request.CorrelationId, cancellationToken: cancellationToken);


                if (runWorkflowResult.WorkflowInstance != null)
                {
                    var activity = workflow!.Activities.FirstOrDefault(x =>
                        x.Id == runWorkflowResult.WorkflowInstance.LastExecutedActivityId);

                    if (activity != null)
                    {
                        var customActivityNavigation =
                            _startWorkflowMapper.RunWorkflowResultToCustomNavigationActivity(runWorkflowResult, activity.Type);

                        if (customActivityNavigation != null)
                        {
                            await _elsaCustomRepository.CreateCustomActivityNavigationAsync(customActivityNavigation!, cancellationToken);
                            result.Data = _startWorkflowMapper.RunWorkflowResultToStartWorkflowResponse(runWorkflowResult, activity.Type, workflowName);
                        }
                        else
                        {
                            result.ErrorMessages.Add("Failed to deserialize RunWorkflowResult");
                        }

                        if (activity.Type == ActivityTypeConstants.QuestionScreen)
                        {
                            //create one for each question
                            var dictionList = runWorkflowResult.WorkflowInstance.ActivityData
                                .FirstOrDefault(x => x.Key == activity.Id).Value;

                            AssessmentQuestions? dictionaryQuestions = (AssessmentQuestions?)dictionList.FirstOrDefault(x => x.Key == "Questions").Value;

                            var questionList = (List<Question>)dictionaryQuestions!.Questions;
                            if (questionList!.Any())
                            {
                                var assessments = new List<QuestionScreenAnswer>();

                                foreach (var item in questionList!)
                                {
                                    var assessment =
                                        _startWorkflowMapper.RunWorkflowResultToQuestionScreenAnswer(runWorkflowResult,
                                            activity.Type, item);
                                    if (assessment != null)
                                    {
                                        assessments.Add(assessment);
                                    }
                                }
                                await _elsaCustomRepository.CreateQuestionScreenAnswersAsync(assessments, cancellationToken);
                            }
                        }
                    }
                    else
                    {
                        result.ErrorMessages.Add("Failed to get activity");
                    }
                }
                else
                {
                    result.ErrorMessages.Add("Workflow instance is null");
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