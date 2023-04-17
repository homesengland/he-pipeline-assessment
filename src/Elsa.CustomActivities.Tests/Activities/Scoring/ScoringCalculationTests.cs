using Elsa.ActivityResults;
using Elsa.CustomActivities.Activities.Scoring;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;
using Elsa.Services;
using Elsa.Services.Workflows;
using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Models;

namespace Elsa.CustomActivities.Tests.Activities.Scoring
{
    public class ScoringCalculationTests
    {
        [Theory]
        [AutoMoqData]
        public async Task OnExecuteAsyncReturnsSuspendResult_GivenEmptyCalculation(ScoringCalculation sut)
        {
            //Arrange
            var context = new ActivityExecutionContext(default!, default!, default!, default!, default, default);
            sut.Calculation = string.Empty;

            //Act 
            var result = await sut.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<SuspendResult>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task OnExecuteAsyncReturnsSuspendResult_GivenExceptionThrown(ScoringCalculation sut)
        {
            //Arrange
            var context = new ActivityExecutionContext(default!, default!, default!, default!, default, default);
            Assert.Null(context.WorkflowExecutionContext);

            //Act 
            var result = await sut.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<SuspendResult>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task OnExecuteAsyncSetsWorkflowInstanceScore_GivenWorkflowInstanceExists(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            QuestionWorkflowInstance questionWorkflowInstance,
            [WithAutofixtureResolution] WorkflowExecutionContext workflowExecutionContext,
            ScoringCalculation sut)
        {
            //Arrange
            var context = new ActivityExecutionContext(default!, workflowExecutionContext!, default!, default!, default, default);
            elsaCustomRepository
                .Setup(x => x.GetQuestionWorkflowInstanceByDefinitionId(context.WorkflowInstance.DefinitionId,
                    context.WorkflowInstance.CorrelationId,
                    CancellationToken.None)).ReturnsAsync(questionWorkflowInstance);

            //Act 
            var result = await sut.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OutcomeResult>(result);
            var outcomeResult = (OutcomeResult)result;
            Assert.Equal("Done", outcomeResult.Outcomes.First());
            Assert.Equal(sut.Calculation, questionWorkflowInstance.Score);
            Assert.Equal(context.WorkflowInstance.Id, questionWorkflowInstance.WorkflowInstanceId);
            elsaCustomRepository.Verify(
                x => x.SaveChanges(CancellationToken.None),
                Times.Once);
            elsaCustomRepository.Verify(
                x => x.CreateQuestionWorkflowInstance(It.IsAny<QuestionWorkflowInstance>(), CancellationToken.None),
                Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task OnExecuteAsyncCreatesNewQuestionWorkflowInstance_GivenWorkflowInstanceDoesNotExist(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [WithAutofixtureResolution] WorkflowExecutionContext workflowExecutionContext,
            ScoringCalculation sut)
        {
            //Arrange
            var context = new ActivityExecutionContext(default!, workflowExecutionContext!, default!, default!, default, default);
            elsaCustomRepository
                .Setup(x => x.GetQuestionWorkflowInstanceByDefinitionId(context.WorkflowInstance.DefinitionId,
                    context.WorkflowInstance.CorrelationId,
                    CancellationToken.None)).ReturnsAsync((QuestionWorkflowInstance?)null);

            //Act 
            var result = await sut.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OutcomeResult>(result);
            var outcomeResult = (OutcomeResult)result;
            Assert.Equal("Done", outcomeResult.Outcomes.First());
            elsaCustomRepository.Verify(
                x => x.SaveChanges(CancellationToken.None),
                Times.Never);

            elsaCustomRepository.Verify(
                x => x.CreateQuestionWorkflowInstance(It.Is<QuestionWorkflowInstance>(x =>
                    x.WorkflowInstanceId == context.WorkflowInstance.Id &&
                    x.WorkflowDefinitionId == context.WorkflowInstance.DefinitionId &&
                    x.CorrelationId == context.WorkflowInstance.CorrelationId &&
                    x.WorkflowName == context.WorkflowInstance.DefinitionId &&
                    x.Score == sut.Calculation), CancellationToken.None),
                Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task OnExecuteAsyncCreatesNewQuestionWorkflowInstanceWithWorkflowBlueprintName_GivenWorkflowInstanceDoesNotExist(
            [WithAutofixtureResolution] IServiceProvider serviceProvider,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            IWorkflowFactory workflowFactory,
            Mock<IWorkflowBlueprint> workflowBlueprint,
            WorkflowInstance instance,
            ScoringCalculation sut)
        {
            //Arrange
            var factory = new WorkflowExecutionContextForWorkflowBlueprintFactory(serviceProvider, workflowFactory);
            workflowBlueprint.SetupGet(x => x.Name).Returns("TestWorkflowBlueprintName");
            Mock.Get(workflowFactory)
                .Setup(x => x.InstantiateAsync(workflowBlueprint.Object, default, default, default, default))
                .Returns(() => Task.FromResult(instance));

            var workflowExecutionContext = await factory.CreateWorkflowExecutionContextAsync(workflowBlueprint.Object);
            var context = new ActivityExecutionContext(default!, workflowExecutionContext!, default!, default!, default, default);

            elsaCustomRepository
                .Setup(x => x.GetQuestionWorkflowInstanceByDefinitionId(context.WorkflowInstance.DefinitionId,
                    context.WorkflowInstance.CorrelationId,
                    CancellationToken.None)).ReturnsAsync((QuestionWorkflowInstance?)null);

            //Act 
            var result = await sut.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OutcomeResult>(result);
            var outcomeResult = (OutcomeResult)result;
            Assert.Equal("Done", outcomeResult.Outcomes.First());
            elsaCustomRepository.Verify(
                x => x.SaveChanges(CancellationToken.None),
                Times.Never);

            elsaCustomRepository.Verify(
                x => x.CreateQuestionWorkflowInstance(It.Is<QuestionWorkflowInstance>(x =>
                    x.WorkflowInstanceId == context.WorkflowInstance.Id &&
                    x.WorkflowDefinitionId == context.WorkflowInstance.DefinitionId &&
                    x.CorrelationId == context.WorkflowInstance.CorrelationId &&
                    x.WorkflowName == "TestWorkflowBlueprintName" &&
                    x.Score == sut.Calculation), CancellationToken.None),
                Times.Once);
        }
    }
}
