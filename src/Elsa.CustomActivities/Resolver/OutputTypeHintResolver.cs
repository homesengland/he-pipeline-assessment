using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.PropertyDecorator;
using System.Reflection;

namespace Elsa.CustomActivities.Resolver
{
    public static class OutputTypeHintResolver
    {
        public static string GetOutputTypeHint<T>(PropertyInfo activityPropertyInfo) where T : HeActivityInputAttribute
        {
            var activityPropertyAttribute = activityPropertyInfo.GetCustomAttribute<T>();

            if (activityPropertyAttribute != null && activityPropertyAttribute.ExpectedOutputType != null)
                return activityPropertyAttribute.ExpectedOutputType;

            Type type = activityPropertyInfo.PropertyType;

            if (type == typeof(bool) || type == typeof(bool?))
                return ExpectedOutputHints.Boolean;

            if (type == typeof(string))
                return ExpectedOutputHints.Text;

            if (type == typeof(int))
                return ExpectedOutputHints.Number;

            return ExpectedOutputHints.Text;
        }

        public static Type GetTypeFromTypeHint(string typeHint)
        {
            switch (typeHint)
            {
                case ExpectedOutputHints.Boolean:
                    return typeof(bool);
                case ExpectedOutputHints.Number:
                    return typeof(int);
                default:
                    return typeof(string);
            }
        }
    }
}