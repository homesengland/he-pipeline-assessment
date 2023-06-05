﻿using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.SubmitOverride;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.LoadOverrideCheckYourAnswers
{
    public class LoadOverrideCheckYourAnswersRequestHandler : IRequestHandler<LoadOverrideCheckYourAnswersRequest, SubmitOverrideCommand>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentInterventionMapper _mapper;
        private readonly ILogger<LoadOverrideCheckYourAnswersRequestHandler> _logger;

        public LoadOverrideCheckYourAnswersRequestHandler(IAssessmentRepository assessmentRepository, 
            IAssessmentInterventionMapper mapper, 
            ILogger<LoadOverrideCheckYourAnswersRequestHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<SubmitOverrideCommand> Handle(LoadOverrideCheckYourAnswersRequest request, CancellationToken cancellationToken)
        {

            var intervention = await _assessmentRepository.GetAssessmentIntervention(request.InterventionId);
            if (intervention == null)
            {
                throw new NotFoundException($"Assessment Intervention with Id {request.InterventionId} not found");
            }
            AssessmentInterventionCommand command = _mapper.AssessmentInterventionCommandFromAssessmentIntervention(intervention);
            if(command == null)
            {
                throw new ArgumentException($"Unable to map AssessmentInterventionCommand from intervention: {JsonConvert.SerializeObject(intervention)} from mapper");
            }
            var serializedCommand = JsonConvert.SerializeObject(command);
            var submitOverrideCommand = JsonConvert.DeserializeObject<SubmitOverrideCommand>(serializedCommand);
            if(submitOverrideCommand == null)
            {
                throw new ArgumentException($"Unable to deserialise SubmitOverrideCommand: {serializedCommand} from serialized AssessmentInterventionCommand");
            }
            return submitOverrideCommand;
        }
    }
}
