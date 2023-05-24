using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.UI.Features.Workflow.ViewModels;
using TableInput = He.PipelineAssessment.UI.Features.Workflow.ViewModels.TableInput;

namespace He.PipelineAssessment.UI.Extensions
{
    public static class DtoMapperExtensions
    {
        public static List<DataTable> ToDataTableList(this WorkflowActivityData @this, int index, List<string?> usedDisplayGroupIds)
        {
            List<DataTable> tables = new List<DataTable>();
            if (@this.Questions != null && @this.Questions.Count() > 0 && @this.Questions.Any(x => x.QuestionType == QuestionTypeConstants.DataTable))
            {
                tables = @this.Questions.Where(q => q.QuestionType == QuestionTypeConstants.DataTable
               && !usedDisplayGroupIds.Contains(q.DataTable.DisplayGroupId)
               //&& q.DataTable.DisplayGroupId !=null
               && q.DataTable.DisplayGroupId == @this.Questions[index].DataTable.DisplayGroupId)
               .Select(x => new DataTable
               {
                   QuestionText = @this.Questions.FirstOrDefault(l => l.QuestionId == x.QuestionId)?.Question,
                   QuestionIndex = @this.Questions.FindIndex(l => l.QuestionId == x.QuestionId),
                   DisplayGroupId = x.DataTable.DisplayGroupId,
                   InputType = x.DataTable.InputType,
                   Inputs = x.DataTable.Inputs.Select(y =>
                           new TableInput
                           {
                               Identifier = y.Identifier,
                               InputHeading = y.Title,
                               Input = y.Input,
                               IsReadOnly = y.IsReadOnly,
                               IsSummaryTotal = y.IsSummaryTotal
                               
                           }).ToList()
               }).ToList();
            }
            return tables;
        }
    }
}

