using He.PipelineAssessment.Models.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionList
{
    public class InterventionListRequest : IRequest<List<AssessmentInterventionViewModel>>
    {
        public bool CanSeeSensitiveRecords { get; set; }
        public string? Username { get; set; }
    }
}
