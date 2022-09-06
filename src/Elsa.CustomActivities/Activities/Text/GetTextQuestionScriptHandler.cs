using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;

namespace Elsa.CustomActivities.Activities.Text
{
    public class GetTextQuestionScriptHandler
    {
        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            if (activityExecutionContext.Input != null && activityExecutionContext.Input.GetType() == typeof(AssessmentQuestion))
            {
                var engine = notification.Engine;
                engine.SetValue("textQuestionResponse", activityExecutionContext.GetInput<AssessmentQuestion>() ?? new AssessmentQuestion());
            }
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;

            output.AppendLine("declare const textQuestionResponse: AssessmentQuestion;");

            return Task.CompletedTask;
        }
    }
}
