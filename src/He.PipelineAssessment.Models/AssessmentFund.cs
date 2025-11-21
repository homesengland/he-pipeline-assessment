using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace He.PipelineAssessment.Models
{
    public class AssessmentFund : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsEarlyStage { get; set; }
        public bool IsDisabled { get; set; }
        public virtual List<AssessmentToolWorkflow>? AssessmentToolWorkflows { get; set; }
    }
}
