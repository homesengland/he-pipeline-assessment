using AutoFixture.Xunit2;
using Azure.Core;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Workflow.ReturnToActivity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.ReturnToActivity
{
    public class ReturnToActivityCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsApplicationException_GivenHttpClientResponseIsNull(
        [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
        [Frozen] Mock<IRoleValidation> roleValidation,
        [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        ReturnToActivityCommand command,
        ReturnToActivityCommandHandler sut,
        AssessmentToolWorkflowInstance assessmentToolWorkflowInstance)
        {
            //Arrange
            assessmentRepository.Setup(x=> x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync(assessmentToolWorkflowInstance);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId, assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);

            elsaServerHttpClient.Setup(x => x.ReturnToActivity(It.IsAny<ReturnToActivityData>()))
                .ReturnsAsync((ReturnToActivityDataDto?)null);

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal($"Failed to return to activity. ActivityId: {command.ActivityId} WorkflowInstanceId: {command.WorkflowInstanceId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnUnauthorizedException_GivenUserDoesNotHavePermission(
        [Frozen] Mock<IRoleValidation> roleValidation,
        [Frozen] Mock<IAssessmentRepository> repository,
        ReturnToActivityCommand command,
        ReturnToActivityCommandHandler sut,
        AssessmentToolWorkflowInstance assessmentToolWorkflowInstance
        )
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync(assessmentToolWorkflowInstance);

            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId, assessmentToolWorkflowInstance.WorkflowDefinitionId))
                .ReturnsAsync(false);
                
            //Act
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal("You do not have permission to access this resource.", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsCorrectResponse_GivenHttpClientResponseIsSuccessful(
        [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
        [Frozen] Mock<IRoleValidation> roleValidation,
        [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        ReturnToActivityCommand command,
        ReturnToActivityCommandHandler sut,
        AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
        ReturnToActivityDataDto response)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync(assessmentToolWorkflowInstance);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId, assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);

            elsaServerHttpClient.Setup(x => x.ReturnToActivity(It.IsAny<ReturnToActivityData>()))
                .ReturnsAsync((ReturnToActivityDataDto?)response);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(result!.WorkflowInstanceId, command.WorkflowInstanceId);
            Assert.Equal(result!.ActivityId, response.Data.ActivityId);
            Assert.Equal(result!.ActivityType, response.Data.ActivityType);
        }
    }
}
