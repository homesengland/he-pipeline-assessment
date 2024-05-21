using FluentValidation.Results;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.ManageCategory
{
    public class UpdateCategoryCommandDto
    {
        public int CatgeoryId { get; set; }
        public UpdateCategoryCommand UpdateCategoryCommand { get; set; } = new();
        public ValidationResult? ValidationResult { get; set; }
    }

    public class UpdateCategoryCommand : IRequest
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string CategoryOldName { get; set; } = null!;
    }
}
