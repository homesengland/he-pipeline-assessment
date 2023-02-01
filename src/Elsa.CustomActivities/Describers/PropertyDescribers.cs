
using System.Reflection;
using Elsa.Attributes;
using Elsa.Metadata;
using Humanizer;
using Elsa.CustomActivities.Activities;
using Elsa.CustomActivities.Resolver;

namespace Elsa.CustomActivities.Describers
{
    public static class CustomPropertyDescribers
    {

        public static IEnumerable<ActivityInputDescriptor> DescribeInputProperties(Type propertyType)
        {
            var properties = propertyType.GetProperties();
            var inputProperties = DescribeInputProperties(properties).OrderBy(x => x.Order);
            return inputProperties;
        }

        private static IEnumerable<ActivityInputDescriptor> DescribeInputProperties(IEnumerable<PropertyInfo> properties, string category = null)
        {
            foreach (var propertyInfo in properties)
            {
                var activityPropertyObjectAttribute = propertyInfo.GetCustomAttribute<CustomElsaAttributeObject>();

                if (activityPropertyObjectAttribute != null)
                {

                    var objectProperties = propertyInfo.PropertyType.GetProperties();

                    var nestedInputProperties = DescribeInputProperties(objectProperties, activityPropertyObjectAttribute?.Category!);

                    foreach (var property in nestedInputProperties)
                    {
                        property.Name = $"{propertyInfo.Name}_{property.Name}";
                        yield return property;
                    }
                    continue;
                }

                var activityPropertyAttribute = propertyInfo.GetCustomAttribute<ActivityInputAttribute>();

                if (activityPropertyAttribute == null)
                    continue;

                yield return new ActivityInputDescriptor
                (
                    activityPropertyAttribute.Name ?? propertyInfo.Name,
                    propertyInfo.PropertyType,
                    CustomUIHintResolver.GetUIHint(propertyInfo),
                    activityPropertyAttribute.Label ?? propertyInfo.Name.Humanize(LetterCasing.Title),
                    activityPropertyAttribute.Hint,
                    null,
                    //_optionsResolver.GetOptions(propertyInfo),
                    activityPropertyAttribute.Category ?? category,
                    activityPropertyAttribute.Order,
                    activityPropertyAttribute.DefaultValue,
                    activityPropertyAttribute.DefaultSyntax,
                    activityPropertyAttribute.SupportedSyntaxes,
                    activityPropertyAttribute.IsReadOnly,
                    activityPropertyAttribute.IsBrowsable,
                    activityPropertyAttribute.IsDesignerCritical,
                    activityPropertyAttribute.DefaultWorkflowStorageProvider,
                    activityPropertyAttribute.DisableWorkflowProviderSelection,
                    activityPropertyAttribute.ConsiderValuesAsOutcomes
                );
            }
        }

        //public async Task<ActivityDescriptor> DescribeAsync(Type activityType, CancellationToken cancellationToken = default)
        //{
        //    var activityAttribute = activityType.GetCustomAttribute<ActivityAttribute>(false);
        //    var typeName = activityAttribute?.Type ?? activityType.Name;
        //    var displayName = activityAttribute?.DisplayName ?? activityType.Name.Humanize(LetterCasing.Title);
        //    var description = activityAttribute?.Description;
        //    var category = activityAttribute?.Category ?? "Miscellaneous";
        //    var traits = activityAttribute?.Traits ?? ActivityTraits.Action;
        //    //var outcomes = await GetOutcomesAsync(activityAttribute, cancellationToken);
        //    var outcomes = Array.Empty<string>();
        //    var properties = activityType.GetProperties();
        //    var inputProperties = DescribeInputProperties(properties).OrderBy(x => x.Order);
        //    var outputProperties = DescribeOutputProperties(properties);

        //    return new ActivityDescriptor
        //    {
        //        Type = typeName.Pascalize(),
        //        DisplayName = displayName,
        //        Description = description,
        //        Category = category,
        //        Traits = traits,
        //        InputProperties = inputProperties.ToArray(),
        //        OutputProperties = outputProperties.ToArray(),
        //        Outcomes = outcomes
        //    };
        //}



        //private IEnumerable<ActivityOutputDescriptor> DescribeOutputProperties(IEnumerable<PropertyInfo> properties)
        //{
        //    foreach (var propertyInfo in properties)
        //    {
        //        var activityPropertyAttribute = propertyInfo.GetCustomAttribute<ActivityOutputAttribute>();

        //        if (activityPropertyAttribute == null)
        //            continue;

        //        yield return new ActivityOutputDescriptor
        //        (
        //            (activityPropertyAttribute.Name ?? propertyInfo.Name).Pascalize(),
        //            propertyInfo.PropertyType,
        //            activityPropertyAttribute.Hint,
        //            activityPropertyAttribute.DefaultWorkflowStorageProvider,
        //            activityPropertyAttribute.DisableWorkflowProviderSelection
        //        );
        //    }
        //}


    }
}
