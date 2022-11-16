//using Elsa.CustomWorkflow.Sdk.Models.Workflow;
//using He.PipelineAssessment.Common.Tests;
//using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
//using Xunit;

//namespace He.PipelineAssessment.UI.Tests.Features.Workflow.SaveAndContinue
//{
//    public class SaveAndContinueMapperTests
//    {
//        [Theory]
//        [AutoMoqData]
//        public void SaveAndContinueCommandToSaveAndContinueCommandDto_Returns(
//            SaveAndContinueCommand saveAndContinueCommand,
//            SaveAndContinueMapper sut
//            )
//        {
//            //Arrange

//            //Act
//            var result = sut.SaveAndContinueCommandToSaveAndContinueCommandDto(saveAndContinueCommand);

//            //Assert
//            Assert.NotNull(result);
//            Assert.Equal(saveAndContinueCommand.Data.ActivityId, result.ActivityId);
//            Assert.Equal(saveAndContinueCommand.Data.WorkflowInstanceId, result.WorkflowInstanceId);
//            Assert.Equal($"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}", result.Id);
//            Assert.Equal(saveAndContinueCommand.Data.QuestionActivityData!.Answer, result.Answer);
//        }

//        [Theory]
//        [AutoMoqData]
//        public void SaveAndContinueCommandToMultiSaveAndContinueCommandDto_Returns(
//            SaveAndContinueCommand saveAndContinueCommand,
//            SaveAndContinueMapper sut
//        )
//        {
//            //Arrange

//            //Act
//            var result = sut.SaveAndContinueCommandToMultiSaveAndContinueCommandDto(saveAndContinueCommand);

//            //Assert
//            Assert.NotNull(result);
//            Assert.Equal(saveAndContinueCommand.Data.ActivityId, result.ActivityId);
//            Assert.Equal(saveAndContinueCommand.Data.WorkflowInstanceId, result.WorkflowInstanceId);
//            Assert.Equal($"{saveAndContinueCommand.Data.WorkflowInstanceId}-{saveAndContinueCommand.Data.ActivityId}", result.Id);
//            Assert.Equal(saveAndContinueCommand.Data.MultiQuestionActivityData?.Select(x => new Answer(x.QuestionId, x.Answer, x.Comments)).ToList(), result.Answers);
//        }
//    }
//}
