using Azure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;
using Microsoft.IdentityModel.Abstractions;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentTool
{
    public class UpdateAssessmentToolCommandHandler : IRequestHandler<UpdateAssessmentToolCommand>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly ILogger<UpdateAssessmentToolCommandHandler> _logger;
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;

        public UpdateAssessmentToolCommandHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository, ILogger<UpdateAssessmentToolCommandHandler> logger, IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _logger = logger;
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository; 
        }

        public async Task Handle(UpdateAssessmentToolCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _adminAssessmentToolRepository.GetAssessmentToolById(request.Id);
                if (entity == null)
                {
                    throw new NotFoundException($"Assessment Tool with Id {request.Id} not found");
                }
                var oldValue = entity.Name;
                entity.Name = request.Name;
                entity.Order = request.Order;


                var response = await _adminAssessmentToolRepository.UpdateAssessmentTool(entity);

                if (response == 1)
                {
                    if (entity.AssessmentToolWorkflows != null)
                    {
                        foreach (var item in entity.AssessmentToolWorkflows)
                        {
                            item.Category = item.Category.Replace(oldValue, request.Name);
                            await _adminAssessmentToolWorkflowRepository.UpdateAssessmentToolWorkflow(item);
                        }
                    }
                   
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to update assessment tool. AssessmentToolID: {request.Id}");
            }

        }
    }
}
