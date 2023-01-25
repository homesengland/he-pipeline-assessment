namespace He.PipelineAssessment.UI.Features.Admin.AssessmentTool
{
    public interface IAssessmentToolMapper
    {
        AssessmentToolData AssessmentToolsToAssessmentToolData(List<Models.AssessmentTool> assessmentTools);
    }
    public class AssessmentToolMapper : IAssessmentToolMapper
    {      
        public AssessmentToolData AssessmentToolsToAssessmentToolData(List<Models.AssessmentTool> assessmentTools)
        {
            return new AssessmentToolData
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
                        IsFirstWorkflow= y.IsFirstWorkflow,
                        IsLatest   = y.IsLatest,
                        WorkflowDefinitionId= y.WorkflowDefinitionId,   
                        Version= y.Version,

                    }).ToList(): new List<AssessmentToolWorkflowDto> { }
                }).ToList(),
            };
        }
    }
}
