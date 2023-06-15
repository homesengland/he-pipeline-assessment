using Azure.Core;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Override.CreateOverride
{
    public class CreateOverrideCommandHandler : IRequestHandler<CreateOverrideCommand, int>
    {
        private readonly ICreateOverrideMapper _mapper;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentToolWorkflowInstanceHelpers _assessmentToolWorkflowInstanceHelpers;

        public CreateOverrideCommandHandler(IAssessmentRepository assessmentRepository, ICreateOverrideMapper mapper, ILogger<CreateOverrideCommandHandler> logger, IAssessmentToolWorkflowInstanceHelpers assessmentToolWorkflowInstanceHelpers)
        {
            _assessmentRepository = assessmentRepository;
            _mapper = mapper;
            _assessmentToolWorkflowInstanceHelpers = assessmentToolWorkflowInstanceHelpers;
        }


        public async Task<int> Handle(CreateOverrideCommand command, CancellationToken cancellationToken)
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
                    $"Unable to create override for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment.");
            }

            var assessmentIntervention = _mapper.CreateOverrideCommandToAssessmentIntervention(command);

            await _assessmentRepository.CreateAssessmentIntervention(assessmentIntervention);

            return assessmentIntervention.Id;
        }
    }
}
