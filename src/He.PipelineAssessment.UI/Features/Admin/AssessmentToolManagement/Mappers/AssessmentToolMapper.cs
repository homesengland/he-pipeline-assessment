namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers
{
    public interface IAssessmentToolMapper
    {
        AssessmentToolListData AssessmentToolsToAssessmentToolData(List<AssessmentTool> assessmentTools);
        AssessmentTool CreateAssessmentToolCommandToAssessmentTool(CreateAssessmentToolCommand createAssessmentToolCommand);

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
                        Name = y.Name,
                        AssessmentToolId = y.AssessmentToolId,
                        IsFirstWorkflow = y.IsFirstWorkflow,
                        IsLatest = y.IsLatest,
                        WorkflowDefinitionId = y.WorkflowDefinitionId,
                        Version = y.Version,

                    }).ToList() : new List<AssessmentToolWorkflowDto> { }
                }).ToList(),
            };
        }

        public AssessmentTool CreateAssessmentToolCommandToAssessmentTool(CreateAssessmentToolCommand createAssessmentToolCommand)
        {
            return new AssessmentTool
            {
                Name = createAssessmentToolCommand.Name,
                Order = createAssessmentToolCommand.Order,
                IsVisible = true
            };
        }

        public List<Queries.GetAssessmentToolWorkflows.AssessmentToolWorkflowDto> AssessmentToolWorkflowsToAssessmentToolDto(List<AssessmentToolWorkflow> assessmentToolWorkflows)
        {
            return assessmentToolWorkflows.Select(x => new Queries.GetAssessmentToolWorkflows.AssessmentToolWorkflowDto
            {
                Id = x.Id,
                Name = x.Name,
                AssessmentToolId = x.AssessmentToolId,
                IsFirstWorkflow = x.IsFirstWorkflow,
                IsLatest = x.IsLatest,
                Version = x.Version,
                WorkflowDefinitionId = x.WorkflowDefinitionId
            }).ToList();
        }
    }


}
