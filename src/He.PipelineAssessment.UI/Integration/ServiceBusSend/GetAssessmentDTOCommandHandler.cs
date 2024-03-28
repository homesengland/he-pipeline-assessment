using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class GetAssessmentDTOCommandHandler : IRequestHandler<GetAssessmentDTOCommand, GetAssessmentDTOCommandResponse?>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<GetAssessmentDTOCommandHandler> _logger;

        public GetAssessmentDTOCommandHandler(
            IAssessmentRepository assessmentRepository,
            ILogger<GetAssessmentDTOCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        public async Task<GetAssessmentDTOCommandResponse?> Handle(GetAssessmentDTOCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var assessmentToolWorkflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);

                GetAssessmentDTOCommandResponse result = new GetAssessmentDTOCommandResponse(){ };
                
                if (assessmentToolWorkflowInstance != null)
                {
                    result.Assessment.AssessmentToolId = assessmentToolWorkflowInstance.AssessmentToolWorkflowId!.Value;
                    result.Assessment.CreatedDateTime = assessmentToolWorkflowInstance.CreatedDateTime;
                    result.Assessment.SubmittedDateTime = assessmentToolWorkflowInstance.SubmittedDateTime;
                    result.Assessment.Status = assessmentToolWorkflowInstance.Status;
                    result.Assessment.InstanceId = assessmentToolWorkflowInstance.Id;
                    result.Assessment.WorkflowInstanceId = assessmentToolWorkflowInstance.WorkflowInstanceId;
                    result.Assessment.Result = assessmentToolWorkflowInstance.Result;
                    result.Assessment.ProjectId = assessmentToolWorkflowInstance.Assessment.SpId;
                    result.Assessment.SubmittedBy = assessmentToolWorkflowInstance.SubmittedBy;
                    result.Assessment.Name = string.Compare(result.Assessment.Status, "Suspended - RB", true)==0
                        ? $"{assessmentToolWorkflowInstance.AssessmentToolWorkflow.Name} - {assessmentToolWorkflowInstance.WorkflowName}"
                        : result.Assessment.Name = assessmentToolWorkflowInstance.AssessmentToolWorkflow.Name;
                }

                return await Task.FromResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
