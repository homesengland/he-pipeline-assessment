using Elsa.CustomActivities.Activities.Currency;
using Elsa.CustomActivities.Activities.Date;
using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.CustomActivities.Activities.SingleChoice;
using Elsa.CustomActivities.Activities.Text;

namespace Elsa.Server.Extensions
{
    public static class ElsaStartupExtensions
    {
        public static void AddCustomElsaScriptHandlers(this IServiceCollection services)
        {
            services.AddNotificationHandlers(typeof(GetMultipleChoiceQuestionScriptHandler));
            services.AddNotificationHandlers(typeof(GetSingleChoiceQuestionScriptHandler));
            services.AddNotificationHandlers(typeof(GetCurrencyQuestionScriptHandler));
            services.AddNotificationHandlers(typeof(GetTextQuestionScriptHandler));
            services.AddNotificationHandlers(typeof(GetDateQuestionScriptHandler));
            services.AddJavaScriptTypeDefinitionProvider<CustomTypeDefinitionProvider>();
        }
    }
}
