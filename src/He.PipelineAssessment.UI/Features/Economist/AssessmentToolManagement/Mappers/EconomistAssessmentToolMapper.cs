using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Economist.AssessmentToolManagement.Queries.GetAssessmentTools;

namespace He.PipelineAssessment.UI.Features.Economist.AssessmentToolManagement.Mappers
{
    public interface IEconomistAssessmentToolMapper
    {
        AssessmentToolListData AssessmentToolsToAssessmentToolData(List<AssessmentTool> assessmentTools);

    }
    public class EconomistAssessmentToolMapper : IEconomistAssessmentToolMapper
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
                        IsEconomistWorkflow = y.IsEconomistWorkflow,
                        IsLatest = y.IsLatest,
                        WorkflowDefinitionId = y.WorkflowDefinitionId,
                        Version = y.Version,

                    }).ToList() : new List<AssessmentToolWorkflowDto> { }
                }).ToList(),
            };
        }
      
    }


}
