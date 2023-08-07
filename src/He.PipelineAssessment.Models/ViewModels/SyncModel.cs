using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Models.ViewModels
{
    public class SyncModel
    {
        public int UpdatedAssessmentCount { get; set; }
        public int NewAssessmentCount { get; set;}
        public bool Synced { get; set; } = false;
    }
}
