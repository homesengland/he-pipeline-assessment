using Elsa.Activities.ControlFlow;
using Elsa.ActivityResults;
using Elsa.CustomModels;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Microsoft.Extensions.DependencyInjection;
using NetBox.Extensions;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.QuestionScreen
{
    public class QuestionScreenTests
    {
        [Theory]
        [AutoMoqData]
        public async Task OnResumeAsyncReturnsCombinedResult_GivenCorrectNoInvalidValidations(
    List<Question> assessmentQuestion,
    ActivityBlueprint activityBlueprint,
    [WithAutofixtureResolution] WorkflowExecutionContext workflowExecutionContext,
    List<CustomActivities.Activities.QuestionScreen.Question> questions,
    CustomActivities.Activities.QuestionScreen.QuestionScreen sut)
        {
            //Arrange
            foreach (var question in questions)
            {
                foreach (var validation in question.Validations.Validations)
                {
                    validation.IsValid = true;
                    validation.ValidationMessage = null;
                }
            }
            sut.Questions.Questions = questions;
            var context = new ActivityExecutionContext(default!, workflowExecutionContext!, activityBlueprint, assessmentQuestion, default, default);
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
        public async Task OnResumeAsyncReturnsCombinedSuspendedResultAndFalseOutcome_GivenAnyInvalidValidations(
            List<Question> assessmentQuestion,
            ActivityBlueprint activityBlueprint,
            [WithAutofixtureResolution] WorkflowExecutionContext workflowExecutionContext,
            List<CustomActivities.Activities.QuestionScreen.Question> questions,
            CustomActivities.Activities.QuestionScreen.QuestionScreen sut)
        {
            //Arrange
            foreach (var question in questions)
            {
                foreach (var validation in question.Validations.Validations)
                {
                    validation.IsValid = false;
                }
            }
            sut.Questions.Questions = questions;
            var context = new ActivityExecutionContext(default!, workflowExecutionContext!, activityBlueprint, assessmentQuestion, default, default);
            sut.Cases = new List<SwitchCase>();

            //Act
            var result = await sut.ResumeAsync(context);

            //Assert
            Assert.NotNull(result);
            var combinedResult = (CombinedResult)result;
            Assert.Equal(2, combinedResult.Results.Count);
            var outcomeResult = (OutcomeResult)combinedResult.Results.First(x => x.GetType() == typeof(OutcomeResult));
            Assert.Equal("False", outcomeResult.Outcomes.First());
            Assert.Contains(combinedResult.Results, x => x.GetType() == typeof(SuspendResult));
        }

        [Theory]
        [AutoMoqData]
        public async Task OnResumeAsyncReturnsCombinedResultWithDefaultOutcomeAndSuspendResult_GivenCorrectContextWithNoMatches(
            List<Question> assessmentQuestion,
            ActivityBlueprint activityBlueprint,
            [WithAutofixtureResolution] WorkflowExecutionContext workflowExecutionContext,
            List<CustomActivities.Activities.QuestionScreen.Question> questions,
            CustomActivities.Activities.QuestionScreen.QuestionScreen sut)
        {
            //Arrange
            foreach(var question in questions)
            {
                foreach(var validation in question.Validations.Validations)
                {
                    validation.IsValid = true;
                    validation.ValidationMessage = null;
                }
            }
            sut.Questions.Questions = questions;
            var context = new ActivityExecutionContext(default!, workflowExecutionContext!, activityBlueprint, assessmentQuestion, default, default);
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
            List<Question> assessmentQuestion,
            ActivityBlueprint activityBlueprint,
            [WithAutofixtureResolution] WorkflowExecutionContext workflowExecutionContext,
                        List<CustomActivities.Activities.QuestionScreen.Question> questions,
            CustomActivities.Activities.QuestionScreen.QuestionScreen sut)
        {
            //Arrange
            foreach (var question in questions)
            {
                foreach (var validation in question.Validations.Validations)
                {
                    validation.IsValid = true;
                    validation.ValidationMessage = null;
                }
            }
            sut.Questions.Questions = questions;
            var context = new ActivityExecutionContext(default!, workflowExecutionContext!, activityBlueprint!, assessmentQuestion, default, default);
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
