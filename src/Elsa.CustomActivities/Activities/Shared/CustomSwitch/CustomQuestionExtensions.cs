using Elsa.Activities.ControlFlow;
using Elsa.Builders;
using Elsa.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Activities.Shared.CustomSwitch
{
        public static class CustomQuestionExtensions
        {
            public static ISetupActivity<CustomQuestion> WithCases(this ISetupActivity<CustomQuestion> activity, Func<ActivityExecutionContext, ValueTask<ICollection<SwitchCase>>> value) => activity.Set(x => x.Cases, value!);
            public static ISetupActivity<CustomQuestion> WithCases(this ISetupActivity<CustomQuestion> activity, Func<ActivityExecutionContext, ICollection<SwitchCase>> value) => activity.Set(x => x.Cases, value!);
            public static ISetupActivity<CustomQuestion> WithCases(this ISetupActivity<CustomQuestion> activity, Func<ICollection<SwitchCase>> value) => activity.Set(x => x.Cases, value!);
            public static ISetupActivity<CustomQuestion> WithCases(this ISetupActivity<CustomQuestion> activity, ICollection<SwitchCase> value) => activity.Set(x => x.Cases, value!);
            public static ISetupActivity<CustomQuestion> WithMode(this ISetupActivity<CustomQuestion> activity, SwitchMode value) => activity.Set(x => x.Mode, value);
        }
    }
