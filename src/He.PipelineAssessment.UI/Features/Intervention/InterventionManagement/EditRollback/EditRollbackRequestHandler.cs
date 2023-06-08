﻿using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.EditRollback
{
    public class EditRollbackRequestHandler : IRequestHandler<EditRollbackRequest, AssessmentInterventionDto>
    {

        private readonly IAssessmentInterventionMapper _mapper;
        private readonly IAssessmentRepository _repository;
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;
        private readonly ILogger<EditRollbackRequestHandler> _logger;

        public EditRollbackRequestHandler(IAssessmentInterventionMapper mapper, 
            IAssessmentRepository repo, IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository, 
            ILogger<EditRollbackRequestHandler> logger)
        {
            _mapper = mapper;
            _repository = repo;
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
            _logger = logger;
        }
        public async Task<AssessmentInterventionDto> Handle(EditRollbackRequest request, CancellationToken cancellationToken)
        {
            AssessmentIntervention? intervention = await _repository.GetAssessmentIntervention(request.InterventionId);
            if (intervention == null)
            {
                throw new NotFoundException($"Assessment Intervention with Id {request.InterventionId} not found");
            }
            AssessmentInterventionCommand command = _mapper.AssessmentInterventionCommandFromAssessmentIntervention(intervention);
            var assessmentToolWorkflows = await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflows();
            if(assessmentToolWorkflows == null || !assessmentToolWorkflows.Any())
            {
                throw new NotFoundException($"No Assessment tool workflows found");
            }
            var dto = new AssessmentInterventionDto
            {
                AssessmentInterventionCommand = command,
                TargetWorkflowDefinitions =
                    _mapper.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows)
            };
            return dto;
        }
    }
}
