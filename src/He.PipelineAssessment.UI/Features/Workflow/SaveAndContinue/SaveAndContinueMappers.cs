using Elsa.CustomWorkflow.Sdk.Models;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public interface ISaveAndContinueMapper
    {
        //MultipleChoice.SaveAndContinueCommandDto SaveAndContinueCommandToMultipleChoiceSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);

        //Currency.SaveAndContinueCommandDto
        //    SaveAndContinueCommandToCurrencySaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);

        //Date.SaveAndContinueCommandDto
        //    SaveAndContinueCommandToDateSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);

        //Text.SaveAndContinueCommandDto
        //    SaveAndContinueCommandToTextSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);
        SaveAndContinueCommandDto
            SaveAndContinueCommandToSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand);
    }

    public class SaveAndContinueMapper : ISaveAndContinueMapper
    {
        //public MultipleChoice.SaveAndContinueCommandDto SaveAndContinueCommandToMultipleChoiceSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand)
        //{
        //    var choiceList = saveAndContinueCommand.Data.MultipleChoiceQuestionActivityData!.Choices.Where(x => x.IsSelected).Select(choice => choice.Answer).ToList();

        //    return new MultipleChoice.SaveAndContinueCommandDto
        //    {
        //        Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
        //        Answers = choiceList,
        //        WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
        //        ActivityId = saveAndContinueCommand.Data.ActivityId
        //    };
        //}

        //public Currency.SaveAndContinueCommandDto SaveAndContinueCommandToCurrencySaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand)
        //{

        //    return new Currency.SaveAndContinueCommandDto
        //    {
        //        Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
        //        Answer = saveAndContinueCommand.Data.CurrencyQuestionActivityData!.Answer,
        //        WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
        //        ActivityId = saveAndContinueCommand.Data.ActivityId
        //    };
        //}

        //public Date.SaveAndContinueCommandDto SaveAndContinueCommandToDateSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand)
        //{
        //    DateTime? dateTime = null;

        //    if (saveAndContinueCommand.Data.DateQuestionActivityData!.Day.HasValue &&
        //        saveAndContinueCommand.Data.DateQuestionActivityData.Month.HasValue &&
        //    saveAndContinueCommand.Data.DateQuestionActivityData.Year.HasValue)
        //    {
        //        var dateString =
        //            $"{saveAndContinueCommand.Data.DateQuestionActivityData.Year.Value}-{saveAndContinueCommand.Data.DateQuestionActivityData.Month.Value}-{saveAndContinueCommand.Data.DateQuestionActivityData.Day.Value}";
        //        bool isValidDate = DateTime.TryParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime parsedDateTime);
        //        dateTime = isValidDate ? parsedDateTime : null;
        //    }

        //    return new Date.SaveAndContinueCommandDto
        //    {
        //        Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
        //        Answer = dateTime,
        //        WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
        //        ActivityId = saveAndContinueCommand.Data.ActivityId
        //    };
        //}

        //public Text.SaveAndContinueCommandDto SaveAndContinueCommandToTextSaveAndContinueCommandDto(SaveAndContinueCommand saveAndContinueCommand)
        //{
        //    return new Text.SaveAndContinueCommandDto
        //    {
        //        Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
        //        Answer = saveAndContinueCommand.Data.TextQuestionActivityData!.Answer,
        //        WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
        //        ActivityId = saveAndContinueCommand.Data.ActivityId
        //    };
        //}

        public SaveAndContinueCommandDto SaveAndContinueCommandToSaveAndContinueCommandDto(
            SaveAndContinueCommand saveAndContinueCommand)
        {
            return new SaveAndContinueCommandDto
            {
                Id = $"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}",
                Answer = saveAndContinueCommand.Data.QuestionActivityData!.Answer,
                WorkflowInstanceId = saveAndContinueCommand.Data.WorkflowInstanceId,
                ActivityId = saveAndContinueCommand.Data.ActivityId
            };
        }
    }
}
