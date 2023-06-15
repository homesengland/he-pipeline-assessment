using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.CreateRollback
{
    public class CreateRollbackCommandHandler : IRequestHandler<CreateRollbackCommand, int>
    {
        private readonly ICreateRollbackMapper _mapper;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentToolWorkflowInstanceHelpers _assessmentToolWorkflowInstanceHelpers;

        public CreateRollbackCommandHandler(IAssessmentRepository assessmentRepository, ICreateRollbackMapper mapper, ILogger<CreateRollbackCommandHandler> logger, IAssessmentToolWorkflowInstanceHelpers assessmentToolWorkflowInstanceHelpers)
        {
            _assessmentRepository = assessmentRepository;
            _mapper = mapper;
            _assessmentToolWorkflowInstanceHelpers = assessmentToolWorkflowInstanceHelpers;
        }


        public async Task<int> Handle(CreateRollbackCommand command, CancellationToken cancellationToken)
        {
            AssessmentToolWorkflowInstance? workflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId);
            if (workflowInstance == null)
            {
                throw new NotFoundException($"Assessment Tool Workflow Instance with Id {command.WorkflowInstanceId} not found");
            }

            var isLatest = _assessmentToolWorkflowInstanceHelpers.IsLatestSubmittedWorkflow(workflowInstance);
            if (!isLatest)
            {
                throw new Exception(
                    $"Unable to create  for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment.");
            }

            var assessmentIntervention = _mapper.CreateRollbackCommandToAssessmentIntervention(command);

            await _assessmentRepository.CreateAssessmentIntervention(assessmentIntervention);

            return assessmentIntervention.Id;
        }
    }
}
