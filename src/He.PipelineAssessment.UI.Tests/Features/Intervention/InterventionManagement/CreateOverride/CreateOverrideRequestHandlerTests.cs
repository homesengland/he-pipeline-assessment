﻿using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using Moq;
using Xunit;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.Infrastructure;
using Auth0.ManagementApi.Models;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Override.CreateOverride;
using He.PipelineAssessment.UI.Features.Intervention;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsException_GivenRepoReturnsNoWorkflowInstance(
                  [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                  CreateOverrideRequest request,
                  AssessmentToolWorkflowInstance? workflowInstance,
                  CreateOverrideRequestHandler sut
              )
        {
            //Arrange
            workflowInstance = null;

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(($"Assessment Tool Workflow Instance with Id {request.WorkflowInstanceId} not found"), ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task HandleThrowsException_GivenIsNotLatestSubmittedWorkflow(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminRepository,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowHelper,
            AssessmentToolWorkflowInstance workflowInstance,
            CreateOverrideRequest request,
            CreateOverrideRequestHandler sut
        )
        {
            //Arrange
            List<AssessmentToolWorkflow> emptyListOfWorkflow = new List<AssessmentToolWorkflow>();

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);

            assessmentToolWorkflowHelper
                .Setup(x => x.IsLatestSubmittedWorkflow(workflowInstance)).Returns(false);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            adminRepository.Setup(x => x.GetAssessmentToolWorkflowsForOverride(workflowInstance.AssessmentToolWorkflow.AssessmentTool.Order)).ReturnsAsync(emptyListOfWorkflow);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(($"Unable to create override for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment."), ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task HandleThrowsException_GivenAdminRepoReturnsNoAssessmentToolWorkflows(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminRepository,
          [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowHelper,
          AssessmentToolWorkflowInstance workflowInstance,
          CreateOverrideRequest request,
          CreateOverrideRequestHandler sut
      )
        {
            //Arrange
            List<AssessmentToolWorkflow> emptyListOfWorkflow = new List<AssessmentToolWorkflow>();

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);

            assessmentToolWorkflowHelper
                .Setup(x => x.IsLatestSubmittedWorkflow(workflowInstance)).Returns(true);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            adminRepository.Setup(x => x.GetAssessmentToolWorkflowsForOverride(workflowInstance.AssessmentToolWorkflow.AssessmentTool.Order)).ReturnsAsync(emptyListOfWorkflow);

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(($"No suitable assessment tool workflows found for override"), ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_DoesNotHandleError_GivenMapperThrowsError(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminRepository,
          [Frozen] Mock<IAssessmentInterventionMapper> mapper,
          [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowHelper,
          AssessmentToolWorkflowInstance workflowInstance,
          string userName,
          string email,
          CreateOverrideRequest request,
          List<AssessmentToolWorkflow> workflows,
          ArgumentException exception,
          CreateOverrideRequestHandler sut
)
        {
            //Arrange
            var dtoConfig = new DtoConfig()
            {
                AdministratorName = userName,
                UserName = userName,
                AdministratorEmail = email,
                UserEmail = email,
                DecisionType = InterventionDecisionTypes.Override,
                Status = InterventionStatus.Pending
            };

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);

            assessmentToolWorkflowHelper
                .Setup(x => x.IsLatestSubmittedWorkflow(workflowInstance)).Returns(true);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            adminRepository.Setup(x => x.GetAssessmentToolWorkflowsForOverride(workflowInstance
                .AssessmentToolWorkflow.AssessmentTool.Order)).ReturnsAsync(workflows);
            mapper.Setup(x => x.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(workflows)).Throws(exception);
            mapper.Setup(x => x.AssessmentInterventionDtoFromWorkflowInstance(workflowInstance, dtoConfig)).Throws(exception);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(exception.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsValidResult_GivenNoErrors(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminRepository,
          [Frozen] Mock<IAssessmentInterventionMapper> mapper,
          [Frozen] Mock<IUserProvider> userProvider,
          [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowHelper,
          AssessmentToolWorkflowInstance workflowInstance,
          CreateOverrideRequest request,
          List<AssessmentToolWorkflow> workflows,
          List<TargetWorkflowDefinition> targetDefinitions,
          AssessmentInterventionDto dto,
          CreateOverrideRequestHandler sut)
        {
            //Arrange
            dto.TargetWorkflowDefinitions = targetDefinitions;
            var dtoConfig = new DtoConfig()
            {
                AdministratorName = dto.AssessmentInterventionCommand.RequestedBy,
                UserName = dto.AssessmentInterventionCommand.RequestedBy,
                AdministratorEmail = dto.AssessmentInterventionCommand.RequestedByEmail,
                UserEmail = dto.AssessmentInterventionCommand.RequestedByEmail,
                DecisionType = InterventionDecisionTypes.Override,
                Status = InterventionStatus.Pending
            };

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);

            assessmentToolWorkflowHelper
                .Setup(x => x.IsLatestSubmittedWorkflow(workflowInstance)).Returns(true);

            userProvider.Setup(x => x.GetUserName()).Returns(dto.AssessmentInterventionCommand.RequestedBy);
            userProvider.Setup(x => x.GetUserEmail()).Returns(dto.AssessmentInterventionCommand.RequestedByEmail);
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId)).ReturnsAsync(workflowInstance);
            adminRepository.Setup(x => x.GetAssessmentToolWorkflowsForOverride(workflowInstance.AssessmentToolWorkflow.AssessmentTool.Order)).ReturnsAsync(workflows);
            mapper.Setup(x => x.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(workflows)).Returns(targetDefinitions);
            mapper.Setup(x => x.AssessmentInterventionDtoFromWorkflowInstance(workflowInstance, dtoConfig)).Returns(dto);
            //Act
            AssessmentInterventionDto result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Equal(dto, result);
        }
    }
}
