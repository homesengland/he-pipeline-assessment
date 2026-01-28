using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Models
{
    public class SensitiveRecordWhitelist: AuditableEntity
    {
        public int Id { get; set; }
        public int AssessmentId { get; set; }
        public string Email { get; set; } = string.Empty;

    }
}
