using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
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
                var runWorkflowResult = await _startsWorkflow.StartWorkflowAsync(workflow!, cancellationToken: cancellationToken);

                if (runWorkflowResult.WorkflowInstance != null)
                {
                    var activity = workflow!.Activities.FirstOrDefault(x =>
                        x.Id == runWorkflowResult.WorkflowInstance.LastExecutedActivityId);

                    if (activity != null)
                    {


                        var assessmentQuestion =
                            _startWorkflowMapper.RunWorkflowResultToAssessmentQuestion(runWorkflowResult, activity.Type);

                        if (assessmentQuestion != null)
                        {
                            await _elsaCustomRepository.CreateAssessmentQuestionAsync(assessmentQuestion!, cancellationToken);
                            result.Data = _startWorkflowMapper.RunWorkflowResultToStartWorkflowResponse(runWorkflowResult);
                        }
                        else
                        {
                            result.ErrorMessages.Add("Failed to deserialize RunWorkflowResult");
                        }

                        if (activity.Type == "QuestionScreen")
                        {
                            //create one for each question
                            var dictionList = runWorkflowResult.WorkflowInstance.ActivityData
                                .FirstOrDefault(x => x.Key == activity.Id).Value;

                            var dictionaryQuestions = dictionList.FirstOrDefault(x => x.Key == "Questions").Value;
                            if (dictionaryQuestions != null)
                            {
                                var questionList = (List<Question>)dictionaryQuestions;
                                if (questionList!.Any())
                                {
                                    var assessments = new List<AssessmentQuestion>();

                                    foreach (var item in questionList!)
                                    {
                                        var assessment =
                                            _startWorkflowMapper.RunWorkflowResultToAssessmentQuestion(runWorkflowResult,
                                                activity.Type, item);
                                        if (assessment != null)
                                        {
                                            assessments.Add(assessment);
                                        }
                                    }
                                    await _elsaCustomRepository.CreateAssessmentQuestionAsync(assessments, cancellationToken);
                                }
                            }
                        }
                    }
                    else
                    {
                        result.ErrorMessages.Add("Failed to get activity");
                    }
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