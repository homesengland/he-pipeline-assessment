using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands
{
    public class CreateAssessmentToolCommand : IRequest<CreateAssessmentToolData>
    {      
        public CreateAssessmentToolDto AssessmentToolDto { get; set; } = null!;
    }
}
