using AutoFixture.Xunit2;
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
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Rollback.CreateRollback;
using He.PipelineAssessment.UI.Services;
using System;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_DoesNotHandleException_GivenInterventionServiceThrowsException(
                  [Frozen] Mock<IInterventionService> interventionService,
                  CreateOverrideRequest request,
                  Exception exception,
                  CreateOverrideRequestHandler sut
              )
        {
            //Arrange
            interventionService.Setup(x => x.CreateInterventionRequest(request)).ThrowsAsync(exception);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(exception.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_DoesNotHandleError_GivenMapperThrowsError(
              [Frozen] Mock<IInterventionService> interventionService,
              [Frozen] Mock<IAssessmentInterventionMapper> mapper,
              CreateOverrideRequest request,
              List<AssessmentToolWorkflow> workflows,
              ArgumentException exception,
              AssessmentInterventionDto dto,
              CreateOverrideRequestHandler sut
)
        {
            //Arrange
            interventionService.Setup(x => x.CreateInterventionRequest(request)).ReturnsAsync(dto);
            interventionService.Setup(x => x.GetAssessmentToolWorkflowsForOverride(request.WorkflowInstanceId)).ReturnsAsync(workflows);

            mapper.Setup(x => x.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(workflows, dto.AssessmentInterventionCommand.SelectedWorkflowDefinitions)).Throws(exception);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(exception.Message, ex.Message);
        }


        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsValidResult_GivenNoErrors(
          [Frozen] Mock<IInterventionService> interventionService,
          [Frozen] Mock<IAssessmentInterventionMapper> mapper,
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
            interventionService.Setup(x => x.CreateInterventionRequest(request)).ReturnsAsync(dto);
            interventionService.Setup(x => x.GetAssessmentToolWorkflowsForOverride(request.WorkflowInstanceId)).ReturnsAsync(workflows);

            mapper.Setup(x => x.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(workflows, dto.AssessmentInterventionCommand.SelectedWorkflowDefinitions)).Returns(targetDefinitions);
            mapper.Setup(x => x.AssessmentInterventionDtoFromWorkflowInstance(workflowInstance, dtoConfig)).Returns(dto);
            //Act
            AssessmentInterventionDto result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Equal(dto, result);
        }
    }
}
