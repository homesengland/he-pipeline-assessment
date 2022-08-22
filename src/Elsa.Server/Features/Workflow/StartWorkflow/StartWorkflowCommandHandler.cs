using Elsa.Models;
using Elsa.Server.Data;
using Elsa.Server.Models;
using Elsa.Services;
using MediatR;

namespace Elsa.Server.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandler : IRequestHandler<StartWorkflowCommand, OperationResult<StartWorkflowResponse>>
    {
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IStartsWorkflow _workflowRunner;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;

        public StartWorkflowCommandHandler(IWorkflowRegistry workflowRegistry, IStartsWorkflow workflowRunner, IPipelineAssessmentRepository pipelineAssessmentRepository)
        {
            _workflowRegistry = workflowRegistry;
            _workflowRunner = workflowRunner;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
        }

        public async Task<OperationResult<StartWorkflowResponse>> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<StartWorkflowResponse>();
            try
            {
                var sampleWorkflow =
                    await _workflowRegistry.GetWorkflowAsync(request.WorkflowDefinitionId, VersionOptions.Published, cancellationToken: cancellationToken);
                var runWorkflowResult = await _workflowRunner.StartWorkflowAsync(sampleWorkflow!, cancellationToken: cancellationToken);

                var multipleChoiceQuestion = runWorkflowResult.ToMultipleChoiceQuestionModel();

                if (multipleChoiceQuestion != null)
                {
                    await _pipelineAssessmentRepository.CreateMultipleChoiceQuestionAsync(multipleChoiceQuestion, cancellationToken);
                    result.Data = runWorkflowResult.ToStartWorkflowResponse();
                }
                else
                {
                    result.ErrorMessages.Add("Failed to deserialize RunWorkflowResult");
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