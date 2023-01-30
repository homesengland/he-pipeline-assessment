﻿using Azure.Core;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflowCommand;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers
{
    public interface IAssessmentToolWorkflowMapper
    {
        AssessmentToolWorkflow CreateAssessmentToolWorkflowCommandToAssessmentToolWorkflow(CreateAssessmentToolWorkflowCommand assessmentToolWorkflowCommand);
    }
    public class AssessmentToolWorkflowMapper : IAssessmentToolWorkflowMapper
    {
        public AssessmentToolWorkflow CreateAssessmentToolWorkflowCommandToAssessmentToolWorkflow(CreateAssessmentToolWorkflowCommand assessmentToolWorkflowCommand)
        {
            return new AssessmentToolWorkflow
            {

                AssessmentToolId = assessmentToolWorkflowCommand.AssessmentToolId,
                IsFirstWorkflow = assessmentToolWorkflowCommand.IsFirstWorkflow,
                Version = assessmentToolWorkflowCommand.Version,
                IsLatest = assessmentToolWorkflowCommand.IsLatest,
            };
        }
    }
}
