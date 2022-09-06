using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using Newtonsoft.Json;
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
            var choiceList = saveAndContinueCommand.Data.QuestionActivityData!.Choices.Where(x => x.IsSelected).Select(choice => choice.Answer).ToList();
            var stringAnswer = JsonConvert.SerializeObject(choiceList);

            //Act
            var result = sut.SaveAndContinueCommandToSaveAndContinueCommandDto(saveAndContinueCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(saveAndContinueCommand.Data.ActivityId, result.ActivityId);
            Assert.Equal(saveAndContinueCommand.Data.WorkflowInstanceId, result.WorkflowInstanceId);
            Assert.Equal($"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}", result.Id);
            Assert.Equal(stringAnswer, result.Answer);
        }
    }
}
