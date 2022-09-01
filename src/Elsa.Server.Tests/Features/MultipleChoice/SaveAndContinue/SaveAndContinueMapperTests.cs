using AutoFixture.Xunit2;
using Elsa.CustomModels;
using Elsa.Server.Features.MultipleChoice.SaveAndContinue;
using Elsa.Server.Features.Shared.SaveAndContinue;
using Elsa.Server.Providers;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.MultipleChoice.SaveAndContinue
{
    public class SaveAndContinueMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void SaveAndContinueCommandToNextMultipleChoiceQuestionModel_ShouldReturnMultipleChoiceQuestionModel(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            MultipleChoiceSaveAndContinueCommand saveAndContinueCommand,
            string nextActivityId,
            SaveAndContinueMapper sut
        )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.SaveAndContinueCommandToNextMultipleChoiceQuestionModel(saveAndContinueCommand, nextActivityId, null);

            //Assert
            Assert.IsType<MultipleChoiceQuestionModel>(result);
            Assert.Equal(
                $"{saveAndContinueCommand.WorkflowInstanceId}-{nextActivityId}",
                result.Id);
            Assert.Equal(nextActivityId, result!.ActivityId);
            Assert.False(result.FinishWorkflow);
            Assert.False(result.NavigateBack);
            Assert.Null(result.Answer);
            Assert.Equal(saveAndContinueCommand.WorkflowInstanceId, result.WorkflowInstanceId);
            Assert.Equal(saveAndContinueCommand.ActivityId, result.PreviousActivityId);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
        }
    }
}
