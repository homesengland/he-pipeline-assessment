﻿namespace Elsa.CustomWorkflow.Sdk.Models
{
    public class MultipleChoiceQuestionDto
    {
        public string Id { get; set; } = null!;
        public string ActivityId { get; set; } = null!;

        public string WorkflowInstanceId { get; set; } = null!;

        public string Answer { get; set; } = null!;

        public bool? FinishWorkflow { get; set; }
        public bool? NavigateBack { get; set; }
        public string PreviousActivityId { get; set; } = null!;
    }
}
