using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Models.ViewModels
{
    public abstract class AuditableEntityViewModel
    {
            public DateTime CreatedDate { get; set; }

            public string CreatedBy { get; set; }

            public DateTime? LastModifiedDate { get; set; }

            public string? LastModifiedBy { get; set; }
    }
}
