using Elsa.CustomInfrastructure.Data.Repository;
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
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;
        private readonly IStartWorkflowMapper _startWorkflowMapper;

        public StartWorkflowCommandHandler(IWorkflowRegistry workflowRegistry, IStartsWorkflow startsWorkflow, IPipelineAssessmentRepository pipelineAssessmentRepository, IStartWorkflowMapper startWorkflowMapper)
        {
            _workflowRegistry = workflowRegistry;
            _startsWorkflow = startsWorkflow;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
            _startWorkflowMapper = startWorkflowMapper;
        }

        public async Task<OperationResult<StartWorkflowResponse>> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<StartWorkflowResponse>();
            try
            {
                var sampleWorkflow =
                    await _workflowRegistry.FindAsync(request.WorkflowDefinitionId, VersionOptions.Published, cancellationToken: cancellationToken);
                var runWorkflowResult = await _startsWorkflow.StartWorkflowAsync(sampleWorkflow!, cancellationToken: cancellationToken);

                if (runWorkflowResult.WorkflowInstance != null)
                {
                    var activity = sampleWorkflow!.Activities.FirstOrDefault(x =>
                        x.Id == runWorkflowResult.WorkflowInstance.LastExecutedActivityId);

                    if (activity != null)
                    {
                        var multipleChoiceQuestion =
                            _startWorkflowMapper.RunWorkflowResultToMultipleChoiceQuestionModel(runWorkflowResult, activity.Type);

                        if (multipleChoiceQuestion != null)
                        {
                            await _pipelineAssessmentRepository.CreateMultipleChoiceQuestionAsync(multipleChoiceQuestion!, cancellationToken);
                            result.Data = _startWorkflowMapper.RunWorkflowResultToStartWorkflowResponse(runWorkflowResult);
                        }
                        else
                        {
                            result.ErrorMessages.Add("Failed to deserialize RunWorkflowResult");
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