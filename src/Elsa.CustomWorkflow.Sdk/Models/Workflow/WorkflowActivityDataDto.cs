﻿namespace Elsa.CustomWorkflow.Sdk.Models.Workflow
{
    public class WorkflowActivityDataDto
    {
        public WorkflowActivityData Data { get; set; } = null!;
        public bool IsValid { get; set; }
        public IList<string> ValidationMessages { get; set; } = new List<string>();
    }

    public class WorkflowActivityData
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;
        public MultipleChoiceQuestionActivityData MultipleChoiceQuestionActivityData { get; set; } = null!;
        public CurrencyQuestionActivityData CurrencyQuestionActivityData { get; set; } = null!;

    }
    public class CurrencyQuestionActivityData
    {
        public string Title { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;
        public object Output { get; set; } = null!;
    }

    public class MultipleChoiceQuestionActivityData
    {
        public string Title { get; set; } = null!;
        public Choice[] Choices { get; set; } = null!;
        public string Question { get; set; } = null!;
        public object Output { get; set; } = null!;
    }

    public class Choice
    {
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
        public bool IsSelected { get; set; }
    }
}
