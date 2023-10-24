using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Ammendment.SubmitAmmendment;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Ammendment.LoadAmmendmentCheckYourAnswersAssessor
{
    public class LoadAmmendmentCheckYourAnswersAssessorRequestHandler : IRequestHandler<LoadAmmendmentCheckYourAnswersAssessorRequest, SubmitAmmendmentCommand>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentInterventionMapper _mapper;
        private readonly ILogger<LoadAmmendmentCheckYourAnswersAssessorRequest> _logger;

        public LoadAmmendmentCheckYourAnswersAssessorRequestHandler(IAssessmentRepository assessmentRepository,
            IAssessmentInterventionMapper mapper,
            ILogger<LoadAmmendmentCheckYourAnswersAssessorRequest> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<SubmitAmmendmentCommand> Handle(LoadAmmendmentCheckYourAnswersAssessorRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var intervention = await _assessmentRepository.GetAssessmentIntervention(request.InterventionId);
                if (intervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {request.InterventionId} not found");
                }
                var command = _mapper.AssessmentInterventionCommandFromAssessmentIntervention(intervention);
                var submitAmmendmentCommand = SerializedCommand(command);
                return submitAmmendmentCommand;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to load rollback check your answers. InterventionId: {request.InterventionId}");
            }
        }

        private SubmitAmmendmentCommand SerializedCommand(AssessmentInterventionCommand command)
        {
            var serializedCommand = JsonConvert.SerializeObject(command);
            var submitAmmendmentCommand = JsonConvert.DeserializeObject<SubmitAmmendmentCommand>(serializedCommand);
            if (submitAmmendmentCommand == null)
            {
                throw new ArgumentException($"Unable to deserialise AssessmentInterventionCommand: {JsonConvert.SerializeObject(command)} from mapper");
            }
            return submitAmmendmentCommand;
        }
    }
}
