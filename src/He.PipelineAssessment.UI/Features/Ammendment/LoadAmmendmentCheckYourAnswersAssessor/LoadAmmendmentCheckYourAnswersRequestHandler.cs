using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Ammendment.SubmitAmmendment;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Ammendment.LoadAmmendmentCheckYourAnswers
{
    public class LoadAmmendmentCheckYourAnswersRequestHandler : IRequestHandler<LoadAmmendmentCheckYourAnswersRequest, SubmitAmmendmentCommand>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentInterventionMapper _mapper;
        private readonly ILogger<LoadAmmendmentCheckYourAnswersRequest> _logger;

        public LoadAmmendmentCheckYourAnswersRequestHandler(IAssessmentRepository assessmentRepository,
            IAssessmentInterventionMapper mapper,
            ILogger<LoadAmmendmentCheckYourAnswersRequest> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<SubmitAmmendmentCommand> Handle(LoadAmmendmentCheckYourAnswersRequest request, CancellationToken cancellationToken)
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
                throw new ApplicationException($"Unable to load ammendment check your answers. InterventionId: {request.InterventionId}");
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
