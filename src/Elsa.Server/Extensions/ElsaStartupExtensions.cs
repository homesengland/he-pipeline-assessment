using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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

    public static class RedisServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisNoSsl(
            this IServiceCollection services,
            string connectionString)
        {
            var configurationOptions = ConfigurationOptions.Parse(connectionString);
            configurationOptions.CertificateValidation += CertificateValidationCallBack!;
            var connectionMultiplexer = (IConnectionMultiplexer)ConnectionMultiplexer.Connect(configurationOptions);
            return services.AddSingleton(connectionMultiplexer);
        }

        private static bool CertificateValidationCallBack(
            object sender,
            X509Certificate certificate,
            X509Chain? chain,
            SslPolicyErrors sslPolicyErrors)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null)
                {
                    foreach (X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) &&
                            (status.Status == X509ChainStatusFlags.UntrustedRoot))
                        {
                            // Self-signed certificates with an untrusted root are valid. 
                            continue;
                        }
                        else
                        {
                            if (status.Status != X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }
                }

                // When processing reaches this line, the only errors in the certificate chain are 
                // untrusted root errors for self-signed certificates. These certificates are valid
                // for default Exchange server installations, so return true.
                return true;
            }
            else
            {
                // In all other cases, return false.
                return false;
            }
        }
    }
}
