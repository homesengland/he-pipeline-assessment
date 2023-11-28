using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow
{
    public class ReturnToActivityDataDto
    {
        public ReturnToActivityData Data { get; set; } = null!;
        public bool IsValid { get; set; }
        public ValidationResult? ValidationMessages { get; set; }
    }
    public class ReturnToActivityData
    {
        public string WorkflowInstanceId { get; set; }
        public string ActivityId { get; set; }
        public string ActivityType { get; set; }
    }
}
