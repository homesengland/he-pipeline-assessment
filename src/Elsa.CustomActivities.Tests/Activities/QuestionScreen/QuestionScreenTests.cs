using Elsa.Activities.ControlFlow;
using Elsa.ActivityResults;
using Elsa.CustomModels;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.QuestionScreen
{
    public class QuestionScreenTests
    {
        [Theory]
        [AutoMoqData]
        public async Task OnResumeAsyncReturnsCombinedResultWithDefaultOutcomeAndSuspendResult_GivenCorrectContextWithNoMatches(
            List<QuestionScreenAnswer> assessmentQuestion,
            CustomActivities.Activities.QuestionScreen.QuestionScreen sut)
        {
            //Arrange
            var context = new ActivityExecutionContext(default!, default!, default!, assessmentQuestion, default, default);

            sut.Cases = new List<SwitchCase>();

            //Act
            var result = await sut.ResumeAsync(context);

            //Assert
            Assert.NotNull(result);
            var combinedResult = (CombinedResult)result;
            Assert.Equal(2, combinedResult.Results.Count);
            var outcomeResult = (OutcomeResult)combinedResult.Results.First(x => x.GetType() == typeof(OutcomeResult));
            Assert.Equal("Default", outcomeResult.Outcomes.First());
            Assert.Contains(combinedResult.Results, x => x.GetType() == typeof(SuspendResult));
        }

        [Theory]
        [AutoMoqData]
        public async Task OnResumeAsyncReturnsCombinedResultWithMatchedOutcomeAndSuspendResult_GivenCorrectContextWithMatches(
            List<QuestionScreenAnswer> assessmentQuestion,
            CustomActivities.Activities.QuestionScreen.QuestionScreen sut)
        {
            //Arrange
            var context = new ActivityExecutionContext(default!, default!, default!, assessmentQuestion, default, default);
            sut.Cases = new List<SwitchCase>();
            sut.Cases.Add(new SwitchCase("Test", true));

            //Act
            var result = await sut.ResumeAsync(context);

            //Assert
            Assert.NotNull(result);
            var combinedResult = (CombinedResult)result;
            Assert.Equal(2, combinedResult.Results.Count);
            var outcomeResult = (OutcomeResult)combinedResult.Results.First(x => x.GetType() == typeof(OutcomeResult));
            Assert.Equal("Test", outcomeResult.Outcomes.First());
            Assert.Contains(combinedResult.Results, x => x.GetType() == typeof(SuspendResult));
        }

        [Theory, AutoMoqData]
        public async Task OnExecuteReturnsSuspendResult_GivenActivityExecutionContext(CustomActivities.Activities.QuestionScreen.QuestionScreen sut)
        {
            //Arrange
            var context = new ActivityExecutionContext(default!, default!, default!, null, default, default);

            //Act
            var result = await sut.ExecuteAsync(context);

            //Assert
            Assert.IsType<SuspendResult>(result);
        }
    }
}
