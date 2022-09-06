﻿using Elsa.CustomModels;
using Elsa.Server.Providers;

namespace Elsa.Server.Features.Shared.SaveAndContinue
{
    public interface ISaveAndContinueMapper
    {
        AssessmentQuestion SaveAndContinueCommandToNextMultipleChoiceQuestionModel(SaveAndContinueCommand command, string nextActivityId, string nextActivityType);
    }

    public class SaveAndContinueMapper : ISaveAndContinueMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public SaveAndContinueMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public AssessmentQuestion SaveAndContinueCommandToNextMultipleChoiceQuestionModel(SaveAndContinueCommand command, string nextActivityId, string nextActivityType)
        {
            return new AssessmentQuestion
            {
                Id = $"{command.WorkflowInstanceId}-{nextActivityId}",
                ActivityId = nextActivityId,
                ActivityType = nextActivityType,
                FinishWorkflow = false,
                NavigateBack = false,
                Answer = null,
                WorkflowInstanceId = command.WorkflowInstanceId,
                PreviousActivityId = command.ActivityId,
                CreatedDateTime = _dateTimeProvider.UtcNow()
            };
        }
    }
}
