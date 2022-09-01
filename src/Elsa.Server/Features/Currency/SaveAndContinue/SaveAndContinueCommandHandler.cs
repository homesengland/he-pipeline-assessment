using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Server.Features.Shared;
using Elsa.Server.Features.Shared.SaveAndContinue;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using MediatR;

namespace Elsa.Server.Features.Currency.SaveAndContinue
{
    public class SaveAndContinueCommandHandler : IRequestHandler<SaveAndContinueCommand, OperationResult<SaveAndContinueResponse>>
    {
        private readonly ISaveAndContinueHandler _saveAndContinueHandler;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;
        private readonly IDateTimeProvider _dateTimeProvider;


        public SaveAndContinueCommandHandler(ISaveAndContinueHandler saveAndContinueHandler, IPipelineAssessmentRepository pipelineAssessmentRepository, IDateTimeProvider dateTimeProvider)
        {
            _saveAndContinueHandler = saveAndContinueHandler;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<OperationResult<SaveAndContinueResponse>> Handle(SaveAndContinueCommand command, CancellationToken cancellationToken)
        {
            var result = new OperationResult<SaveAndContinueResponse>();
            try
            {
                var dbMultipleChoiceQuestionModel =
                    await _pipelineAssessmentRepository.GetMultipleChoiceQuestions(command.ActivityId, command.WorkflowInstanceId, cancellationToken);
                if (dbMultipleChoiceQuestionModel != null)
                {
                    dbMultipleChoiceQuestionModel.SetAnswer(command.Answer, _dateTimeProvider.UtcNow());
                    await _pipelineAssessmentRepository.UpdateMultipleChoiceQuestion(dbMultipleChoiceQuestionModel,
                        cancellationToken);

                    result = await _saveAndContinueHandler.Handle(dbMultipleChoiceQuestionModel, command, cancellationToken);

                }
            }
            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }


    }
}
