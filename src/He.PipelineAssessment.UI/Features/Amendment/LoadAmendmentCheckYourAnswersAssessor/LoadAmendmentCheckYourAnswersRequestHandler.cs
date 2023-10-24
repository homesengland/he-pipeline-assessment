using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Amendment.SubmitAmendment;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Amendment.LoadAmendmentCheckYourAnswers
{
    public class LoadAmendmentCheckYourAnswersRequestHandler : IRequestHandler<LoadAmendmentCheckYourAnswersRequest, SubmitAmendmentCommand>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAssessmentInterventionMapper _mapper;
        private readonly ILogger<LoadAmendmentCheckYourAnswersRequest> _logger;

        public LoadAmendmentCheckYourAnswersRequestHandler(IAssessmentRepository assessmentRepository,
            IAssessmentInterventionMapper mapper,
            ILogger<LoadAmendmentCheckYourAnswersRequest> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<SubmitAmendmentCommand> Handle(LoadAmendmentCheckYourAnswersRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var intervention = await _assessmentRepository.GetAssessmentIntervention(request.InterventionId);
                if (intervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {request.InterventionId} not found");
                }
                var command = _mapper.AssessmentInterventionCommandFromAssessmentIntervention(intervention);
                var submitAmendmentCommand = SerializedCommand(command);
                return submitAmendmentCommand;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to load amendment check your answers. InterventionId: {request.InterventionId}");
            }
        }

        private SubmitAmendmentCommand SerializedCommand(AssessmentInterventionCommand command)
        {
            var serializedCommand = JsonConvert.SerializeObject(command);
            var submitAmendmentCommand = JsonConvert.DeserializeObject<SubmitAmendmentCommand>(serializedCommand);
            if (submitAmendmentCommand == null)
            {
                throw new ArgumentException($"Unable to deserialise AssessmentInterventionCommand: {JsonConvert.SerializeObject(command)} from mapper");
            }
            return submitAmendmentCommand;
        }
    }
}
