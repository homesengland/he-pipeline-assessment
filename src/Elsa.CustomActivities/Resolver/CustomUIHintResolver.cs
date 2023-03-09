using Elsa.Attributes;
using Elsa.CustomActivities.Constants;
using System.Collections;
using System.Reflection;

namespace Elsa.CustomActivities.Resolver
{
    public static class CustomUIHintResolver
    {
        public static string GetUIHint<T>(PropertyInfo activityPropertyInfo) where T : ActivityInputAttribute
        {
            var activityPropertyAttribute = activityPropertyInfo.GetCustomAttribute<T>();

            if (activityPropertyAttribute != null && activityPropertyAttribute.UIHint != null)
                return activityPropertyAttribute.UIHint;

            var type = activityPropertyInfo.PropertyType;

            if (type == typeof(bool) || type == typeof(bool?))
                return HePropertyUIHints.Checkbox;

            if (type == typeof(string))
                return HePropertyUIHints.SingleLine;

            if (typeof(IEnumerable).IsAssignableFrom(type))
                return HePropertyUIHints.Dropdown;

            if (type.IsEnum && type.GetTypeOfNullable().IsEnum)
                return HePropertyUIHints.Dropdown;

            return HePropertyUIHints.SingleLine;
        }
    }
}
