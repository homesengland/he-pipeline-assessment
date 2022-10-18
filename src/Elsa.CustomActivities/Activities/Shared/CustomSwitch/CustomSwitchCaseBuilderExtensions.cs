using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Elsa.Activities.ControlFlow;
using Elsa.Builders;

// ReSharper disable ExplicitCallerInfoArgument
// ReSharper disable once CheckNamespace
namespace Elsa.CustomActivities.Activities.Shared.CustomSwitch
{
    public static class CustomSwitchBuilderExtensions
    {
        public static IActivityBuilder Switch(
            this IBuilder builder,
            Func<CustomSwitchCaseBuilder, ValueTask> cases,
            SwitchMode mode = SwitchMode.MatchFirst,
            [CallerLineNumber] int lineNumber = default,
            [CallerFilePath] string? sourceFile = default)
        {
            var switchCaseBuilder = new CustomSwitchCaseBuilder();

            var activityBuilder = builder.Then<CustomQuestion>(async setup =>
            {
                await cases(switchCaseBuilder);

                setup
                    .WithCases(async context =>
                    {
                        var evaluatedCases = switchCaseBuilder.Cases.Select(async x => new SwitchCase(x.Name, await x.Condition(context))).ToList();
                        return await Task.WhenAll(evaluatedCases);
                    })
                    .WithMode(mode);
            }, null, lineNumber, sourceFile);

            foreach (var caseDescriptor in switchCaseBuilder.Cases)
                caseDescriptor.OutcomeBuilder(activityBuilder);

            return activityBuilder;
        }

        public static IActivityBuilder Switch(
            this IBuilder builder,
            Action<CustomSwitchCaseBuilder> cases,
            SwitchMode mode = SwitchMode.MatchFirst,
            [CallerLineNumber] int lineNumber = default,
            [CallerFilePath] string? sourceFile = default)
        {
            return builder.Switch(caseBuilder =>
            {
                cases(caseBuilder);
                return new ValueTask();
            }, mode, lineNumber, sourceFile);
        }
    }
}