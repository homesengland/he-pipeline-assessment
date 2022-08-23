using Elsa.Server.Features.Workflow.StartWorkflow;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.StartWorkflow
{
    public class StartWorkflowMapperTests
    {
        [Theory]
        [InlineAutoMoqData(1)]
        [InlineAutoMoqData(2)]
        [InlineAutoMoqData(3)]
        public void Test1(int myInt, StartWorkflowCommand startWorkflowCommand)
        {
            startWorkflowCommand.WorkflowDefinitionId = myInt.ToString();
        }
    }
}
