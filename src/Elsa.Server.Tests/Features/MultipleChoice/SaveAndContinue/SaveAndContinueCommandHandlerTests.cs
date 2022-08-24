using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Features.Workflow.StartWorkflow;
using Elsa.Server.Models;
using Elsa.Services.Models;
using Elsa.Services;
using Moq;
using Xunit;
using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Features.MultipleChoice.SaveAndContinue;
using Elsa.Server.Providers;

namespace Elsa.Server.Tests.Features.MultipleChoice.SaveAndContinue
{
    public class SaveAndContinueCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful_AndDoesNotInsertNewQuestionIfAlreadytExists(
            [Frozen] Mock<IMultipleChoiceQuestionInvoker> multipleChoiceQuestionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
            [Frozen] Mock<ISaveAndContinueMapper> saveAndContinueMapper,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            MultipleChoiceQuestionModel currentMultipleChoiceQuestionModel,
            List<CollectedWorkflow> collectedWorkflows,
            WorkflowInstance workflowInstance,
            MultipleChoiceQuestionModel nextMultipleChoiceQuestionModel,
            SaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueResponse saveAndContinueResponse,
            SaveAndContinueCommandHandler sut)
        {
            var opResult = new OperationResult<SaveAndContinueResponse>()
            {
                Data = new SaveAndContinueResponse
                {
                    WorkflowInstanceId = saveAndContinueCommand.WorkflowInstanceId,
                    NextActivityId = workflowInstance.Output!.ActivityId
                },
                ErrorMessages = new List<string>(),
                ValidationMessages = new List<string>()
            };

            //Arrange
            pipelineAssessmentRepository.Setup(x => x.GetMultipleChoiceQuestions(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentMultipleChoiceQuestionModel);

            multipleChoiceQuestionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, currentMultipleChoiceQuestionModel, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore
                .Setup(x => x.FindAsync(
                    It.Is<WorkflowInstanceIdSpecification>(x => x.Id == collectedWorkflows.First().WorkflowInstanceId),
                    CancellationToken.None)).ReturnsAsync(workflowInstance);

            pipelineAssessmentRepository.Setup(x => x.GetMultipleChoiceQuestions(workflowInstance.Output.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(nextMultipleChoiceQuestionModel);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            pipelineAssessmentRepository.Verify(x => x.UpdateMultipleChoiceQuestion(currentMultipleChoiceQuestionModel, CancellationToken.None), Times.Once);
            pipelineAssessmentRepository.Verify(x => x.CreateMultipleChoiceQuestionAsync(nextMultipleChoiceQuestionModel, CancellationToken.None), Times.Never);
            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Empty(result.ErrorMessages);
            Assert.Empty(result.ValidationMessages);
        }
    }
}
