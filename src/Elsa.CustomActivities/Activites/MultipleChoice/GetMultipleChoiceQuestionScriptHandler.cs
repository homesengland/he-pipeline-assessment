using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;

namespace Elsa.CustomActivities.Activites.MultipleChoice
{
    public class GetMultipleChoiceQuestionScriptHandler : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {
        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            if (activityExecutionContext.Input != null && activityExecutionContext.Input.GetType() == typeof(MultipleChoiceQuestionModel))
            {
                var engine = notification.Engine;
                engine.SetValue("multipleChoiceQuestionResponse", activityExecutionContext.GetInput<MultipleChoiceQuestionModel>() ?? new MultipleChoiceQuestionModel());
            }
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;

            output.AppendLine("declare const multipleChoiceQuestionResponse: MultipleChoiceQuestionModel;");

            return Task.CompletedTask;
        }
    }
}
