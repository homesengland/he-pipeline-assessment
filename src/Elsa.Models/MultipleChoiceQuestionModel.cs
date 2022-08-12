namespace Elsa.Models
{
    public class MultipleChoiceQuestionModel
    {
        public string QuestionID { get; set; }

        public string WorkflowInstanceID { get; set; }

        public string Answer { get; set; }

        public bool FinishWorkflow { get; set; }
        public bool NavigateBack { get; set; }
        public string PreviousActivityId { get; set; }
    }
}
