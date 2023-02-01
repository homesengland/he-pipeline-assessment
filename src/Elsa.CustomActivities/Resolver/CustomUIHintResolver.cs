using Elsa.Attributes;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.Design;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Resolver
{
    public static class CustomUIHintResolver
    {
        public static string GetUIHint(PropertyInfo activityPropertyInfo)
        {
            var activityPropertyAttribute = activityPropertyInfo.GetCustomAttribute<ActivityInputAttribute>();

            if (activityPropertyAttribute != null && activityPropertyAttribute.UIHint != null)
                return activityPropertyAttribute.UIHint;

            var type = activityPropertyInfo.PropertyType;

            if (type == typeof(bool) || type == typeof(bool?))
                return ActivityInputUIHints.Checkbox;

            if (type == typeof(string))
                return ActivityInputUIHints.SingleLine;

            if (typeof(IEnumerable).IsAssignableFrom(type))
                return ActivityInputUIHints.Dropdown;

            if (type.IsEnum && type.GetTypeOfNullable().IsEnum)
                return ActivityInputUIHints.Dropdown;

            return ActivityInputUIHints.SingleLine;
        }
    }
}
