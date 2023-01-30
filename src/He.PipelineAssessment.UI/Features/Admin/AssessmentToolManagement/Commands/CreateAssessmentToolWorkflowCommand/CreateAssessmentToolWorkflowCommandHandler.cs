using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflowCommand
{
    public class CreateAssessmentToolWorkflowCommandHandler : IRequestHandler<CreateAssessmentToolWorkflowCommand, int>
    {
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;
        private readonly IAssessmentToolWorkflowMapper _assessmentToolWorkflowMapper;
        
        public CreateAssessmentToolWorkflowCommandHandler(IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository,
                                                          IAssessmentToolWorkflowMapper assessmentToolWorkflowMapper)
        {
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
            _assessmentToolWorkflowMapper = assessmentToolWorkflowMapper;
           
        }
        
        public async Task<int> Handle(CreateAssessmentToolWorkflowCommand request, CancellationToken cancellationToken)
        {
            var entity =  _assessmentToolWorkflowMapper.CreateAssessmentToolWorkflowCommandToAssessmentToolWorkflow(request);
            await _adminAssessmentToolWorkflowRepository.CreateAssessmentToolWorkflow(entity); 
                       
            return entity.Id;           
        }
    }
}
