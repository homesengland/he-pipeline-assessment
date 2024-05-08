using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.ManageCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand>
    {
        private readonly IAdminCategoryRepository _adminCategoryRepository;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public CreateCategoryCommandHandler(IAdminCategoryRepository adminCategoryRepository, ILogger<CreateCategoryCommandHandler> logger)
        {
            _adminCategoryRepository = adminCategoryRepository;
            _logger = logger;
        }

        public async Task Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _adminCategoryRepository.CreateCategory(new Category
                {
                    CategoryName = request.CategoryName,
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to create category.");
            }
        }
    }
}
