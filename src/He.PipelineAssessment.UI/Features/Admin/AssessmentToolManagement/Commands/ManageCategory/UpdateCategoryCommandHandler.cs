using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using MediatR;
using Microsoft.Xrm.Sdk;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.ManageCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IAdminCategoryRepository _adminCategoryRepository;
        private readonly ILogger<UpdateCategoryCommandHandler> _logger;
        private readonly IAdminAssessmentToolWorkflowRepository _workflowRepository;

        public UpdateCategoryCommandHandler(IAdminCategoryRepository adminCategoryRepository
            , ILogger<UpdateCategoryCommandHandler> logger
            , IAdminAssessmentToolWorkflowRepository workflowRepository)
        {
            _adminCategoryRepository = adminCategoryRepository;
            _logger = logger;
            _workflowRepository = workflowRepository;
        }

        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = _adminCategoryRepository.GetCategories().Result.Where(x => x.CategoryName == request.CategoryOldName);
                if(category.Any())
                {
                    await _adminCategoryRepository.UpdateCategory(request.CategoryId, request.CategoryName);
                    await _workflowRepository.UpdateWorkflowNameAndCategory(request.CategoryOldName, request.CategoryName);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException("Unable to update category.");
            }
        }
    }
}
