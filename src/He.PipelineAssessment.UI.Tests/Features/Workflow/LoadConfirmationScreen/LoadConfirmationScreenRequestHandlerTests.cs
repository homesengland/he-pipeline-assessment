using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen;
using He.PipelineAssessment.UI.Features.Shared;
using MediatR;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.LoadConfirmationScreen
{
    public class LoadConfirmationScreenRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsApplicationException_GivenHttpClientResponseIsNull(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IMediator> mediator,
            LoadConfirmationScreenRequest request,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange

            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync((WorkflowActivityDataDto?)null);

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            elsaServerHttpClient.Verify(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
            Assert.Equal("Failed to load Confirmation Screen activity.", ex.Message);
            mediator.Verify(x => x.Send(It.IsAny<UpdateAssessmentStatusRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsApplicationException_GivenNoAssessmentToolWorkflowInstanceFound(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IMediator> mediator,
            LoadConfirmationScreenRequest request,
            WorkflowActivityDataDto workflowActivityDataDto,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange

            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x =>
                    x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync((AssessmentToolWorkflowInstance?)null);

            //Act
                    var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal("Failed to load Confirmation Screen activity.", ex.Message);
            elsaServerHttpClient.Verify(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<UpdateAssessmentStatusRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsSaveAndContinueCommand_GivenNoErrorsEncountered(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IMediator> mediator,
            LoadConfirmationScreenRequest request,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            WorkflowActivityDataDto workflowActivityDataDto,
            bool isVariationAllowed,
            bool isLatestSubmittedWorkflow,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange
            assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x =>
                    x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(true);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId,
                assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);

            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsVariationAllowed(assessmentToolWorkflowInstance))
                .ReturnsAsync(isVariationAllowed);
            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsOrderEqualToLatestSubmittedWorkflowOrder(assessmentToolWorkflowInstance))
                .ReturnsAsync(isLatestSubmittedWorkflow);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<LoadConfirmationScreenResponse>(result);
            Assert.Equal(assessmentToolWorkflowInstance.Assessment.SpId, result!.CorrelationId);
            Assert.Equal(assessmentToolWorkflowInstance.AssessmentId, result.AssessmentId);
            Assert.Equal(isVariationAllowed, result.IsVariationAllowed);
            Assert.Equal(isLatestSubmittedWorkflow, result.IsLatestSubmittedWorkflow);
            Assert.Equal(assessmentToolWorkflowInstance.AssessmentToolWorkflow!.IsAmendable, result.IsAmendableWorkflow);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Once);
            elsaServerHttpClient.Verify(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
            mediator.Verify(x => x.Send(It.Is<UpdateAssessmentStatusRequest>(req => req.AssessmentId == assessmentToolWorkflowInstance.AssessmentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsSaveAndContinueCommandWithFalseFields_GivenNoErrorsEncounteredAndNoAssessmentToolWorkflow(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IMediator> mediator,
            LoadConfirmationScreenRequest request,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            WorkflowActivityDataDto workflowActivityDataDto,
            bool isVariationAllowed,
            bool isLatestSubmittedWorkflow,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange
            assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;
            assessmentToolWorkflowInstance.AssessmentToolWorkflow = null!;
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x =>
                    x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);

            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(true);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId,
                assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);


            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsVariationAllowed(assessmentToolWorkflowInstance))
                .ReturnsAsync(isVariationAllowed);
            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsOrderEqualToLatestSubmittedWorkflowOrder(assessmentToolWorkflowInstance))
                .ReturnsAsync(isLatestSubmittedWorkflow);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<LoadConfirmationScreenResponse>(result);
            Assert.Equal(assessmentToolWorkflowInstance.Assessment.SpId, result!.CorrelationId);
            Assert.Equal(assessmentToolWorkflowInstance.AssessmentId, result.AssessmentId);
            Assert.False(result.IsVariationAllowed);
            Assert.False(result.IsLatestSubmittedWorkflow);
            Assert.False(result.IsAmendableWorkflow);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Once);
            elsaServerHttpClient.Verify(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
            mediator.Verify(x => x.Send(It.Is<UpdateAssessmentStatusRequest>(req => req.AssessmentId == assessmentToolWorkflowInstance.AssessmentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsUnauthorisedException_GivenUserCannotViewSensitiveRecords(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IMediator> mediator,
            LoadConfirmationScreenRequest request,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            WorkflowActivityDataDto workflowActivityDataDto,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange
            assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x =>
                    x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(false);

            //Act
            var result = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"You do not have permission to access this resource.", result.Message);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Never);
            mediator.Verify(x => x.Send(It.IsAny<UpdateAssessmentStatusRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsSaveAndContinueCommandWithIsAuthorisedFalse_GivenUserIncorrectBusinessArea(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IMediator> mediator,
            LoadConfirmationScreenRequest request,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            WorkflowActivityDataDto workflowActivityDataDto,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange
            assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x =>
                    x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(true);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId,
                assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(false);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<LoadConfirmationScreenResponse>(result);
            Assert.Equal(0, result!.CorrelationId);
            Assert.Equal(0, result.AssessmentId);
            Assert.False(result.IsAuthorised);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Never);
            mediator.Verify(x => x.Send(It.IsAny<UpdateAssessmentStatusRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_CreatesNextWorkflows_GivenTheyDoNotExistAndNextWorkflowDefinitionIdsIsNotEmpty
        (
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IMediator> mediator,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            LoadConfirmationScreenRequest request,
            WorkflowActivityDataDto workflowActivityDataDto,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange
            assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;
            assessmentToolWorkflowInstance.IsVariation = false;
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x =>
                    x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(true);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId,
                assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);

            workflowActivityDataDto.Data.NextWorkflowDefinitionIds = "workflowDefinition1, workflowDefinition2";
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition1")).ReturnsAsync((AssessmentToolInstanceNextWorkflow?)null);
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition2")).ReturnsAsync((AssessmentToolInstanceNextWorkflow?)null);

            //Act
            await sut.Handle(request, CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x =>
                x.CreateAssessmentToolInstanceNextWorkflows(It.Is<List<AssessmentToolInstanceNextWorkflow>>(y =>
                    y.Count == 2 && y.Any(z => z.NextWorkflowDefinitionId == "workflowDefinition1"))));
            assessmentRepository.Verify(x =>
                x.CreateAssessmentToolInstanceNextWorkflows(It.Is<List<AssessmentToolInstanceNextWorkflow>>(y =>
                    y.Count == 2 && y.Any(z => z.NextWorkflowDefinitionId == "workflowDefinition2"))));
            mediator.Verify(x => x.Send(It.Is<UpdateAssessmentStatusRequest>(req => req.AssessmentId == assessmentToolWorkflowInstance.AssessmentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_CreatesOnlyNextWorkflowsWhichDoNotExist_GivenNextWorkflowDefinitionIdsIsNotEmpty
        (
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IMediator> mediator,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            LoadConfirmationScreenRequest request,
            WorkflowActivityDataDto workflowActivityDataDto,
            AssessmentToolInstanceNextWorkflow nextWorkflow1,
            LoadConfirmationScreenRequestHandler sut
        )
        {
            //Arrange
            assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;
            assessmentToolWorkflowInstance.IsVariation = false;
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x =>
                    x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(true);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId,
                assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);

            workflowActivityDataDto.Data.NextWorkflowDefinitionIds = "workflowDefinition1, workflowDefinition2";
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition1")).ReturnsAsync(nextWorkflow1);
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition2")).ReturnsAsync((AssessmentToolInstanceNextWorkflow?)null);

            //Act
            await sut.Handle(request, CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x =>
                x.CreateAssessmentToolInstanceNextWorkflows(It.Is<List<AssessmentToolInstanceNextWorkflow>>(y =>
                    y.First().NextWorkflowDefinitionId == "workflowDefinition2")));
            mediator.Verify(x => x.Send(It.Is<UpdateAssessmentStatusRequest>(req => req.AssessmentId == assessmentToolWorkflowInstance.AssessmentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_DoesNotCreateNextWorkflows_GivenTheyExistAndNextWorkflowDefinitionIdsIsNotEmpty
        (
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IMediator> mediator,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            LoadConfirmationScreenRequest request,
            WorkflowActivityDataDto workflowActivityDataDto,
            AssessmentToolInstanceNextWorkflow nextWorkflow1,
            AssessmentToolInstanceNextWorkflow nextWorkflow2,
            LoadConfirmationScreenRequestHandler sut
        )
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x =>
                    x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(true);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId,
                assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);

            workflowActivityDataDto.Data.NextWorkflowDefinitionIds = "workflowDefinition1, workflowDefinition2";
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition1")).ReturnsAsync(nextWorkflow1);
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition2")).ReturnsAsync(nextWorkflow2);

            //Act
            await sut.Handle(request, CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x =>
                x.CreateAssessmentToolInstanceNextWorkflows(It.Is<List<AssessmentToolInstanceNextWorkflow>>(z =>
                    z.Count == 2 && z.Any(y => y.NextWorkflowDefinitionId == "workflowDefinition1"))), Times.Never);
            assessmentRepository.Verify(x =>
                x.CreateAssessmentToolInstanceNextWorkflows(It.Is<List<AssessmentToolInstanceNextWorkflow>>(z =>
                    z.Count == 2 && z.Any(y => y.NextWorkflowDefinitionId == "workflowDefinition2"))), Times.Never);
            mediator.Verify(x => x.Send(It.Is<UpdateAssessmentStatusRequest>(req => req.AssessmentId == assessmentToolWorkflowInstance.AssessmentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_DoesNotAttemptToCreateNextWorkflows_GivenNextWorkflowDefinitionIdsIsEmpty
        (
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IMediator> mediator,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            LoadConfirmationScreenRequest request,
            WorkflowActivityDataDto workflowActivityDataDto,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x =>
                    x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(true);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId,
                assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);

            workflowActivityDataDto.Data.NextWorkflowDefinitionIds = string.Empty;

            //Act
            await sut.Handle(request, CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x =>
                    x.CreateAssessmentToolInstanceNextWorkflows(It.IsAny<List<AssessmentToolInstanceNextWorkflow>>()),
                Times.Never);
            mediator.Verify(x => x.Send(It.Is<UpdateAssessmentStatusRequest>(req => req.AssessmentId == assessmentToolWorkflowInstance.AssessmentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_DoesNotAttemptToCreateNextWorkflows_GivenNextWorkflowDefinitionIdsIsNull
        (
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IMediator> mediator,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            LoadConfirmationScreenRequest request,
            WorkflowActivityDataDto workflowActivityDataDto,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x =>
                    x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(true);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId,
                assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);

            workflowActivityDataDto.Data.NextWorkflowDefinitionIds = null;

            //Act
            await sut.Handle(request, CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x =>
                    x.CreateAssessmentToolInstanceNextWorkflows(It.IsAny<List<AssessmentToolInstanceNextWorkflow>>()),
                Times.Never);
            mediator.Verify(x => x.Send(It.Is<UpdateAssessmentStatusRequest>(req => req.AssessmentId == assessmentToolWorkflowInstance.AssessmentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_DoesNotAttemptToCreateNextWorkflows_GivenAssessmentTollWorkflowInstanceAlreadyExists
       (
           [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
           [Frozen] Mock<IAssessmentRepository> assessmentRepository,
           [Frozen] Mock<IRoleValidation> roleValidation,
           [Frozen] Mock<IMediator> mediator,
           AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
           LoadConfirmationScreenRequest request,
           WorkflowActivityDataDto workflowActivityDataDto,
           LoadConfirmationScreenRequestHandler sut,
           List<AssessmentToolWorkflowInstance> assessmentToolWorkflowInstances
       )
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x =>
                    x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(true);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId,
                assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);

            workflowActivityDataDto.Data.NextWorkflowDefinitionIds = "a1234";
            assessmentToolWorkflowInstances.First().WorkflowDefinitionId = "a1234";
            assessmentRepository.Setup(x =>
                    x.GetAssessmentToolWorkflowInstances(assessmentToolWorkflowInstance.AssessmentId))
                .ReturnsAsync(assessmentToolWorkflowInstances);
            assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;
            assessmentToolWorkflowInstance.IsVariation = false;

            //Act
            await sut.Handle(request, CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x =>
                    x.CreateAssessmentToolInstanceNextWorkflows(It.IsAny<List<AssessmentToolInstanceNextWorkflow>>()),
                Times.Never);
            mediator.Verify(x => x.Send(It.Is<UpdateAssessmentStatusRequest>(req => req.AssessmentId == assessmentToolWorkflowInstance.AssessmentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        //[Theory]
        //[AutoMoqData]
        //public async Task Handle_DoesNotCreateNextWorkflows_GivenCurrentInstanceIsVariation
        //(
        //    [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
        //    [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        //    AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
        //    LoadConfirmationScreenRequest request,
        //    WorkflowActivityDataDto workflowActivityDataDto,
        //    AssessmentToolInstanceNextWorkflow nextWorkflow1,
        //    AssessmentToolInstanceNextWorkflow nextWorkflow2,
        //    LoadConfirmationScreenRequestHandler sut
        //)
        //{
        //    //TODO this + maybe other scenarios
        //}
    }
}