using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Infrastructure
{
    public class RoleConstants
    {
        public static class AppRole
        {
            public const string PipelineObserver = "PipelineAssessment.Observer";

            public const string SensitiveRecordsViewer = "PipelineAssessment.SensitiveRecordsViewer";

            public const string PipelineProjectManager = "PipelineAssessment.ProjectManager";

            public const string PipelineAdminOperations = "PipelineAssessment.AdminOperations";

            public const string PipelineEconomist = "PipelineAssessment.Economist";

        }
    }
}
