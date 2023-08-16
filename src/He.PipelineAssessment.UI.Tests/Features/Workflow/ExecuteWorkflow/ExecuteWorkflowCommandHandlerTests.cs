using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Azure.Core;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Headings;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Workflow.ExecuteWorkflow;
using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.ExecuteWorkflow
{
    public class ExecuteWorkflowCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnUnauthorizedException_GivenUserDoesNotHavePermission(
            [Frozen]Mock<IRoleValidation> roleValidation,
            [Frozen]Mock<IAssessmentRepository> repository,
            ExecuteWorkflowCommand command,
            AssessmentToolWorkflowInstance instance,
            ExecuteWorkflowCommandHandler sut
        )
        {
            //Arrange
            repository.Setup(x=>x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync(instance);

            roleValidation.Setup(x => x.ValidateRole(instance.AssessmentId, instance.WorkflowDefinitionId))
                .ReturnsAsync(false);

            //Act
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal("You do not have permission to access this resource.",ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldThrowException_GivenHttpClientResponseIsNull(
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentRepository> repository,
            ExecuteWorkflowCommand command,
            AssessmentToolWorkflowInstance instance,
            ExecuteWorkflowCommandHandler sut
        )
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync(instance);

            roleValidation.Setup(x => x.ValidateRole(instance.AssessmentId, instance.WorkflowDefinitionId))
                .ReturnsAsync(true);

            //Act
            var exceptionThrown =  await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal($"Cannot Execute Workflow. ActivityId: {command.ActivityId} WorkflowInstanceId:{command.WorkflowInstanceId}", exceptionThrown.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturn(
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentRepository> repository,
            [Frozen] Mock<IElsaServerHttpClient> httpClient,
            ExecuteWorkflowCommand command,
            AssessmentToolWorkflowInstance instance,
            WorkflowNextActivityDataDto nextActivityDataDto,
            ExecuteWorkflowCommandHandler sut
        )
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync(instance);

            roleValidation.Setup(x => x.ValidateRole(instance.AssessmentId, instance.WorkflowDefinitionId))
                .ReturnsAsync(true);

            httpClient.Setup(x => x.PostExecuteWorkflow(It.IsAny<ExecuteWorkflowCommandDto>())).ReturnsAsync(nextActivityDataDto);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<LoadQuestionScreenRequest>(result);
        }
    }
}
