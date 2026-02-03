using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Services;
using AutoFixture.Xunit2;
using Xunit;
using Moq;
using He.PipelineAssessment.Infrastructure.Repository;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Intervention;
using Microsoft.Extensions.Logging;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using Newtonsoft.Json;
using He.PipelineAssessment.UI.Integration.ServiceBusSend;
using MediatR;
using He.PipelineAssessment.UI.Features.Shared;

namespace He.PipelineAssessment.UI.Tests.Services
{
    public class InterventionServicesTests
    {

        #region ConfirmIntervention

        [Theory]
        [AutoMoqData]
        public async Task ConfirmIntervention_Should_ThrowErrorIfRepositoryReturnsNull(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IServiceBusMessageSender> serviceBusMessageSender,
            [Frozen] Mock<IMediator> mediator,
            AssessmentInterventionCommand command)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync((AssessmentIntervention?)null);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object,
                serviceBusMessageSender.Object,
                mediator.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.ConfirmIntervention(command));

            //Assert
            Assert.Equal($"Assessment Intervention with Id {command.AssessmentInterventionId} not found", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task ConfirmIntervention_Should_ThrowApplicationError_GivenExceptionThrown(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IServiceBusMessageSender> serviceBusMessageSender,
            [Frozen] Mock<IMediator> mediator,
            AssessmentInterventionCommand command)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ThrowsAsync(new Exception("Sample Error"));

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object,
                serviceBusMessageSender.Object,
                mediator.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => service.ConfirmIntervention(command));

            //Assert
            Assert.Equal($"Confirm {command.DecisionType} failed. AssessmentInterventionId: {command.AssessmentInterventionId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task ConfirmIntervention_Should_UpdateAssessmentRepository_GivenValidData(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IServiceBusMessageSender> serviceBusMessageSender,
            [Frozen] Mock<IMediator> mediator,
             AssessmentIntervention intervention,
            AssessmentInterventionCommand command)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            assessmentRepository.Setup(x => x.UpdateAssessmentIntervention(intervention)).ReturnsAsync(1);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object,
                serviceBusMessageSender.Object,
                mediator.Object
                );

            //Act
            await service.ConfirmIntervention(command);

            //Assert
            assessmentRepository.Verify(
                x => x.GetAssessmentIntervention(command.AssessmentInterventionId),
                Times.Once);
            assessmentRepository.Verify(
                x => x.UpdateAssessmentIntervention(intervention),
                Times.Once);
        }

        #endregion

        #region SubmitIntervention

        [Theory]
        [AutoMoqData]
        public async Task SubmitIntervention_ShouldThrowException_GivenNoInterventionFoundInRepository(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IServiceBusMessageSender> serviceBusMessageSender,
            [Frozen] Mock<IMediator> mediator,
            AssessmentInterventionCommand command)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync((AssessmentIntervention?)null);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object,
                serviceBusMessageSender.Object,
                mediator.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.SubmitIntervention(command));

            //Assert
            Assert.Equal($"Assessment Intervention with Id {command.AssessmentInterventionId} not found", ex.Message);
        }

        [Theory]
        [InlineAutoMoqData(InterventionStatus.Pending)]
        [InlineAutoMoqData(InterventionStatus.Rejected)]
        [InlineAutoMoqData(InterventionStatus.Draft)]
        public async Task SubmitIntervention_ShouldPerformNoFurtherActions_GivenInterventionStatusUnapproved(
            string status,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IServiceBusMessageSender> serviceBusMessageSender,
            [Frozen] Mock<IMediator> mediator,
            AssessmentIntervention intervention,
            AssessmentInterventionCommand command)
        {
            //Arrange
            command.Status = status;
            intervention.Status = status;
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);



            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object,
                serviceBusMessageSender.Object,
                mediator.Object
                );

            //Act
            await service.SubmitIntervention(command);

            //Assert
            assessmentRepository.Verify(x => x.GetWorkflowInstancesToDeleteForRollback(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                                        intervention.TargetAssessmentToolWorkflows!.First().AssessmentToolWorkflow.AssessmentTool.Order), Times.Never);
            assessmentRepository.Verify(x => x.GetSubsequentWorkflowInstancesForOverride(intervention
                                        .AssessmentToolWorkflowInstance.WorkflowInstanceId), Times.Never);
            assessmentRepository.Verify(x => x.GetWorkflowInstancesToDeleteForAmendment(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                                intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order), Times.Never);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Never);
            mediator.Verify(x => x.Send(It.IsAny<UpdateAssessmentWithFundIdRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task SubmitIntervention_ShouldThrowApplicationException_GivenInvalidDecisionType(
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                [Frozen] Mock<IRoleValidation> roleValidation,
                [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
                [Frozen] Mock<IAssessmentInterventionMapper> mapper,
                [Frozen] Mock<IUserProvider> userProvider,
                [Frozen] Mock<ILogger<InterventionService>> logger,
                [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
                [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
                [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
                [Frozen] Mock<IServiceBusMessageSender> serviceBusMessageSender,
                [Frozen] Mock<IMediator> mediator,
                AssessmentIntervention intervention,
                AssessmentInterventionCommand command)
        {
            //Arrange
            command.Status = InterventionStatus.Approved;
            intervention.Status = InterventionStatus.Approved;
            intervention.DecisionType = string.Empty;
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);



            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object,
                serviceBusMessageSender.Object,
                mediator.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => service.SubmitIntervention(command));

            //Assert
            Assert.Equal($"Unable to submit {command.DecisionType}. AssessmentInterventionId: {command.AssessmentInterventionId}.", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task SubmitIntervention_ShouldRequestRollbackWorkflows_GivenRollbackDecisionType(
        [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        [Frozen] Mock<IRoleValidation> roleValidation,
        [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
        [Frozen] Mock<IAssessmentInterventionMapper> mapper,
        [Frozen] Mock<IUserProvider> userProvider,
        [Frozen] Mock<ILogger<InterventionService>> logger,
        [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
        [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
        [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
        [Frozen] Mock<IServiceBusMessageSender> serviceBusMessageSender,
        [Frozen] Mock<IMediator> mediator,
        AssessmentIntervention intervention,
        List<AssessmentToolWorkflowInstance> instances,
        AssessmentInterventionCommand command)
        {
            //Arrange
            command.Status = InterventionStatus.Approved;
            intervention.Status = InterventionStatus.Approved;
            intervention.DecisionType = InterventionDecisionTypes.Rollback;
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            assessmentRepository.Setup(x => x.GetWorkflowInstancesToDeleteForRollback(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                                        intervention.TargetAssessmentToolWorkflows!.First().AssessmentToolWorkflow.AssessmentTool.Order)).ReturnsAsync(instances);



            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object,
                serviceBusMessageSender.Object,
                mediator.Object
                );

            //Act
            await service.SubmitIntervention(command);

            //Assert
            assessmentRepository.Verify(x => x.GetWorkflowInstancesToDeleteForRollback(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                                        intervention.TargetAssessmentToolWorkflows!.First().AssessmentToolWorkflow.AssessmentTool.Order), Times.Once);
            assessmentRepository.Verify(x => x.GetSubsequentWorkflowInstancesForOverride(intervention
                                        .AssessmentToolWorkflowInstance.WorkflowInstanceId), Times.Never);
            assessmentRepository.Verify(x => x.GetWorkflowInstancesToDeleteForAmendment(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                                intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order), Times.Never);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Once);
            assessmentRepository.Verify(
                x => x.DeleteAllNextWorkflows(intervention.AssessmentToolWorkflowInstance.AssessmentId), Times.Once);
            assessmentRepository.Verify(
                x => x.DeleteAllNextWorkflowsByOrder(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order), Times.Never);
            elsaServerHttpClient.Verify(
                x => x.PostArchiveQuestions(It.Is<string[]>(y =>
                    y.Intersect(instances.Select(z => z.WorkflowInstanceId).ToArray()).Count() == y.Length)),
                Times.Once);
            assessmentRepository.Verify(
                x => x.CreateAssessmentToolInstanceNextWorkflows(
                    It.Is<List<AssessmentToolInstanceNextWorkflow>>(y =>
                        y.Count == intervention.TargetAssessmentToolWorkflows.Count)), Times.Once);
            mediator.Verify(x => x.Send(It.Is<UpdateAssessmentWithFundIdRequest>(req => req.AssessmentId == command.AssessmentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task SubmitIntervention_ShouldRequestOverrideWorkflows_GivenOverrideDecisionType(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IServiceBusMessageSender> serviceBusMessageSender,
            [Frozen] Mock<IMediator> mediator,
            AssessmentIntervention intervention,
            List<AssessmentToolWorkflowInstance> instances,
            AssessmentInterventionCommand command)
        {
            //Arrange
            command.Status = InterventionStatus.Approved;
            intervention.Status = InterventionStatus.Approved;
            intervention.DecisionType = InterventionDecisionTypes.Override;
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId))
                .ReturnsAsync(intervention);
            assessmentRepository.Setup(x =>
                    x.GetSubsequentWorkflowInstancesForOverride(intervention.AssessmentToolWorkflowInstance
                        .WorkflowInstanceId))
                .ReturnsAsync(instances);



            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object,
                serviceBusMessageSender.Object,
                mediator.Object
                );

            //Act
            await service.SubmitIntervention(command);

            //Assert
            assessmentRepository.Verify(x => x.GetWorkflowInstancesToDeleteForRollback(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                                        intervention.TargetAssessmentToolWorkflows!.First().AssessmentToolWorkflow.AssessmentTool.Order), Times.Never);
            assessmentRepository.Verify(x => x.GetSubsequentWorkflowInstancesForOverride(intervention
                                        .AssessmentToolWorkflowInstance.WorkflowInstanceId), Times.Once);
            assessmentRepository.Verify(x => x.GetWorkflowInstancesToDeleteForAmendment(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                                intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order), Times.Never);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Once);
            assessmentRepository.Verify(
                x => x.DeleteAllNextWorkflows(intervention.AssessmentToolWorkflowInstance.AssessmentId), Times.Never);
            assessmentRepository.Verify(
                x => x.DeleteAllNextWorkflowsByOrder(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order), Times.Never);
            elsaServerHttpClient.Verify(
                x => x.PostArchiveQuestions(It.Is<string[]>(y =>
                    y.Intersect(instances.Select(z => z.WorkflowInstanceId).ToArray()).Count() == y.Length)),
                Times.Never);
            assessmentRepository.Verify(
                x => x.CreateAssessmentToolInstanceNextWorkflows(
                    It.Is<List<AssessmentToolInstanceNextWorkflow>>(y =>
                        y.Count == intervention.TargetAssessmentToolWorkflows.Count)), Times.Once);
            mediator.Verify(x => x.Send(It.Is<UpdateAssessmentWithFundIdRequest>(req => req.AssessmentId == command.AssessmentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task SubmitIntervention_ShouldRequestAmendmentWorkflows_GivenAmendmentDecisionType(
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                [Frozen] Mock<IRoleValidation> roleValidation,
                [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
                [Frozen] Mock<IAssessmentInterventionMapper> mapper,
                [Frozen] Mock<IUserProvider> userProvider,
                [Frozen] Mock<ILogger<InterventionService>> logger,
                [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
                [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
                [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
                [Frozen] Mock<IServiceBusMessageSender> serviceBusMessageSender,
                [Frozen] Mock<IMediator> mediator,
                AssessmentIntervention intervention,
                List<AssessmentToolWorkflowInstance> instances,
                AssessmentInterventionCommand command)
        {
            //Arrange
            command.Status = InterventionStatus.Approved;
            intervention.Status = InterventionStatus.Approved;
            intervention.DecisionType = InterventionDecisionTypes.Amendment;
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            assessmentRepository.Setup(x => x.GetWorkflowInstancesToDeleteForAmendment(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                                intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order))
                .ReturnsAsync(instances);



            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object,
                serviceBusMessageSender.Object,
                mediator.Object
                );

            //Act
            await service.SubmitIntervention(command);

            //Assert
            assessmentRepository.Verify(x => x.GetWorkflowInstancesToDeleteForRollback(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                                        intervention.TargetAssessmentToolWorkflows!.First().AssessmentToolWorkflow.AssessmentTool.Order), Times.Never);
            assessmentRepository.Verify(x => x.GetSubsequentWorkflowInstancesForOverride(intervention
                                        .AssessmentToolWorkflowInstance.WorkflowInstanceId), Times.Never);
            assessmentRepository.Verify(x => x.GetWorkflowInstancesToDeleteForAmendment(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                                intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order), Times.Once);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Once);
            assessmentRepository.Verify(
                x => x.DeleteAllNextWorkflows(intervention.AssessmentToolWorkflowInstance.AssessmentId), Times.Never);
            assessmentRepository.Verify(
                x => x.DeleteAllNextWorkflowsByOrder(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                    intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order),
                Times.Once);
            elsaServerHttpClient.Verify(
                x => x.PostArchiveQuestions(It.Is<string[]>(y =>
                    y.Intersect(instances.Select(z => z.WorkflowInstanceId).ToArray()).Count() == y.Length)),
                Times.Once);
            assessmentRepository.Verify(
                x => x.CreateAssessmentToolInstanceNextWorkflows(
                    It.Is<List<AssessmentToolInstanceNextWorkflow>>(y =>
                        y.Count == intervention.TargetAssessmentToolWorkflows.Count)), Times.Once);
            mediator.Verify(x => x.Send(It.Is<UpdateAssessmentWithFundIdRequest>(req => req.AssessmentId == command.AssessmentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task SubmitIntervention_ShouldRequestVariationWorkflows_GivenVariationDecisionType(
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
                [Frozen] Mock<IMediator> mediator,
                AssessmentIntervention intervention,
                AssessmentInterventionCommand command,
                InterventionService sut)
        {
            //Arrange
            command.Status = InterventionStatus.Approved;
            intervention.Status = InterventionStatus.Approved;
            intervention.DecisionType = InterventionDecisionTypes.Variation;
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);

            //Act
            await sut.SubmitIntervention(command);

            //Assert
            assessmentRepository.Verify(x => x.GetWorkflowInstancesToDeleteForRollback(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                                        intervention.TargetAssessmentToolWorkflows!.First().AssessmentToolWorkflow.AssessmentTool.Order), Times.Never);
            assessmentRepository.Verify(x => x.GetSubsequentWorkflowInstancesForOverride(intervention
                                        .AssessmentToolWorkflowInstance.WorkflowInstanceId), Times.Never);
            assessmentRepository.Verify(x => x.GetWorkflowInstancesToDeleteForAmendment(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                                intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order), Times.Never);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Once);
            assessmentRepository.Verify(
                x => x.DeleteAllNextWorkflows(intervention.AssessmentToolWorkflowInstance.AssessmentId), Times.Never);
            assessmentRepository.Verify(
                x => x.DeleteAllNextWorkflowsByOrder(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                    intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order),
                Times.Never);
            elsaServerHttpClient.Verify(
                x => x.PostArchiveQuestions(It.IsAny<string[]>()),
                Times.Never);
            assessmentRepository.Verify(
                x => x.CreateAssessmentToolInstanceNextWorkflows(
                    It.Is<List<AssessmentToolInstanceNextWorkflow>>(y =>
                        y.Count == intervention.TargetAssessmentToolWorkflows.Count)), Times.Once);
            mediator.Verify(x => x.Send(It.Is<UpdateAssessmentWithFundIdRequest>(req => req.AssessmentId == command.AssessmentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task SubmitIntervention_ShouldCreateNextWorkflowsWithCorrectFlags_GivenVariationDecisionType(
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                [Frozen] Mock<IMediator> mediator,
                AssessmentIntervention intervention,
                AssessmentInterventionCommand command,
                string definitionId,
                bool isLast,
                InterventionService sut)
        {
            //Arrange
            command.Status = InterventionStatus.Approved;
            intervention.Status = InterventionStatus.Approved;
            intervention.DecisionType = InterventionDecisionTypes.Variation;
            intervention.TargetAssessmentToolWorkflows = new List<TargetAssessmentToolWorkflow>
            {
                new()
                {
                    AssessmentToolWorkflow = new AssessmentToolWorkflow
                    {
                        WorkflowDefinitionId = definitionId,
                        IsLast = isLast
                    }
                }
            };
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);

            //Act
            await sut.SubmitIntervention(command);

            //Assert
            assessmentRepository.Verify(
                x => x.CreateAssessmentToolInstanceNextWorkflows(
                    It.Is<List<AssessmentToolInstanceNextWorkflow>>(y =>
                        y.Count == 1 &&
                        y[0].AssessmentId == intervention.AssessmentToolWorkflowInstance.AssessmentId &&
                        y[0].AssessmentToolWorkflowInstanceId == intervention.AssessmentToolWorkflowInstanceId &&
                        y[0].NextWorkflowDefinitionId == definitionId &&
                        y[0].IsVariation == true &&
                        y[0].IsLast == isLast
                        )), Times.Once);
            mediator.Verify(x => x.Send(It.Is<UpdateAssessmentWithFundIdRequest>(req => req.AssessmentId == command.AssessmentId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineAutoMoqData(InterventionDecisionTypes.Rollback)]
        [InlineAutoMoqData(InterventionDecisionTypes.Amendment)]
        [InlineAutoMoqData(InterventionDecisionTypes.Override)]
        public async Task SubmitIntervention_ShouldCreateNextWorkflowsWithCorrectFlags_GivenOtherDecisionTypes(
            string decisionType,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IMediator> mediator,
            AssessmentIntervention intervention,
            AssessmentInterventionCommand command,
            string definitionId,
            bool isLast,
            List<AssessmentToolWorkflowInstance> instances,
            InterventionService sut)
        {
            //Arrange
            command.Status = InterventionStatus.Approved;
            intervention.Status = InterventionStatus.Approved;
            intervention.DecisionType = decisionType;
            intervention.TargetAssessmentToolWorkflows = new List<TargetAssessmentToolWorkflow>
            {
                new()
                {
                    AssessmentToolWorkflow = new AssessmentToolWorkflow
                    {
                        WorkflowDefinitionId = definitionId,
                        IsLast = isLast,
                        AssessmentTool = new AssessmentTool
                        {
                            Order = 1
                        }
                    }
                }
            };
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            assessmentRepository.Setup(x => x.GetWorkflowInstancesToDeleteForAmendment(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                    intervention.AssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order))
                .ReturnsAsync(instances);
            assessmentRepository.Setup(x => x.GetWorkflowInstancesToDeleteForRollback(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                intervention.TargetAssessmentToolWorkflows!.First().AssessmentToolWorkflow.AssessmentTool.Order)).ReturnsAsync(instances);
            assessmentRepository.Setup(x => x.GetSubsequentWorkflowInstancesForOverride(intervention.AssessmentToolWorkflowInstance.WorkflowInstanceId))
                .ReturnsAsync(instances);

            //Act
            await sut.SubmitIntervention(command);

            //Assert
            assessmentRepository.Verify(
                x => x.CreateAssessmentToolInstanceNextWorkflows(
                    It.Is<List<AssessmentToolInstanceNextWorkflow>>(y =>
                        y.Count == 1 &&
                        y[0].AssessmentId == intervention.AssessmentToolWorkflowInstance.AssessmentId &&
                        y[0].AssessmentToolWorkflowInstanceId == intervention.AssessmentToolWorkflowInstanceId &&
                        y[0].NextWorkflowDefinitionId == definitionId &&
                        y[0].IsVariation == false &&
                        y[0].IsLast == isLast
                    )), Times.Once);
            mediator.Verify(x => x.Send(It.Is<UpdateAssessmentWithFundIdRequest>(req => req.AssessmentId == command.AssessmentId), It.IsAny<CancellationToken>()), Times.Once);
        }


        #endregion
    }
}