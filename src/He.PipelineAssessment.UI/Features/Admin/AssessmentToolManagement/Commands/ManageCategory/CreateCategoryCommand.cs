using FluentValidation.Results;
using He.PipelineAssessment.Models;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.ManageCategory
{

    public class ManageCategoriesDto
    {
        public CreateCategoryDto CreateCategoryDto { get; set; } = new();

        public UpdateCategoryCommandDto UpdateCategoryCommandDto { get; set; } = new();

    }
    public class CreateCategoryDto
    {
        public CreateCategoryCommand CreateCategoryCommand { get; set; } = new();
        public ValidationResult? ValidationResult { get; set; }

        public List<Category> Categories { get; set; } = null!;
    }

    public class CreateCategoryCommand : IRequest
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
