using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.ExecuteWorkflow
{
    public class ExecuteWorkflowCommandHandler : IRequestHandler<ExecuteWorkflowCommand, LoadQuestionScreenRequest?>
    {
        private readonly IRoleValidation _roleValidation;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        public ExecuteWorkflowCommandHandler(IRoleValidation roleValidation, IAssessmentRepository assessmentRepository, IElsaServerHttpClient elsaServerHttpClient)
        {
            _roleValidation = roleValidation;
            _assessmentRepository = assessmentRepository;
            _elsaServerHttpClient = elsaServerHttpClient;
        }

        public async Task<LoadQuestionScreenRequest?> Handle(ExecuteWorkflowCommand command, CancellationToken cancellationToken)
        {
            var assessmentWorkflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId);

            if (!await _roleValidation.ValidateRole(assessmentWorkflowInstance!.AssessmentId, assessmentWorkflowInstance!.WorkflowDefinitionId))
            {
                throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
            }

            var dto = new ExecuteWorkflowCommandDto()
            {
                WorkflowInstanceId = command.WorkflowInstanceId,
                ActivityType = command.ActivityType,
                ActivityId = command.ActivityId
            };

            var response = await _elsaServerHttpClient.PostExecuteWorkflow(dto);

            if (response != null)
            {
                var result = new LoadQuestionScreenRequest()
                {
                    ActivityId = response.Data.NextActivityId,
                    WorkflowInstanceId = response.Data.WorkflowInstanceId,
                    ActivityType = response.Data.ActivityType,
                };

                return await Task.FromResult(result);
            }
            else
            {
                return null;
            }
        }
    }
}
