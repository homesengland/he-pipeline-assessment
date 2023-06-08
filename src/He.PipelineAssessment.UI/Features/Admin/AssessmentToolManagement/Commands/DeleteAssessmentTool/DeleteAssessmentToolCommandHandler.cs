using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentTool
{
    public class DeleteAssessmentToolCommandHandler : IRequestHandler<DeleteAssessmentToolCommand, int>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;

        public DeleteAssessmentToolCommandHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
        }

        public async Task<int> Handle(DeleteAssessmentToolCommand request, CancellationToken cancellationToken)
        {
            var entity = await _adminAssessmentToolRepository.GetAssessmentToolById(request.Id);
            if (entity == null)
            {
                throw new NotFoundException($"Assessment Tool with Id {request.Id} not found");
            }

            if (entity.AssessmentToolWorkflows != null)
            {
                foreach (var assessmentToolWorkflow in entity.AssessmentToolWorkflows)
                {
                    assessmentToolWorkflow.Status = AssessmentToolStatus.Deleted;
                }
            }

            entity.Status = AssessmentToolStatus.Deleted;

            return await _adminAssessmentToolRepository.SaveChanges();
        }
    }
}
