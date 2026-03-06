using MediatR;
using System.ComponentModel.DataAnnotations;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentFund
{
    public class UpdateAssessmentFundCommandDTO
    {
        public UpdateAssessmentFundCommand UpdateAssessmentFundCommand { get; set; } = new();

    }

    public class UpdateAssessmentFundCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsEarlyStage { get; set; }
        public bool IsDisabled { get; set; }
    }
}
