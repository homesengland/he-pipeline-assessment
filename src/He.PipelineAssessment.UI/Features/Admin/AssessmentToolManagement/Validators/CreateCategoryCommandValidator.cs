using FluentValidation;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.ManageCategory;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        private IAdminCategoryRepository _adminCategoryRepository;
        public CreateCategoryCommandValidator(IAdminCategoryRepository adminCategoryRepository)
        {
            this._adminCategoryRepository = adminCategoryRepository;
            RuleFor(c => c.CategoryName).NotEmpty().WithMessage("The {PropertyName} cannot be empty");


            RuleFor(c => c)
              .Must((c) => BeUnique(c.CategoryName))
              .WithMessage("The Category Name must be unique");
        }

        private bool BeUnique(string categoryName)
        {
            var categories =
                _adminCategoryRepository.GetCategories().Result;
            if (categories != null && categories.Where(x => x.CategoryName == categoryName).Count() > 0)
                return false;
            return true;
        }
    }
}
