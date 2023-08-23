﻿using System.Net.Security;
using System.Runtime.InteropServices;
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
            configurationOptions.CertificateSelection += OptionsOnCertificateSelection;
            var connectionMultiplexer = (IConnectionMultiplexer)ConnectionMultiplexer.Connect(configurationOptions);
            return services.AddSingleton(connectionMultiplexer);
        }

        private static X509Certificate OptionsOnCertificateSelection(object sender, string targethost, X509CertificateCollection localcertificates, X509Certificate? remotecertificate, string[] acceptableissuers)
        {
            return CreateCertFromPemFile("/mnt/redis-tls.crt", "/mnt/redis-tls.key");
        }

        static X509Certificate2 CreateCertFromPemFile(string certPath, string keyPath)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return X509Certificate2.CreateFromPemFile(certPath, keyPath);

            using var cert = X509Certificate2.CreateFromPemFile(certPath, keyPath);
            return new X509Certificate2(cert.Export(X509ContentType.Pkcs12));
        }

        private static bool CertificateValidationCallBack(
            object sender,
            X509Certificate certificate,
            X509Chain? chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
