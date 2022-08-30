using AutoFixture.Xunit2;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Server.Features.Workflow.LoadWorkflowActivity;
using Moq;
using System.Text.Json;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityMappersTests
    {
        [Theory]
        [InlineAutoMoqData("Yes")]
        [InlineAutoMoqData("Yes@@@No")]
        [InlineAutoMoqData("Yes And This")]
        [InlineAutoMoqData("Yes, but, yes and this")]
        [InlineAutoMoqData("Yes, but, yes and this@@@Yes@@@No")]
        [InlineAutoMoqData("Not in choice list")]
        public void ActivityDataDictionaryToActivityData_ShouldReturnCorrectSelectedValues_GivenListOfAnswers(
            string answers,
            [Frozen] Mock<ILoadWorkflowActivityJsonHelper> jsonHelper,
            WorkflowInstance workflowInstance,
            MultipleChoiceQuestionActivityData multipleChoiceQuestionActivityData,
            MultipleChoiceQuestionModel multipleChoiceQuestionModel,
            LoadWorkflowActivityMapper sut
        )
        {
            //Arrange
            var choiceList = new List<Choice>();
            choiceList.Add(new Choice() { Answer = "Yes", IsSelected = false, IsSingle = false });
            choiceList.Add(new Choice() { Answer = "No", IsSelected = false, IsSingle = false });
            choiceList.Add(new Choice() { Answer = "Yes And This", IsSelected = false, IsSingle = false });
            choiceList.Add(new Choice() { Answer = "Yes, but, yes and this", IsSelected = false, IsSingle = false });

            multipleChoiceQuestionActivityData.Choices = choiceList.ToArray();

            var activityDataDictionary = workflowInstance.ActivityData.FirstOrDefault().Value;
            multipleChoiceQuestionModel.Answer = answers;
            var answerList = answers.Split(Constants.StringSeparator);


            multipleChoiceQuestionActivityData.Output = JsonSerializer.Serialize(multipleChoiceQuestionModel);

            jsonHelper.Setup(x => x.ActivityDataDictionaryToActivityData(activityDataDictionary)).Returns(multipleChoiceQuestionActivityData);
            jsonHelper.Setup(x => x.ActivityOutputJsonToMultipleChoiceQuestionModel(It.IsAny<string>())).Returns(multipleChoiceQuestionModel);

            //Act
            var result = sut.ActivityDataDictionaryToActivityData(activityDataDictionary);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<MultipleChoiceQuestionActivityData>(result);
            Assert.Equal(multipleChoiceQuestionActivityData.Question, result!.Question);

            var expectedChoices = result.Choices.Where(x => answerList.Contains(x.Answer));

            Assert.Equal(expectedChoices.Count(), result.Choices.Count(x => x.IsSelected));
            foreach (var choice in result.Choices.Where(x => x.IsSelected).ToList())
            {
                Assert.Contains(choice.Answer, answerList);
            }

        }

        [Theory]
        [AutoMoqData]
        public void ActivityDataDictionaryToActivityData_ShouldReturnNull_GivenActivityDataDictionaryFailsToDeserialize(
            [Frozen] Mock<ILoadWorkflowActivityJsonHelper> jsonHelper,
            WorkflowInstance workflowInstance,
            LoadWorkflowActivityMapper sut
        )
        {
            //Arrange
            var activityDataDictionary = workflowInstance.ActivityData.FirstOrDefault().Value;

            jsonHelper.Setup(x => x.ActivityDataDictionaryToActivityData(activityDataDictionary)).Returns((MultipleChoiceQuestionActivityData?)null);

            //Act
            var result = sut.ActivityDataDictionaryToActivityData(activityDataDictionary);

            //Assert
            Assert.Null(result);


        }

        [Theory]
        [AutoMoqData]
        public void ActivityDataDictionaryToActivityData_ShouldNotReturnSelectedChoices_GivenOutputIsNull(
           [Frozen] Mock<ILoadWorkflowActivityJsonHelper> jsonHelper,
           WorkflowInstance workflowInstance,
           MultipleChoiceQuestionActivityData multipleChoiceQuestionActivityData,
           LoadWorkflowActivityMapper sut
       )
        {
            //Arrange
            var activityDataDictionary = workflowInstance.ActivityData.FirstOrDefault().Value;
            multipleChoiceQuestionActivityData.Output = null;

            jsonHelper.Setup(x => x.ActivityDataDictionaryToActivityData(activityDataDictionary)).Returns(multipleChoiceQuestionActivityData);

            //Act
            var result = sut.ActivityDataDictionaryToActivityData(activityDataDictionary);

            //Assert
            Assert.NotNull(result);
            Assert.Null(result!.Output);
            Assert.Empty(result.Choices.Where(x => x.IsSelected));
            jsonHelper.Verify(x => x.ActivityOutputJsonToMultipleChoiceQuestionModel(It.IsAny<string>()), Times.Never);

        }

        [Theory]
        [AutoMoqData]
        public void ActivityDataDictionaryToActivityData_ShouldNotReturnSelectedChoices_GivenMultipleChoiceModelFailsToDeserialise(
            [Frozen] Mock<ILoadWorkflowActivityJsonHelper> jsonHelper,
            WorkflowInstance workflowInstance,
            MultipleChoiceQuestionActivityData multipleChoiceQuestionActivityData,
            LoadWorkflowActivityMapper sut
        )
        {
            //Arrange
            var activityDataDictionary = workflowInstance.ActivityData.FirstOrDefault().Value;

            jsonHelper.Setup(x => x.ActivityDataDictionaryToActivityData(activityDataDictionary)).Returns(multipleChoiceQuestionActivityData);
            jsonHelper.Setup(x => x.ActivityOutputJsonToMultipleChoiceQuestionModel(It.IsAny<string>())).Returns((MultipleChoiceQuestionModel?)null);

            //Act
            var result = sut.ActivityDataDictionaryToActivityData(activityDataDictionary);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result!.Output);
            jsonHelper.Verify(x => x.ActivityOutputJsonToMultipleChoiceQuestionModel(It.IsAny<string>()), Times.Once);
            Assert.Empty(result.Choices.Where(x => x.IsSelected));

        }

        [Theory]
        [AutoMoqData]
        public void ActivityDataDictionaryToActivityData_ShouldNotReturnSelectedChoices_GivenMultipleChoiceModelHasNoAnswer(
            [Frozen] Mock<ILoadWorkflowActivityJsonHelper> jsonHelper,
            WorkflowInstance workflowInstance,
            MultipleChoiceQuestionActivityData multipleChoiceQuestionActivityData,
            MultipleChoiceQuestionModel multipleChoiceQuestionModel,
            LoadWorkflowActivityMapper sut
        )
        {
            //Arrange
            var activityDataDictionary = workflowInstance.ActivityData.FirstOrDefault().Value;

            multipleChoiceQuestionModel.Answer = null;

            jsonHelper.Setup(x => x.ActivityDataDictionaryToActivityData(activityDataDictionary)).Returns(multipleChoiceQuestionActivityData);
            jsonHelper.Setup(x => x.ActivityOutputJsonToMultipleChoiceQuestionModel(It.IsAny<string>())).Returns(multipleChoiceQuestionModel);

            //Act
            var result = sut.ActivityDataDictionaryToActivityData(activityDataDictionary);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result!.Output);
            jsonHelper.Verify(x => x.ActivityOutputJsonToMultipleChoiceQuestionModel(It.IsAny<string>()), Times.Once);
            Assert.Empty(result.Choices.Where(x => x.IsSelected));

        }


    }
}
