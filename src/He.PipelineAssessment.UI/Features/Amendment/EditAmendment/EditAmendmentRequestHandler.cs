﻿using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Amendment.EditAmendment
{
    public class EditAmendmentRequestHandler : IRequestHandler<EditAmendmentRequest, AssessmentInterventionDto>
    {

        private readonly IAssessmentInterventionMapper _mapper;
        private readonly IAssessmentRepository _repository;
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;
        private readonly ILogger<EditAmendmentRequestHandler> _logger;
        private readonly IRoleValidation _roleValidation;

        public EditAmendmentRequestHandler(IAssessmentInterventionMapper mapper,
            IAssessmentRepository repo, IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository,
            ILogger<EditAmendmentRequestHandler> logger, IRoleValidation roleValidation)
        {
            _mapper = mapper;
            _repository = repo;
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
            _logger = logger;
            _roleValidation = roleValidation;
        }
        public async Task<AssessmentInterventionDto> Handle(EditAmendmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                AssessmentIntervention? intervention = await _repository.GetAssessmentIntervention(request.InterventionId);
                if (intervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {request.InterventionId} not found");
                }

                var isAuthorised = await _roleValidation.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId);
                if (!isAuthorised)
                {
                    throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
                }

                AssessmentInterventionCommand command = _mapper.AssessmentInterventionCommandFromAssessmentIntervention(intervention);
                var interventionReasons = await _repository.GetInterventionReasons();
                var dto = new AssessmentInterventionDto
                {
                    AssessmentInterventionCommand = command,
                    InterventionReasons = interventionReasons
                };
                return dto;
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to edit amendment. InterventionId: {request.InterventionId}");
            }

        }
    }
}