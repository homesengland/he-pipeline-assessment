using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;

namespace Elsa.CustomActivities.Activities.MultipleChoice
{
    public class GetMultipleChoiceQuestionScriptHandler : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {
        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            if (activityExecutionContext.Input != null && activityExecutionContext.Input.GetType() == typeof(AssessmentQuestion))
            {
                var engine = notification.Engine;
                engine.SetValue("multipleChoiceQuestionResponse", activityExecutionContext.GetInput<AssessmentQuestion>() ?? new AssessmentQuestion());
            }
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;

            output.AppendLine("declare const multipleChoiceQuestionResponse: AssessmentQuestion;");

            return Task.CompletedTask;
        }
    }
}
