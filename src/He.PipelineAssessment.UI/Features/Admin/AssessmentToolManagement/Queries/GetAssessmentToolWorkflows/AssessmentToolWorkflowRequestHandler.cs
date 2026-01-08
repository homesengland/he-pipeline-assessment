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

            // COMMENT: Sends a request to the mediator to fetch a list of all funds.
            // Creates a new instance of the FundsListRequest class and passes it to the Send method.
            // The mediator then processes this request and returns a FundsListResponse object that contains the list of funds.
            // this is needed to display the funds that are not archived and can be associated with the assessment tool workflows

            var allFunds = await _mediator.Send(new FundsListRequest());

            // COMMENT: The line below basically, in simple terms, filters out the funds that are not disabled and assigns the resulting list to the FundsDropDownListOptions property of the assessmentToolWorkflowListDto object.
            assessmentToolWorkflowListDto.FundsDropDownListOptions = allFunds.Funds.Where(f => !f.IsDisabled).ToList();

            return assessmentToolWorkflowListDto;
        }
    }
}
