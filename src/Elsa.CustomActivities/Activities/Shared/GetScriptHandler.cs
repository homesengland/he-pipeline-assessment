using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;

namespace Elsa.CustomActivities.Activities.Shared
{
    public abstract class GetScriptHandler : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {
        public abstract string JavascriptElementName { get; set; }
        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            if (activityExecutionContext.Input != null && activityExecutionContext.Input.GetType() == typeof(QuestionScreenQuestion))
            {
                var engine = notification.Engine;
                engine.SetValue(JavascriptElementName, activityExecutionContext.GetInput<QuestionScreenQuestion>() ?? new QuestionScreenQuestion());
            }
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;

            output.AppendLine(string.Format("declare const {0}: AssessmentQuestion;", JavascriptElementName));

            return Task.CompletedTask;
        }
    }
}
