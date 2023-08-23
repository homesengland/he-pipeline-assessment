using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using FluentValidation.Results;

namespace He.PipelineAssessment.UI.Features.Workflow.ViewModels
{
    public class QuestionScreenQuestion
    {
        public QuestionActivityData QuestionActivityData { get; set; }
        public int Index { get; set; }
        public bool IsValid { get; set; }
        public ValidationResult? ValidationMessages { get; set; }
    }
}
