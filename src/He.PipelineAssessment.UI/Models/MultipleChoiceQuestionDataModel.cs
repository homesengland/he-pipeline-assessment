using System.Dynamic;

namespace He.PipelineAssessment.UI.Models
{

    public class MultipleChoiceQuestionDataModel
    {
        public Workflowinstance workflowInstance { get; set; }
        public string activityId { get; set; }
        public object exception { get; set; }
        public bool executed { get; set; }
    }

    public class Workflowinstance
    {
        public string definitionId { get; set; }
        public string definitionVersionId { get; set; }
        public object tenantId { get; set; }
        public int version { get; set; }
        public string workflowStatus { get; set; }
        public string correlationId { get; set; }
        public object contextType { get; set; }
        public object contextId { get; set; }
        public object name { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime lastExecutedAt { get; set; }
        public object finishedAt { get; set; }
        public object cancelledAt { get; set; }
        public object faultedAt { get; set; }
        public Variables variables { get; set; }
        public object input { get; set; }
        public Output output { get; set; }
        public ExpandoObject activityData { get; set; }
        public Metadata metadata { get; set; }
        public Blockingactivity[] blockingActivities { get; set; }
        public object fault { get; set; }
        public object[] scheduledActivities { get; set; }
        public object[] scopes { get; set; }
        public object currentActivity { get; set; }
        public string lastExecutedActivityId { get; set; }
        public string id { get; set; }
    }

    public class Variables
    {
        public Data data { get; set; }
    }

    public class Data
    {
    }

    public class Output
    {
        public object providerName { get; set; }
        public string activityId { get; set; }
    }

    public class Activitydata
    {
        public string Title { get; set; }
        public Choice[] Choices { get; set; }
        public string QuestionID { get; set; }
        public string Question { get; set; }
        public Case[] Cases { get; set; }
        public object Mode { get; set; }
        public object Output { get; set; }
        public _Lifecycle _Lifecycle { get; set; }
    }

    public class _Lifecycle
    {
        public bool executing { get; set; }
        public bool executed { get; set; }
    }

    public class Choice
    {
        public string answer { get; set; }
        public bool isSingle { get; set; }
        public bool isSelected { get; set; }
    }

    public class Case
    {
        public string name { get; set; }
        public bool condition { get; set; }
    }

    public class Metadata
    {
    }

    public class Blockingactivity
    {
        public string activityId { get; set; }
        public string activityType { get; set; }
        public object tag { get; set; }
    }

}
