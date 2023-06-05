﻿using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideRequestHandler : IRequestHandler<CreateOverrideRequest, AssessmentInterventionDto>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;
        private readonly IUserProvider _userProvider;
        private readonly IAssessmentInterventionMapper _mapper;
        private readonly ILogger<CreateOverrideRequestHandler> _logger;

        public CreateOverrideRequestHandler(IAssessmentRepository assessmentRepository, 
            IUserProvider userProvider, 
            IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository, 
            IAssessmentInterventionMapper mapper, ILogger<CreateOverrideRequestHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _userProvider = userProvider;
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AssessmentInterventionDto> Handle(CreateOverrideRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                AssessmentToolWorkflowInstance? workflowInstance =
                    await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);
                var dto = _mapper.AssessmentInterventionDtoFromWorkflowInstance(workflowInstance!, _userProvider.GetUserName()!, _userProvider.GetUserEmail()!);

                var assessmentToolWorkflows = await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflows();
                dto.TargetWorkflowDefinitions =
                    _mapper.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows);
                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AssessmentInterventionDto
                {

                };

            }
        }
    }
}
