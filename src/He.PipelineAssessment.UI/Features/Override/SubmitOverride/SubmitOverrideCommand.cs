﻿using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Override.SubmitOverride
{
    public class SubmitOverrideCommand : AssessmentInterventionCommand, IRequest
    {
        public override string FinalInstanceStatus => AssessmentToolWorkflowInstanceConstants.SuspendOverrides;
    }
}
