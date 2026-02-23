using AutoFixture.Xunit2;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Providers;
using He.PipelineAssessment.Tests.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elsa.Server.Tests.Providers
{
    public class WorkflowInstanceProviderTests
    {
        [Theory]
        [AutoMoqData]
        public async Task
    GetWorkflowInstance_ReturnsWorkflowInstance_WhenWorkflowInstanceFound(
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStoreMock,
             string workflowInstanceId,
            CancellationToken cancellationToken,
            WorkflowInstance workflowInstance,
    WorkflowInstanceProvider sut)
        {
            // Arrange
            workflowInstanceStoreMock.Setup(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), cancellationToken)).ReturnsAsync(workflowInstance);

            //Act
            var result = await sut.GetWorkflowInstance(workflowInstanceId, cancellationToken);

            //Assert
            Assert.NotNull(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task
        GetWorkflowInstance_ThrowsException_WhenWorkflowInstanceNotFound(
        [Frozen] Mock<IWorkflowInstanceStore> store,
        string workflowInstanceId,
        CancellationToken cancellationToken,
        WorkflowInstanceProvider sut)
        {
            // Arrange
            // ✅ Use It.IsAny<> to match any specification instance
            store.Setup(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), cancellationToken))
                .ReturnsAsync((WorkflowInstance?)null);
    
            //Act
            var exception = await Assert.ThrowsAsync<Exception>(() => 
                sut.GetWorkflowInstance(workflowInstanceId, cancellationToken));

            //Assert
            Assert.Equal($"Cannot find workflow for workflowId {workflowInstanceId}.", exception.Message);
        }

    }
}
