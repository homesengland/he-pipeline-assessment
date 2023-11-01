using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Amendment.SubmitAmendment;
using He.PipelineAssessment.UI.Services;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using He.PipelineAssessment.Infrastructure.Migrations;
using He.PipelineAssessment.UI.Common.Exceptions;
using Newtonsoft.Json;
using Xunit.Sdk;
using Azure.Core;
using Auth0.ManagementApi.Models;

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
                elsaServerHttpClient.Object
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
                elsaServerHttpClient.Object
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
                elsaServerHttpClient.Object
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

        #region CreateAssessmentIntervention

        [Theory]
        [AutoMoqData]
        public async Task CreateAssessmentIntervention_Should_ThrowErrorIfRepositoryReturnsNull(
     [Frozen] Mock<IAssessmentRepository> assessmentRepository,
     [Frozen] Mock<IRoleValidation> roleValidation,
     [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
     [Frozen] Mock<IAssessmentInterventionMapper> mapper,
     [Frozen] Mock<IUserProvider> userProvider,
     [Frozen] Mock<ILogger<InterventionService>> logger,
     [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
     [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
     [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
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
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.CreateAssessmentIntervention(command));

            //Assert
            Assert.Equal($"Assessment Tool Workflow Instance with Id {command.WorkflowInstanceId} not found", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateAssessmentIntervention_Should_ThrowErrorIfRoleNotAuthorized(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentToolWorkflowInstance workflowInstance,
            AssessmentInterventionCommand command)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            roleValidation.Setup(x => x.ValidateRole(workflowInstance.AssessmentId, workflowInstance.WorkflowInstanceId)).ReturnsAsync(false);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.CreateAssessmentIntervention(command));

            //Assert
            Assert.Equal($"You do not have permission to access this resource.", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateAssessmentIntervention_Should_ThrowErrorIRequestedIdIsNotForLatestAssessment(
    [Frozen] Mock<IAssessmentRepository> assessmentRepository,
    [Frozen] Mock<IRoleValidation> roleValidation,
    [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
    [Frozen] Mock<IAssessmentInterventionMapper> mapper,
    [Frozen] Mock<IUserProvider> userProvider,
    [Frozen] Mock<ILogger<InterventionService>> logger,
    [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
    [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
    [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
    AssessmentToolWorkflowInstance workflowInstance,
    AssessmentInterventionCommand command)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            roleValidation.Setup(x => x.ValidateRole(workflowInstance.AssessmentId, workflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);
            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsOrderEqualToLatestSubmittedWorkflowOrder(workflowInstance)).ReturnsAsync(false);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<InvalidDataException>(() => service.CreateAssessmentIntervention(command));

            //Assert
            Assert.Equal($"Unable to create {command.DecisionType} for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment. WorkflowInstanceId: {command.WorkflowInstanceId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateAssessmentIntervention_Should_ThrowErrorIfMapperThrowsArgumentException(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentToolWorkflowInstance workflowInstance,
            ArgumentException e,
            AssessmentInterventionCommand command)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            roleValidation.Setup(x => x.ValidateRole(workflowInstance.AssessmentId, workflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);
            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsOrderEqualToLatestSubmittedWorkflowOrder(workflowInstance)).ReturnsAsync(true);
            mapper.Setup(x => x.AssessmentInterventionFromAssessmentInterventionCommand(command)).Throws(e);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAssessmentIntervention(command));

            //Assert
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateAssessmentIntervention_Should_CatchesAndThrowsError_GivenAnyErrorThrownWhilstWritingToDatavase(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentToolWorkflowInstance workflowInstance,
            Exception e,
            AssessmentInterventionCommand command,
            AssessmentIntervention intervention)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            roleValidation.Setup(x => x.ValidateRole(workflowInstance.AssessmentId, workflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);
            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsOrderEqualToLatestSubmittedWorkflowOrder(workflowInstance)).ReturnsAsync(true);
            mapper.Setup(x => x.AssessmentInterventionFromAssessmentInterventionCommand(command)).Returns(intervention);
            assessmentRepository.Setup(x => x.CreateAssessmentIntervention(intervention)).ThrowsAsync(e);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => service.CreateAssessmentIntervention(command));

            //Assert
            Assert.Equal($"Unable to create {command.DecisionType}. WorkflowInstanceId: {command.WorkflowInstanceId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateAssessmentIntervention_Should_ReturnAssessmentInterventionId_GivenNoErrorsThrown(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentToolWorkflowInstance workflowInstance,
            AssessmentInterventionCommand command,
            AssessmentIntervention intervention)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            roleValidation.Setup(x => x.ValidateRole(workflowInstance.AssessmentId, workflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);
            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsOrderEqualToLatestSubmittedWorkflowOrder(workflowInstance)).ReturnsAsync(true);
            mapper.Setup(x => x.AssessmentInterventionFromAssessmentInterventionCommand(command)).Returns(intervention);
            assessmentRepository.Setup(x => x.CreateAssessmentIntervention(intervention)).ReturnsAsync(intervention.Id);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            int savedId = await service.CreateAssessmentIntervention(command);

            //Assert
            Assert.Equal(savedId, intervention.Id);
            assessmentRepository.Verify(
                x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId),
                Times.Once);
            assessmentRepository.Verify(
                x => x.CreateAssessmentIntervention(intervention),
                Times.Once);

        }

        #endregion

        #region CreateInterventionRequest

        [Theory]
        [AutoMoqData]
        public async Task CreateInterventionRequest_Should_ThrowErrorIfRepositoryReturnsNull(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            CreateInterventionRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync((AssessmentToolWorkflowInstance?)null);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.CreateInterventionRequest(request));

            //Assert
            Assert.Equal($"Assessment Tool Workflow Instance with Id {request.WorkflowInstanceId} not found", ex.Message);
        }


        public async Task CreateInterventionRequest_Should_ThrowErrorIfRoleNotAuthorised(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentToolWorkflowInstance workflowToolInstance,
            CreateInterventionRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowToolInstance);
            roleValidation.Setup(x => x.ValidateRole(workflowToolInstance.AssessmentId, workflowToolInstance.WorkflowInstanceId)).ReturnsAsync(false);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
            elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.CreateInterventionRequest(request));

            //Assert
            Assert.Equal($"You do not have permission to access this resource.", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateInterventionRequest_Should_ThrowErrorIRequestedIdIsNotForLatestAssessment(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentToolWorkflowInstance workflowInstance,
            CreateInterventionRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            roleValidation.Setup(x => x.ValidateRole(workflowInstance.AssessmentId, workflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);
            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsOrderEqualToLatestSubmittedWorkflowOrder(workflowInstance)).ReturnsAsync(false);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => service.CreateInterventionRequest(request));

            //Assert
            Assert.Equal($"Unable to create {request.DecisionType} for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment. WorkflowInstanceId: {request.WorkflowInstanceId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateInterventionRequest_Should_ThrowErrorGivenAnyOpenAssessmentInterventionsFound(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentToolWorkflowInstance workflowInstance,
            List<AssessmentIntervention> interventions,
            CreateInterventionRequest request)
        {
            //Arrange
            if (!interventions.Any())
            {
                interventions.Add(new AssessmentIntervention());
            }
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            roleValidation.Setup(x => x.ValidateRole(workflowInstance.AssessmentId, workflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);
            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsOrderEqualToLatestSubmittedWorkflowOrder(workflowInstance)).ReturnsAsync(true);
            assessmentRepository.Setup(x => x.GetOpenAssessmentInterventions(workflowInstance.AssessmentId)).ReturnsAsync(interventions);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => service.CreateInterventionRequest(request));

            //Assert
            Assert.Equal($"Unable to create request as an open request already exists for this assessment.", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateInterventionRequest_Should_ThrowErrorGivenMapperThrowsException(
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                [Frozen] Mock<IRoleValidation> roleValidation,
                [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
                [Frozen] Mock<IAssessmentInterventionMapper> mapper,
                [Frozen] Mock<IUserProvider> userProvider,
                [Frozen] Mock<ILogger<InterventionService>> logger,
                [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
                [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
                [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
                AssessmentToolWorkflowInstance workflowInstance,
                string userName,
                string email,
                List<AssessmentIntervention> interventions,
                List<InterventionReason> reasons,
                ApplicationException exception,
                CreateInterventionRequest request)
        {
            //Arrange
            interventions = new List<AssessmentIntervention>();
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            roleValidation.Setup(x => x.ValidateRole(workflowInstance.AssessmentId, workflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);
            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsOrderEqualToLatestSubmittedWorkflowOrder(workflowInstance)).ReturnsAsync(true);
            assessmentRepository.Setup(x => x.GetOpenAssessmentInterventions(workflowInstance.AssessmentId)).ReturnsAsync(interventions);
            userProvider.Setup(x => x.GetUserEmail()).Returns(email);
            userProvider.Setup(x => x.GetUserName()).Returns(userName);
            assessmentRepository.Setup(x => x.GetInterventionReasons(false)).ReturnsAsync(reasons);
            mapper.Setup(x => x.AssessmentInterventionDtoFromWorkflowInstance(workflowInstance, reasons, It.IsAny<DtoConfig>())).Throws(exception);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => service.CreateInterventionRequest(request));

            //Assert
            Assert.Equal(exception.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateInterventionRequest_Should_ReturnDTOGivenNoExceptionsThrown(
        [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        [Frozen] Mock<IRoleValidation> roleValidation,
        [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
        [Frozen] Mock<IAssessmentInterventionMapper> mapper,
        [Frozen] Mock<IUserProvider> userProvider,
        [Frozen] Mock<ILogger<InterventionService>> logger,
        [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
        [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
        [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
        AssessmentToolWorkflowInstance workflowInstance,
        string userName,
        string email,
        List<AssessmentIntervention> interventions,
        AssessmentInterventionDto dto,
        List<InterventionReason> reasons,
        CreateInterventionRequest request)
        {
            //Arrange
            interventions = new List<AssessmentIntervention>();
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            roleValidation.Setup(x => x.ValidateRole(workflowInstance.AssessmentId, workflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);
            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsOrderEqualToLatestSubmittedWorkflowOrder(workflowInstance)).ReturnsAsync(true);
            assessmentRepository.Setup(x => x.GetOpenAssessmentInterventions(workflowInstance.AssessmentId)).ReturnsAsync(interventions);
            userProvider.Setup(x => x.GetUserEmail()).Returns(email);
            userProvider.Setup(x => x.GetUserName()).Returns(userName);
            assessmentRepository.Setup(x => x.GetInterventionReasons(false)).ReturnsAsync(reasons);
            mapper.Setup(x => x.AssessmentInterventionDtoFromWorkflowInstance(workflowInstance, reasons, It.IsAny<DtoConfig>())).Returns(dto);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            string expectedErrorMessage = $"Unable to map AssessmentToolWorkflowInstance:  {JsonConvert.SerializeObject(workflowInstance)} to AssessmentInterventionDto";

            //Act
            AssessmentInterventionDto dtoFromMapper = await service.CreateInterventionRequest(request);

            //Assert
            Assert.Equal(dto, dtoFromMapper);
        }


        #endregion

        #region DeleteIntervention


        [Theory]
        [AutoMoqData]
        public async Task DeleteIntervention_Should_ThrowErrorIfRepositoryReturnsNull(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
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
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteIntervention(command));

            //Assert
            Assert.Equal($"Assessment Intervention with Id {command.AssessmentInterventionId} not found", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteIntervention_Should_ThrowErrorIfUserNotAuthorized(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentIntervention intervention,
            AssessmentInterventionCommand command)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            roleValidation.Setup(x => x.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId))
                .ReturnsAsync(false);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.DeleteIntervention(command));

            //Assert
            Assert.Equal($"You do not have permission to access this resource.", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteIntervention_Should_CallDeleteRepositoryMethodOnce_GivenNoErrors(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentIntervention intervention,
            AssessmentInterventionCommand command)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            roleValidation.Setup(x => x.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId))
                .ReturnsAsync(true);
            assessmentRepository.Setup(x => x.DeleteIntervention(intervention)).ReturnsAsync(intervention.Id);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var result = await service.DeleteIntervention(command);

            assessmentRepository.Verify(x => x.DeleteIntervention(intervention), Times.Once);
            Assert.Equal(intervention.Id, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteIntervention_ShouldCatchAndThrowAnyException_GivenRepositoryThrowsExceptionWhenDeleting(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentIntervention intervention,
            AssessmentInterventionCommand command,
            Exception e)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            roleValidation.Setup(x => x.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId))
                .ReturnsAsync(true);
            assessmentRepository.Setup(x => x.DeleteIntervention(intervention)).ThrowsAsync(e);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => service.DeleteIntervention(command));

            Assert.Equal($"Unable to delete {command.DecisionType}. WorkflowInstanceId: {command.WorkflowInstanceId}", ex.Message);
        }



        #endregion

        #region EditIntervention


        [Theory]
        [AutoMoqData]
        public async Task EditIntervention_Should_ThrowErrorIfRepositoryReturnsNull(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
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
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.EditIntervention(command));

            //Assert
            Assert.Equal($"Assessment Intervention with Id {command.AssessmentInterventionId} not found", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditIntervention_Should_ThrowErrorIfUserNotAuthorized(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentIntervention intervention,
            AssessmentInterventionCommand command)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            roleValidation.Setup(x => x.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId))
                .ReturnsAsync(false);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.EditIntervention(command));

            //Assert
            Assert.Equal($"You do not have permission to access this resource.", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditIntervention_Should_SaveChangesInRepository_GivenNoExceptionsThrown(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentIntervention intervention,
            AssessmentInterventionCommand command)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            roleValidation.Setup(x => x.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId))
                .ReturnsAsync(true);

            AssessmentIntervention interventionToSave = intervention;
            interventionToSave.AdministratorRationale = command.AdministratorRationale;
            interventionToSave.SignOffDocument = command.SignOffDocument;
            interventionToSave.TargetAssessmentToolWorkflowId = command.TargetWorkflowId;
            interventionToSave.Administrator = command.Administrator;
            interventionToSave.AdministratorEmail = command.AdministratorEmail;

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var result = await service.EditIntervention(command);

            //Assert
            Assert.Equal(interventionToSave, intervention);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Once);
            Assert.Equal(intervention.Id, result);
        }

        public async Task EditIntervention_Should_CatchAndThrowException_GivenRepositoryThrowsException(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentIntervention intervention,
            Exception e,
            AssessmentInterventionCommand command)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            roleValidation.Setup(x => x.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId))
                .ReturnsAsync(true);
            assessmentRepository.Setup(x => x.SaveChanges()).ThrowsAsync(e);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => service.EditIntervention(command));

            //Assert
            Assert.Equal($"Unable to edit {command.DecisionType}. AssessmentInterventionId: {command.AssessmentInterventionId}", ex.Message);
        }


        #endregion

        #region EditInterventionAssessor

        [Theory]
        [AutoMoqData]
        public async Task EditInterventionAssessor_Should_ThrowErrorIfRepositoryReturnsNull(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
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
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.EditInterventionAssessor(command));

            //Assert
            Assert.Equal($"Assessment Intervention with Id {command.AssessmentInterventionId} not found", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditInterventionAssessor_Should_CatchAnyException_GivenExceptionThrownWhenSaving(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentIntervention intervention,
            Exception e,
            AssessmentInterventionCommand command)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            assessmentRepository.Setup(x => x.SaveChanges()).ThrowsAsync(e);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => service.EditInterventionAssessor(command));

            //Assert
            Assert.Equal($"Unable to edit {command.DecisionType}. AssessmentInterventionId: {command.AssessmentInterventionId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditInterventionAssessor_Should_SaveChangesInRepository_GivenNoExceptionsThrown(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentIntervention intervention,
            AssessmentInterventionCommand command)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            assessmentRepository.Setup(x => x.SaveChanges()).ReturnsAsync(intervention.Id);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var result = await service.EditInterventionAssessor(command);

            //Assert
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Once);
            Assert.Equal(intervention.Id, result);
        }


        #endregion

        #region EditInterventionAssessorRequest

        [Theory]
        [AutoMoqData]
        public async Task EditInterventionAssessorRequest_ShouldThrowError_IfRepositoryReturnsNull(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            EditInterventionAssessorRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync((AssessmentIntervention?)null);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.EditInterventionAssessorRequest(request));

            //Assert
            Assert.Equal($"Assessment Intervention with Id {request.InterventionId} not found", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditInterventionAssessorRequest_ShouldCatchAndThrowError_IfMapperThrowsAnyError(
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                [Frozen] Mock<IRoleValidation> roleValidation,
                [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
                [Frozen] Mock<IAssessmentInterventionMapper> mapper,
                [Frozen] Mock<IUserProvider> userProvider,
                [Frozen] Mock<ILogger<InterventionService>> logger,
                [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
                [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
                [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
                AssessmentIntervention intervention,
                Exception e,
                EditInterventionAssessorRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Throws(e);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => service.EditInterventionAssessorRequest(request));

            //Assert
            Assert.Equal($"Unable to edit intervention. InterventionId: {request.InterventionId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditInterventionAssessorRequest_ShouldCatchAndThrowException_GivenRepositoryThrowsException(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentIntervention intervention,
            AssessmentInterventionCommand command,
            Exception e,
            EditInterventionAssessorRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Returns(command);
            assessmentRepository.Setup(x => x.GetInterventionReasons(false)).ThrowsAsync(e);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => service.EditInterventionAssessorRequest(request));

            //Assert
            Assert.Equal($"Unable to edit intervention. InterventionId: {request.InterventionId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditInterventionAssessorRequest_ShouldReturnDto_GivenNoExceptionsThrown(
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                [Frozen] Mock<IRoleValidation> roleValidation,
                [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
                [Frozen] Mock<IAssessmentInterventionMapper> mapper,
                [Frozen] Mock<IUserProvider> userProvider,
                [Frozen] Mock<ILogger<InterventionService>> logger,
                [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
                [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
                [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
                AssessmentIntervention intervention,
                AssessmentInterventionCommand command,
                List<InterventionReason> reasons,
                EditInterventionAssessorRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Returns(command);
            assessmentRepository.Setup(x => x.GetInterventionReasons(false)).ReturnsAsync(reasons);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            var expectedResult = new AssessmentInterventionDto
            {
                InterventionReasons = reasons,
                AssessmentInterventionCommand = command
            };

            //Act
            var result = await service.EditInterventionAssessorRequest(request);

            //Assert

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        #endregion

        #region EditInterventionRequest

        [Theory]
        [AutoMoqData]
        public async Task EditInterventionRequest_ShouldThrowException_IfRepositoryReturnsNull(
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                [Frozen] Mock<IRoleValidation> roleValidation,
                [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
                [Frozen] Mock<IAssessmentInterventionMapper> mapper,
                [Frozen] Mock<IUserProvider> userProvider,
                [Frozen] Mock<ILogger<InterventionService>> logger,
                [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
                [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
                [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
                EditInterventionRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync((AssessmentIntervention?)null);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.EditInterventionRequest(request));

            //Assert
            Assert.Equal($"Assessment Intervention with Id {request.InterventionId} not found", ex.Message);
        }


        [Theory]
        [AutoMoqData]
        public async Task EditInterventionRequest_ShouldThrowException_RoleValidationReturnsFalse(
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                [Frozen] Mock<IRoleValidation> roleValidation,
                [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
                [Frozen] Mock<IAssessmentInterventionMapper> mapper,
                [Frozen] Mock<IUserProvider> userProvider,
                [Frozen] Mock<ILogger<InterventionService>> logger,
                [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
                [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
                [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
                AssessmentIntervention intervention,
                EditInterventionRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            roleValidation.Setup(x => x.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(false);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.EditInterventionRequest(request));

            //Assert
            Assert.Equal($"You do not have permission to access this resource.", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditInterventionRequest_ShouldCatchAndThrowException_GivenMapperThrowsException(
        [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        [Frozen] Mock<IRoleValidation> roleValidation,
        [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
        [Frozen] Mock<IAssessmentInterventionMapper> mapper,
        [Frozen] Mock<IUserProvider> userProvider,
        [Frozen] Mock<ILogger<InterventionService>> logger,
        [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
        [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
        [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
        AssessmentIntervention intervention,
        Exception e,
        EditInterventionRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            roleValidation.Setup(x => x.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId,
    intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Throws(e);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => service.EditInterventionRequest(request));

            //Assert
            Assert.Equal($"Unable to edit intervention. InterventionId: {request.InterventionId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task EditInterventionRequest_ShouldReturmDto_GivenNoExceptionsThrown(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentIntervention intervention,
            AssessmentInterventionCommand command,
            EditInterventionRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            roleValidation.Setup(x => x.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId,
    intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Returns(command);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            var expectedDto = new AssessmentInterventionDto()
            {
                AssessmentInterventionCommand = command
            };

            //Act
            var result = await service.EditInterventionRequest(request);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedDto), JsonConvert.SerializeObject(result));
        }

        #endregion

        #region LoadInterventionCheckYourAnswerAssessorRequest

        [Theory]
        [AutoMoqData]
        public async Task LoadInterventionCheckYourAnswersAssessorRequest_ShouldThrowException_IfRepositoryReturnsNull(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            LoadInterventionCheckYourAnswersAssessorRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync((AssessmentIntervention?)null);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.LoadInterventionCheckYourAnswerAssessorRequest(request));

            //Assert
            Assert.Equal($"Assessment Intervention with Id {request.InterventionId} not found", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadInterventionCheckYourAnswersAssessorRequest_ShouldCatchAndThrowException_IfMapperThrowsException(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentIntervention intervention,
            Exception e,
            LoadInterventionCheckYourAnswersAssessorRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Throws(e);

            InterventionService service = new InterventionService(
               assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => service.LoadInterventionCheckYourAnswerAssessorRequest(request));

            //Assert
            Assert.Equal($"Unable to load check your answers. InterventionId: {request.InterventionId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadInterventionCheckYourAnswersAssessorRequest_ShouldReturnAssessmentInterventionCommand_GivenNoExceptionsThrown(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentIntervention intervention,
            AssessmentInterventionCommand command,
            LoadInterventionCheckYourAnswersAssessorRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Returns(command);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var result = await service.LoadInterventionCheckYourAnswerAssessorRequest(request);

            //Assert
            Assert.Equal(command, result);
        }

        #endregion

        #region LoadInterventionCheckYourAnswersRequest

        [Theory]
        [AutoMoqData]
        public async Task LoadInterventionCheckYourAnswersRequest_ShouldThrowException_IfRepositoryReturnsNull(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            LoadInterventionCheckYourAnswersRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync((AssessmentIntervention?)null);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.LoadInterventionCheckYourAnswersRequest(request));

            //Assert
            Assert.Equal($"Assessment Intervention with Id {request.InterventionId} not found", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadInterventionCheckYourAnswersRequest_ShouldCatchAndThrowException_IfMapperThrowsException(
    [Frozen] Mock<IAssessmentRepository> assessmentRepository,
    [Frozen] Mock<IRoleValidation> roleValidation,
    [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
    [Frozen] Mock<IAssessmentInterventionMapper> mapper,
    [Frozen] Mock<IUserProvider> userProvider,
    [Frozen] Mock<ILogger<InterventionService>> logger,
    [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
    [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
    [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
    AssessmentIntervention intervention,
    Exception e,
    LoadInterventionCheckYourAnswersRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Throws(e);

            InterventionService service = new InterventionService(
               assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => service.LoadInterventionCheckYourAnswersRequest(request));

            //Assert
            Assert.Equal($"Unable to load intervention. InterventionId: {request.InterventionId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadInterventionCheckYourAnswersRequest_ShouldReturnAssessmentInterventionCommand_GivenNoExceptionsThrown(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentIntervention intervention,
            AssessmentInterventionCommand command,
            LoadInterventionCheckYourAnswersRequest request)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Returns(command);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var result = await service.LoadInterventionCheckYourAnswersRequest(request);

            //Assert
            Assert.Equal(command, result);
        }



        #endregion

        #region GetAssessmentToolWorkflowsForOverride

        [Theory]
        [AutoMoqData]
        public async Task GetAssessmentToolWorkflowsForOverride_ShouldThrowException_IfRepositoryReturnsNull(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            string workflowInstanceId)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowInstanceId)).ReturnsAsync((AssessmentToolWorkflowInstance?)null);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetAssessmentToolWorkflowsForOverride(workflowInstanceId));

            //Assert
            Assert.Equal($"Assessment Tool Workflow Instance with Id {workflowInstanceId} not found", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAssessmentToolWorkflowsForOverride_ShouldThrowException_GivenEmptyListOfAssessmentsReturnedFromRepository(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentToolWorkflowInstance instance,
            string workflowInstanceId)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowInstanceId)).ReturnsAsync(instance);
            List<AssessmentToolWorkflow> assessmentToolWorkflows = new List<AssessmentToolWorkflow>();
            adminAssessmentToolWorkflowRepository.Setup(x => x.GetAssessmentToolWorkflowsForOverride(instance.AssessmentToolWorkflow.AssessmentTool.Order))
                .ReturnsAsync(assessmentToolWorkflows);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetAssessmentToolWorkflowsForOverride(workflowInstanceId));

            //Assert
            Assert.Equal($"No suitable assessment tool workflows found for override", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAssessmentToolWorkflowsForOverride_ShouldReturnListofAssessmentToolWorkflow_GivenValidRequestData(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentToolWorkflowInstance instance,
            List<AssessmentToolWorkflow> workflows,
            string workflowInstanceId)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowInstanceId)).ReturnsAsync(instance);
            adminAssessmentToolWorkflowRepository.Setup(x => x.GetAssessmentToolWorkflowsForOverride(instance.AssessmentToolWorkflow.AssessmentTool.Order))
                .ReturnsAsync(workflows);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var results = await service.GetAssessmentToolWorkflowsForOverride(workflowInstanceId);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(workflows), JsonConvert.SerializeObject(results));
        }

        #endregion

        #region GetAssessmentToolWorkflowsForRollback

        [Theory]
        [AutoMoqData]
        public async Task GetAssessmentToolWorkflowsForRollback_ShouldThrowException_IfRepositoryReturnsNull(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            string workflowInstanceId)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowInstanceId)).ReturnsAsync((AssessmentToolWorkflowInstance?)null);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetAssessmentToolWorkflowsForRollback(workflowInstanceId));

            //Assert
            Assert.Equal($"Assessment Tool Workflow Instance with Id {workflowInstanceId} not found", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAssessmentToolWorkflowsForRollback_ShouldThrowException_GivenEmptyListOfAssessmentsReturnedFromRepository(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentToolWorkflowInstance instance,
            string workflowInstanceId)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowInstanceId)).ReturnsAsync(instance);
            List<AssessmentToolWorkflow> assessmentToolWorkflows = new List<AssessmentToolWorkflow>();
            adminAssessmentToolWorkflowRepository.Setup(x => x.GetAssessmentToolWorkflowsForRollback(instance.AssessmentToolWorkflow.AssessmentTool.Order))
                .ReturnsAsync(assessmentToolWorkflows);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetAssessmentToolWorkflowsForRollback(workflowInstanceId));

            //Assert
            Assert.Equal($"No suitable assessment tool workflows found for rollback", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAssessmentToolWorkflowsForRollback_ShouldReturnListofAssessmentToolWorkflow_GivenValidRequestData(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            AssessmentToolWorkflowInstance instance,
            List<AssessmentToolWorkflow> workflows,
            string workflowInstanceId)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowInstanceId)).ReturnsAsync(instance);
            adminAssessmentToolWorkflowRepository.Setup(x => x.GetAssessmentToolWorkflowsForRollback(instance.AssessmentToolWorkflow.AssessmentTool.Order))
                .ReturnsAsync(workflows);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var results = await service.GetAssessmentToolWorkflowsForRollback(workflowInstanceId);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(workflows), JsonConvert.SerializeObject(results));
        }

        #endregion

        #region GetAssessmentToolWorkflowsForVariation

        [Theory]
        [AutoMoqData]
        public async Task GetAssessmentToolWorkflowsForVariation_ShouldThrowException_GivenEmptyListOfAssessmentsReturnedFromRepository(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            string workflowInstanceId)
        {
            //Arrange
            List<AssessmentToolWorkflow> assessmentToolWorkflows = new List<AssessmentToolWorkflow>();
            adminAssessmentToolWorkflowRepository.Setup(x => x.GetAssessmentToolWorkflowsForVariation())
                .ReturnsAsync(assessmentToolWorkflows);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.GetAssessmentToolWorkflowsForVariation(workflowInstanceId));

            //Assert
            Assert.Equal($"No suitable assessment tool workflows found for variation", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetAssessmentToolWorkflowsForVariation_ShouldReturnListofAssessmentToolWorkflow_GivenValidRequestData(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<ILogger<InterventionService>> logger,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            List<AssessmentToolWorkflow> workflows,
            string workflowInstanceId)
        {
            //Arrange
            adminAssessmentToolWorkflowRepository.Setup(x => x.GetAssessmentToolWorkflowsForVariation())
                .ReturnsAsync(workflows);

            InterventionService service = new InterventionService(
                assessmentRepository.Object,
                roleValidation.Object,
                assessmentToolWorkflowInstanceHelpers.Object,
                mapper.Object,
                userProvider.Object,
                logger.Object,
                adminAssessmentToolWorkflowRepository.Object,
                dateTimeProvider.Object,
                elsaServerHttpClient.Object
                );

            //Act
            var results = await service.GetAssessmentToolWorkflowsForVariation(workflowInstanceId);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(workflows), JsonConvert.SerializeObject(results));
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
                elsaServerHttpClient.Object
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
                elsaServerHttpClient.Object
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
                elsaServerHttpClient.Object
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
                elsaServerHttpClient.Object
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
            AssessmentIntervention intervention,
            List<AssessmentToolWorkflowInstance> instances,
            AssessmentInterventionCommand command)
        {
            //Arrange
            command.Status = InterventionStatus.Approved;
            intervention.Status = InterventionStatus.Approved;
            intervention.DecisionType = InterventionDecisionTypes.Override;
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            assessmentRepository.Setup(x => x.GetSubsequentWorkflowInstancesForOverride(intervention.AssessmentToolWorkflowInstance.WorkflowInstanceId))
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
                elsaServerHttpClient.Object
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
        }

        [Theory]
        [AutoMoqData]
        public async Task SubmitIntervention_ShouldRequestAmmendmentWorkflows_GivenAmmendmentDecisionType(
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                [Frozen] Mock<IRoleValidation> roleValidation,
                [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
                [Frozen] Mock<IAssessmentInterventionMapper> mapper,
                [Frozen] Mock<IUserProvider> userProvider,
                [Frozen] Mock<ILogger<InterventionService>> logger,
                [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
                [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
                [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
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
                elsaServerHttpClient.Object
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
        }

        #endregion
    }
}
