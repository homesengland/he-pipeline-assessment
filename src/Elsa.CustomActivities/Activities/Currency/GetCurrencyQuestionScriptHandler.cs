﻿using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;

namespace Elsa.CustomActivities.Activities.Currency
{
    public class GetCurrencyQuestionScriptHandler
    {
        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            if (activityExecutionContext.Input != null && activityExecutionContext.Input.GetType() == typeof(MultipleChoiceQuestionModel))
            {
                var engine = notification.Engine;
                engine.SetValue("currencyQuestionResponse", activityExecutionContext.GetInput<MultipleChoiceQuestionModel>() ?? new MultipleChoiceQuestionModel());
            }
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;

            output.AppendLine("declare const currencyQuestionResponse: MultipleChoiceQuestionModel;");

            return Task.CompletedTask;
        }
    }
}
