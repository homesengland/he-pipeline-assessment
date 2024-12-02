using Elsa.Server.Api.Attributes;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Elsa.Server.Extensions
{
    public class CustomElsaNewtonsoftJsonConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (ShouldApplyConvention(controller))
            {
                NewtonsoftJsonFormatterAttribute newtonsoftJsonFormatterAttribute = new NewtonsoftJsonFormatterAttribute();
                newtonsoftJsonFormatterAttribute.Apply(controller);
                controller.Filters.Add(newtonsoftJsonFormatterAttribute);
            }
        }

        private bool ShouldApplyConvention(ControllerModel controller)
        {
            if (controller != null && 
                (controller.ControllerType?.FullName?.StartsWith("Elsa.Server.Features.Dashboard.CustomList") == true ||
                (controller.ControllerType?.FullName?.StartsWith("Elsa.Server.Features.Dashboard.CustomHistory") == true)))

            {
                return !controller.Attributes.Any((object x) => x.GetType() == typeof(NewtonsoftJsonFormatterAttribute));
            }

            return false;
        }
    }
}
