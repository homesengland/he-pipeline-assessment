using Elsa.CustomActivities.Activities.QuestionScreen;

namespace Elsa.Server.Extensions
{
    public static class ElsaStartupExtensions
    {
        public static void AddCustomElsaScriptHandlers(this IServiceCollection services)
        {
            var activityTypes = new List<Type>
            {
                //typeof(GetMultipleChoiceQuestionScriptHandler),
                //typeof(GetSingleChoiceQuestionScriptHandler),
                typeof(GetQuestionScreenScriptHandler),
                //typeof(GetCurrencyQuestionScriptHandler),
                //typeof(GetDateQuestionScriptHandler),
                //typeof(GetTextQuestionScriptHandler)
            };
            services.AddNotificationHandlers(activityTypes.ToArray());
            services.AddJavaScriptTypeDefinitionProvider<CustomTypeDefinitionProvider>();
            //services.AddNotificationHandlers(typeof(GetMultipleChoiceQuestionScriptHandler));
            //services.AddNotificationHandlers(typeof(GetSingleChoiceQuestionScriptHandler));
            //services.AddNotificationHandlers(typeof(GetCurrencyQuestionScriptHandler));
            //services.AddNotificationHandlers(typeof(GetTextQuestionScriptHandler));
            //services.AddNotificationHandlers(typeof(GetDateQuestionScriptHandler));
            //services.AddNotificationHandlers(typeof(GetQuestionScreenScriptHandler));

        }
    }
}
