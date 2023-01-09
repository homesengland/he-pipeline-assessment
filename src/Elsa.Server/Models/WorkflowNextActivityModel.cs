using Elsa.Models;
using Elsa.Services.Models;

namespace Elsa.Server.Models
{
    public class WorkflowNextActivityModel
    {
        public IActivityBlueprint NextActivity { get; set; } = null!;
        public WorkflowInstance? WorkflowInstance { get; set; }
    }
}
