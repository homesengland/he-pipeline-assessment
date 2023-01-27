using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentTools;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement
{
    public interface IAssessmentToolMapper
    {
        AssessmentToolListData AssessmentToolsToAssessmentToolData(List<Models.AssessmentTool> assessmentTools);
        AssessmentTool CreateAssessmentToolCommandToAssessmentTool(CreateAssessmentToolCommand assessmentToolDto);

        List<Queries.GetAssessmentToolWorkflows.AssessmentToolWorkflowDto> AssessmentToolWorkflowsToAssessmentToolDto(List<AssessmentToolWorkflow> toList);
    }
    public class AssessmentToolMapper : IAssessmentToolMapper
    {
        public AssessmentToolListData AssessmentToolsToAssessmentToolData(List<AssessmentTool> assessmentTools)
        {
            return new AssessmentToolListData
            {
                AssessmentTools = assessmentTools.Select(x => new AssessmentToolDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Order = x.Order,
                    AssessmentToolWorkFlows = x.AssessmentToolWorkflows != null ? x.AssessmentToolWorkflows.Select(y => new AssessmentToolWorkflowDto
                    {
                        Id = y.Id,
                        AssessmentToolId = y.AssessmentToolId,
                        IsFirstWorkflow = y.IsFirstWorkflow,
                        IsLatest = y.IsLatest,
                        WorkflowDefinitionId = y.WorkflowDefinitionId,
                        Version = y.Version,

                    }).ToList() : new List<AssessmentToolWorkflowDto> { }
                }).ToList(),
            };
        }

        public AssessmentTool CreateAssessmentToolCommandToAssessmentTool(CreateAssessmentToolCommand assessmentToolDto)
        {
            return new AssessmentTool
            {
                Name = assessmentToolDto.Name,
                Order = assessmentToolDto.Order,
                IsVisible = true
            };
        }

        public List<Queries.GetAssessmentToolWorkflows.AssessmentToolWorkflowDto> AssessmentToolWorkflowsToAssessmentToolDto(List<AssessmentToolWorkflow> assessmentToolWorkflows)
        {
            return assessmentToolWorkflows.Select(x => new Queries.GetAssessmentToolWorkflows.AssessmentToolWorkflowDto
            {
                Id = x.Id,
                AssessmentToolId = x.AssessmentToolId,
                AssessmentToolName = x.AssessmentTool.Name,
                IsFirstWorkflow = x.IsFirstWorkflow,
                IsLatest = x.IsLatest,
                Version = x.Version,
                WorkflowDefinitionId = x.WorkflowDefinitionId
            }).ToList();
        }
    }


}
