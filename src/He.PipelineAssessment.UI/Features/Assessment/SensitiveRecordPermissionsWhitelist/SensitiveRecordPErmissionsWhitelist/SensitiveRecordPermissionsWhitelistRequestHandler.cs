using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist
{
    public class SensitiveRecordPermissionsWhitelistRequestHandler : IRequestHandler<SensitiveRecordPermissionsWhitelistRequest, SensitiveRecordPermissionsWhitelistResponse>
    {
        private readonly IAssessmentRepository _repository;
        private readonly IMediator _mediator;

        public SensitiveRecordPermissionsWhitelistRequestHandler(IAssessmentRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;

        }

        public async Task<SensitiveRecordPermissionsWhitelistResponse> Handle(SensitiveRecordPermissionsWhitelistRequest request, CancellationToken cancellationToken)
        {
            var assessmentSummary = await _mediator.Send(new AssessmentSummaryRequest(request.AssessmentId, 0), cancellationToken);

            var whitelistRecords = await _repository.GetSensitiveRecordWhitelist(request.AssessmentId);

            var permissions = whitelistRecords.Select(x => new SensitiveRecordPermissionsWhitelistDto
            {
                Id = x.Id,
                Email = x.Email,
                DateAdded = x.CreatedDateTime,
                AddedBy = x.CreatedBy
            }).ToList();

            return new SensitiveRecordPermissionsWhitelistResponse
            {
                Permissions = permissions,
                AssessmentSummary = assessmentSummary
            };

        }
    }
}
