using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention.Constants;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.Commands.CreateAssessmentIntervention
{
    public class CreateAssessmentInterventionRequestHandler : IRequestHandler<CreateAssessmentInterventionRequest, CreateAssessmentInterventionDto>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IUserProvider _userProvider;

        public CreateAssessmentInterventionRequestHandler(IAssessmentRepository assessmentRepository, IUserProvider userProvider)
        {
            _assessmentRepository = assessmentRepository;
            _userProvider = userProvider;
        }

        public async Task<CreateAssessmentInterventionDto> Handle(CreateAssessmentInterventionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                AssessmentToolWorkflowInstance? workflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);
                var dto = DtoFromWorkflowInstance(workflowInstance);
                return dto;
            }
            catch(Exception e)
            {
                return new CreateAssessmentInterventionDto
                {
                    
                };

            }
        }

        public CreateAssessmentInterventionDto DtoFromWorkflowInstance(AssessmentToolWorkflowInstance instance)
        {
            string? adminName = _userProvider.GetUserName();
            string? adminEmail = _userProvider.GetUserEmail();

            CreateAssessmentInterventionDto dto = new CreateAssessmentInterventionDto()
            {
                CreateAssessmentInterventionCommand = new CreateAssessmentInterventionCommand()
                {
                    AssessmentWorkflowInstanceId = instance.WorkflowInstanceId,
                    AssessmentResult = instance.Result,
                    AssessmentName = instance.WorkflowName,
                    RequestedBy = adminName,
                    RequestedByEmail = adminEmail,
                    Administrator = adminName,
                    AdministratorEmail = adminEmail,
                    DecisionType = InterventionDecisionTypes.Override,
                    Status = InterventionStatus.NotSubmitted
                }
            };
            return dto;
        }
    }
}
