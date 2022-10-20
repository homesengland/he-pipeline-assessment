using Elsa.CustomActivities.Activities.Shared;

namespace Elsa.Server.Extensions
{
    public static class ElsaStartupExtensions
    {
        public static void AddCustomElsaScriptHandlers(this IServiceCollection services)
        {
            services.AddJavaScriptTypeDefinitionProvider<CustomTypeDefinitionProvider>();
            services.AddNotificationHandlers(typeof(GetScriptHandler));
        }
    }
}
