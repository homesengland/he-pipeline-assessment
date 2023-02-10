
using System.Reflection;
using Elsa.Attributes;
using Elsa.Metadata;
using Humanizer;
using Elsa.CustomActivities.Activities;
using Elsa.CustomActivities.Resolver;
using Elsa.CustomActivities.OptionsProviders;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.CustomActivities.Describers
{
    public interface ICustomPropertyDescriber
    {
        public IEnumerable<ActivityInputDescriptor> DescribeInputProperties(Type propertyType);
    }

    public class CustomPropertyDescriber : ICustomPropertyDescriber
    {
        private readonly IServiceProvider _serviceProvider;
        public CustomPropertyDescriber(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        public IEnumerable<ActivityInputDescriptor> DescribeInputProperties(Type propertyType)
        {
            var properties = propertyType.GetProperties();
            var inputProperties = DescribeInputProperties(properties).OrderBy(x => x.Order);
            return inputProperties;
        }

        private IEnumerable<ActivityInputDescriptor> DescribeInputProperties(IEnumerable<PropertyInfo> properties, string? category = null)
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
                    GetOptions(activityPropertyAttribute.OptionsProvider),
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

        private object? GetOptions(Type? type)
        {
            if (type != null && type.IsAssignableFrom(typeof(IOptionsProvider)))
            {
                IOptionsProvider optionsProvider = (IOptionsProvider)_serviceProvider.GetRequiredService(type);
                return optionsProvider.GetOptions();
            }
            return null;
        }


    }
}
