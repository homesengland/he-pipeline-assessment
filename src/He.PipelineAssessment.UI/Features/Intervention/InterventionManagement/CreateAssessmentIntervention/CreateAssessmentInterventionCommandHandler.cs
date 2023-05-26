﻿using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateAssessmentIntervention
{
    public class CreateAssessmentInterventionCommandHandler : IRequestHandler<CreateAssessmentInterventionCommand, int>
    {
        private readonly ICreateAssessmentInterventionMapper _mapper;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IUserProvider _userProvider;

        public CreateAssessmentInterventionCommandHandler(IAssessmentRepository assessmentRepository, IUserProvider userProvider, IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository, ICreateAssessmentInterventionMapper mapper)
        {
            _assessmentRepository = assessmentRepository;
            _userProvider = userProvider;
            _mapper = mapper;
        }


        public async Task<int> Handle(CreateAssessmentInterventionCommand command, CancellationToken cancellationToken)
        {
            var assessmentIntervention = _mapper.CreateAssessmentInterventionCommandToAssessmentIntervention(command);

            var interventionId = await _assessmentRepository.CreateAssessmentIntervention(assessmentIntervention);

            return interventionId;
        }
    }
}
