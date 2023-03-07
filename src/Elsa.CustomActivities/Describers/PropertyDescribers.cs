
using Elsa.CustomActivities.Activities;
using Elsa.CustomActivities.OptionsProviders;
using Elsa.CustomActivities.PropertyDecorator;
using Elsa.CustomActivities.Resolver;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Elsa.CustomActivities.Describers
{
    public interface ICustomPropertyDescriber
    {
        public List<HeActivityInputDescriptor> DescribeInputProperties(Type propertyType);
    }

    public class CustomPropertyDescriber : ICustomPropertyDescriber
    {
        private readonly IServiceProvider _serviceProvider;
        public CustomPropertyDescriber(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        public List<HeActivityInputDescriptor> DescribeInputProperties(Type propertyType)
        {
            var properties = propertyType.GetProperties();
            var inputProperties = DescribeInputProperties(properties).OrderBy(x => x.Order);
            return inputProperties.ToList();
        }

        private IEnumerable<HeActivityInputDescriptor> DescribeInputProperties(IEnumerable<PropertyInfo> properties, string? category = null)
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

                var activityPropertyAttribute = propertyInfo.GetCustomAttribute<HeActivityInputAttribute>();

                if (activityPropertyAttribute == null)
                    continue;

                yield return new HeActivityInputDescriptor
                (
                    activityPropertyAttribute.Name ?? propertyInfo.Name,
                    propertyInfo.PropertyType,
                    CustomUIHintResolver.GetUIHint<HeActivityInputAttribute>(propertyInfo),
                    activityPropertyAttribute.Label ?? propertyInfo.Name.Humanize(LetterCasing.Title),
                    OutputTypeHintResolver.GetOutputTypeHint<HeActivityInputAttribute>(propertyInfo),
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
                    activityPropertyAttribute.ConsiderValuesAsOutcomes,
                    activityPropertyAttribute.DisplayInDesigner,
                    activityPropertyAttribute.ConditionalActivityTypes

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
