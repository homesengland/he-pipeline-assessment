﻿using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateAssessmentIntervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.LoadAssessmentInterventionCheckYourAnswers
{
    public class CreateAssessmentInterventionRequestHandler : IRequestHandler<CreateAssessmentInterventionCommand, CreateAssessmentInterventionDto>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IUserProvider _userProvider;

        public CreateAssessmentInterventionRequestHandler(IAssessmentRepository assessmentRepository, IUserProvider userProvider)
        {
            _assessmentRepository = assessmentRepository;
            _userProvider = userProvider;
        }

        public Task<CreateAssessmentInterventionDto> Handle(CreateAssessmentInterventionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
