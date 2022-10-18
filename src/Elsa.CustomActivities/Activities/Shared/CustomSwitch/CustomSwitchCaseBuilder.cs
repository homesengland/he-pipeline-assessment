using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elsa.Builders;
using Elsa.Services.Models;

namespace Elsa.CustomActivities.Activities.Shared.CustomSwitch
{
    public class CustomSwitchCaseBuilder
    {
        public ICollection<SwitchCaseDescriptor> Cases { get; } = new List<SwitchCaseDescriptor>();

        public CustomSwitchCaseBuilder Add(string name, Func<ActivityExecutionContext, ValueTask<bool>> condition, Action<IOutcomeBuilder> branch)
        {
            Cases.Add(new SwitchCaseDescriptor(name, condition, builder => branch(builder.When(name))));
            return this;
        }

        public CustomSwitchCaseBuilder Add(Func<ActivityExecutionContext, ValueTask<bool>> condition, Action<IOutcomeBuilder> branch)
        {
            var caseLabel = GetNextCaseLabel();
            Cases.Add(new SwitchCaseDescriptor(caseLabel, condition, builder => branch(builder.When(caseLabel))));
            return this;
        }

        public CustomSwitchCaseBuilder Add(string name, Func<ActivityExecutionContext, bool> condition, Action<IOutcomeBuilder> branch) => Add(name, context => new ValueTask<bool>(condition(context)), branch);
        public CustomSwitchCaseBuilder Add(string name, Func<ValueTask<bool>> condition, Action<IOutcomeBuilder> branch) => Add(name, _ => condition(), branch);
        public CustomSwitchCaseBuilder Add(string name, Func<bool> condition, Action<IOutcomeBuilder> branch) => Add(name, _ => condition(), branch);
        public CustomSwitchCaseBuilder Add(string name, bool condition, Action<IOutcomeBuilder> branch) => Add(name, _ => condition, branch);
        public CustomSwitchCaseBuilder Add(Func<ActivityExecutionContext, bool> condition, Action<IOutcomeBuilder> branch) => Add(GetNextCaseLabel(), context => new ValueTask<bool>(condition(context)), branch);
        public CustomSwitchCaseBuilder Add(Func<ValueTask<bool>> condition, Action<IOutcomeBuilder> branch) => Add(GetNextCaseLabel(), _ => condition(), branch);
        public CustomSwitchCaseBuilder Add(Func<bool> condition, Action<IOutcomeBuilder> branch) => Add(GetNextCaseLabel(), _ => condition(), branch);
        public CustomSwitchCaseBuilder Add(bool condition, Action<IOutcomeBuilder> branch) => Add(GetNextCaseLabel(), _ => condition, branch);

        private string GetNextCaseLabel() => $"Case {Cases.Count + 1}";
    }

    public record SwitchCaseDescriptor(string Name, Func<ActivityExecutionContext, ValueTask<bool>> Condition, Action<IActivityBuilder> OutcomeBuilder);
}