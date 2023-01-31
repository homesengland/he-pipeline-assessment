using FluentValidation.Results;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool
{
    public class CreateAssessmentToolDto
    {
        public CreateAssessmentToolCommand CreateAssessmentToolCommand { get; set; }
        public ValidationResult? ValidationResult { get; set; }

    }

    public class CreateAssessmentToolCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
}
