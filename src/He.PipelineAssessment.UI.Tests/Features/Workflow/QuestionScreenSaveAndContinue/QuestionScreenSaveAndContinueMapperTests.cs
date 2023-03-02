using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.SaveAndContinue
{
    public class QuestionScreenSaveAndContinueMapperTests
    {

        [Theory]
        [AutoMoqData]
        public void SaveAndContinueCommandToMultiSaveAndContinueCommandDto_Returns(
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            QuestionScreenSaveAndContinueMapper sut
        )
        {
            //Arrange

            //Act
            var result = sut.SaveAndContinueCommandToMultiSaveAndContinueCommandDto(saveAndContinueCommand);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(saveAndContinueCommand.Data.ActivityId, result.ActivityId);
            Assert.Equal(saveAndContinueCommand.Data.WorkflowInstanceId, result.WorkflowInstanceId);
            Assert.Equal(saveAndContinueCommand.Data.QuestionScreenAnswers?.Select(x => new Answer(x.QuestionId, x.Answer, x.Comments)).ToList(), result.Answers);
        }
    }
}
