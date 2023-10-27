using Elsa.CustomWorkflow.Sdk.HttpClients;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Amendment.SubmitAmendment
{
    
    public class SubmitAmendmentCommandHandler : IRequestHandler<SubmitAmendmentCommand>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<SubmitAmendmentCommandHandler> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IInterventionService _interventionService;

        public SubmitAmendmentCommandHandler(IAssessmentRepository assessmentRepository, IDateTimeProvider dateTimeProvider, ILogger<SubmitAmendmentCommandHandler> logger, IElsaServerHttpClient elsaServerHttpClient)
        {
            _assessmentRepository = assessmentRepository;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
            _elsaServerHttpClient = elsaServerHttpClient;
        }

        public SubmitAmendmentCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task Handle(SubmitAmendmentCommand command, CancellationToken cancellationToken)
        {
            await _interventionService.SubmitIntervention(command);
        }
    }
}
