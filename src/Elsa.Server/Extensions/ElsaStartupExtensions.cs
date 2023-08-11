using Elsa.CustomActivities.Activities.QuestionScreen.Helpers;
using Elsa.Extensions;
using Elsa.Options;
using Medallion.Threading.Redis;
using StackExchange.Redis;

namespace Elsa.Server.Extensions
{
    public static class ElsaStartupExtensions
    {
        public static void AddCustomElsaScriptHandlers(this IServiceCollection services)
        {
            var activityTypes = new List<Type>
            {
                typeof(QuestionHelper),
                typeof(TextQuestionHelper),
            };
            services.AddNotificationHandlers(activityTypes.ToArray());
            services.AddJavaScriptTypeDefinitionProvider<CustomTypeDefinitionProvider>();
        }
    }

    public static class ElsaOptionsBuilderExtensions
    {
        public static ElsaOptionsBuilder AddRedisCache(
            this ElsaOptionsBuilder options, bool enableInEnvironment)
        {
            if (enableInEnvironment)
            {
                options.UseRedisCacheSignal()
                    .ConfigureDistributedLockProvider(o => o.UseProviderFactory(sp => name =>
                    {
                        var connection =
                            sp.GetRequiredService<
                                IConnectionMultiplexer>();
                        return new RedisDistributedLock(name, connection.GetDatabase());
                    }));
            }

            return options;
        }
    }
}
