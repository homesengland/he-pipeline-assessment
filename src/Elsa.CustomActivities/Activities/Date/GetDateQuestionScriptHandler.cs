﻿using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Activities.Date
{
    public class GetDateQuestionScriptHandler
    {
        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            if (activityExecutionContext.Input != null && activityExecutionContext.Input.GetType() == typeof(AssessmentQuestion))
            {
                var engine = notification.Engine;
                engine.SetValue("dateQuestionResponse", activityExecutionContext.GetInput<AssessmentQuestion>() ?? new AssessmentQuestion());
            }
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;

            output.AppendLine("declare const dateQuestionResponse: AssessmentQuestion;");

            return Task.CompletedTask;
        }
    }
}
