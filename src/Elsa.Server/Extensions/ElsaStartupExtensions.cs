using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Authentication;
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
        private static string _certificatePath = "";
        private static string _certificateKeyPath = "";

        public static IServiceCollection AddRedisWithSelfSignedSslCertificate(
            this IServiceCollection services,
            string connectionString, string certificatePath, string certificateKeyPath)
        {
            _certificatePath = certificatePath;
            _certificateKeyPath = certificateKeyPath;
            var configurationOptions = ConfigurationOptions.Parse(connectionString);
            configurationOptions.CertificateValidation += CertificateValidationCallBack!;
            configurationOptions.CertificateSelection += OptionsOnCertificateSelection;
            configurationOptions.Ssl = true;
            configurationOptions.SslProtocols = SslProtocols.Tls12;
            configurationOptions.AbortOnConnectFail = false;
            var connectionMultiplexer = (IConnectionMultiplexer)ConnectionMultiplexer.Connect(configurationOptions);
            return services.AddSingleton(connectionMultiplexer);
        }

        private static X509Certificate OptionsOnCertificateSelection(object sender, string targethost, X509CertificateCollection localcertificates, X509Certificate? remotecertificate, string[] acceptableissuers)
        {
            return CreateCertFromPemFile(_certificatePath, _certificateKeyPath);
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
