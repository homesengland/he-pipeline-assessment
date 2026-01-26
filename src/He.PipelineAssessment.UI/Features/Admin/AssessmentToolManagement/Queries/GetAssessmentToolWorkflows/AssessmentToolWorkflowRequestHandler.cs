using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using He.PipelineAssessment.UI.Features.Funds.FundsList;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolWorkflows
{
    public class AssessmentToolWorkflowRequestHandler : IRequestHandler<AssessmentToolWorkflowQuery, AssessmentToolWorkflowListDto>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly IAssessmentToolMapper _assessmentToolMapper;
        private readonly IMediator _mediator;

       public AssessmentToolWorkflowRequestHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository, IAssessmentToolMapper assessmentToolMapper, IMediator mediator)
        //public AssessmentToolWorkflowRequestHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository, IAssessmentToolMapper assessmentToolMapper)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _assessmentToolMapper = assessmentToolMapper;
            _mediator = mediator;
        }

        public async Task<AssessmentToolWorkflowListDto> Handle(AssessmentToolWorkflowQuery query, CancellationToken cancellationToken)
        {
            var entity = await _adminAssessmentToolRepository.GetAssessmentToolById(query.AssessmentToolId);
            ArgumentNullException.ThrowIfNull(entity, "Assessment not found");
            var assessmentToolWorkflowListDto = new AssessmentToolWorkflowListDto
            {
                AssessmentToolId = entity.Id,
                AssessmentToolName = entity.Name
            };

            if (entity.AssessmentToolWorkflows != null)
            {
                assessmentToolWorkflowListDto.AssessmentToolWorkflowDtos = _assessmentToolMapper.AssessmentToolWorkflowsToAssessmentToolDto(entity.AssessmentToolWorkflows.ToList());

            }
            var allFunds = await _mediator.Send(new FundsListRequest());

            assessmentToolWorkflowListDto.FundsDropDownListOptions = allFunds.Funds.Where(f => !f.IsDisabled).ToList();

            return assessmentToolWorkflowListDto;
        }
    }
}
