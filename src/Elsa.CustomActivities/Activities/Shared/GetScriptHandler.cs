using Elsa.Activities.Workflows;
using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Activities.Shared
{
    public class GetScriptHandler : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {
        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            if (activityExecutionContext.Input != null && activityExecutionContext.Input.GetType() == typeof(AssessmentQuestion))
            {
                var engine = notification.Engine;
                var input = activityExecutionContext.GetInput<AssessmentQuestion>();

                if (input != null)
                {
                    engine.SetValue("question", input);
                }
            }

            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;

            output.AppendLine(string.Format("declare const {0}: AssessmentQuestion;", "question"));

            return Task.CompletedTask;
        }
    }
}
