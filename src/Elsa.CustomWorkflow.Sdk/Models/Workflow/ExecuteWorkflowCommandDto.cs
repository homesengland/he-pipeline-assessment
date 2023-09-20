using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow
{
    public class ExecuteWorkflowCommandDto
    {
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string WorkflowInstanceId { get; set; } = null!;

        public string WorkflowName { get; set; } = null!;
    }
}
