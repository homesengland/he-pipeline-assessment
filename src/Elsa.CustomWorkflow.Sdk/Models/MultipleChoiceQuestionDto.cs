namespace Elsa.CustomWorkflow.Sdk.Models
{
    public class MultipleChoiceQuestionDto
    {
        public string Id { get; set; }
        public string ActivityID { get; set; }

        public string WorkflowInstanceID { get; set; }

        public string Answer { get; set; }

        public bool FinishWorkflow { get; set; }
        public bool NavigateBack { get; set; }
        public string PreviousActivityId { get; set; }
    }
}
