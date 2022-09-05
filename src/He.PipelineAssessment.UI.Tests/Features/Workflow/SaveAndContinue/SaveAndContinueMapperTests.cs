using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void SaveAndContinueCommandToSaveAndContinueCommandDto_Returns(
            SaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueMapper sut
            )
        {
            //Arrange
            var choiceList = saveAndContinueCommand.Data.MultipleChoiceQuestionActivityData!.Choices.Where(x => x.IsSelected).Select(choice => choice.Answer).ToList();

            //Act
            var result = sut.SaveAndContinueCommandToMultipleChoiceSaveAndContinueCommandDto(saveAndContinueCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(saveAndContinueCommand.Data.ActivityId, result.ActivityId);
            Assert.Equal(saveAndContinueCommand.Data.WorkflowInstanceId, result.WorkflowInstanceId);
            Assert.Equal($"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}", result.Id);
            Assert.Equal(choiceList, result.Answers);
        }
    }
}
