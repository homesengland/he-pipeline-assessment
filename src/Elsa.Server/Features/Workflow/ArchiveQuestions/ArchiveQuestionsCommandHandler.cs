using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.ArchiveQuestions
{
    public class ArchiveQuestionsCommandHandler : IRequestHandler<ArchiveQuestionsCommand, OperationResult<ArchiveQuestionsCommandResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<QuestionScreenSaveAndContinueCommandHandler> _logger;


        public ArchiveQuestionsCommandHandler(
            IElsaCustomRepository elsaCustomRepository, 
            ILogger<QuestionScreenSaveAndContinueCommandHandler> logger)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }

        public async Task<OperationResult<ArchiveQuestionsCommandResponse>> Handle(ArchiveQuestionsCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<ArchiveQuestionsCommandResponse>();
            try
            {
                await _elsaCustomRepository.ArchiveQuestions(request.WorkflowInstanceIds, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }
    }
}
