using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Elsa.Server.Features.Workflow.ExecuteWorkflow;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.ExecuteWorkflow
{
    public class ExecuteWorkflowCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnExecuteWorkflowResponse(
            [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
            WorkflowNextActivityModel workflowNextActivityModel,
            ExecuteWorkflowCommand command,
            ExecuteWorkflowCommandHandler sut)
        {
            //Arrange
            workflowNextActivityProvider.Setup(x => x.GetNextActivity(command.ActivityId, command.WorkflowInstanceId, null,
                command.ActivityType, CancellationToken.None))
                .ReturnsAsync(workflowNextActivityModel);
            
            //Act
            var result =  await sut.Handle(command,CancellationToken.None);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(command.WorkflowInstanceId, result.Data!.WorkflowInstanceId);
            Assert.Equal(workflowNextActivityModel.NextActivity.Type, result.Data!.ActivityType);
            Assert.Equal(workflowNextActivityModel.NextActivity.Id, result.Data!.NextActivityId);
        }
    }
}
