﻿using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Elsa.CustomActivities.Activities.QuestionScreen.Helpers;
using Elsa.Options;
using Medallion.Threading;
using Medallion.Threading.Redis;
using RedLockNet.SERedis.Configuration;
using RedLockNet.SERedis;
using RedLockNet;
using StackExchange.Redis;
using Elsa.Extensions;

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
            this ElsaOptionsBuilder options, bool enableInEnvironment, ILogger logger)
        {
            logger.LogInformation($"Should enable Redis?: {enableInEnvironment}");
            if (enableInEnvironment)
            {
                try
                {
                    options
                        .UseRedisCacheSignal()
                        .ConfigureDistributedLockProvider(o => o.UseRedisLockProvider());
                    return options;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Exception thrown whilst setting up Redis ConnectionMultiplexer");
                }

            }

            return options;
        }

        private static DistributedLockingOptionsBuilder UseRedisLockProvider(this DistributedLockingOptionsBuilder options)
        {
            options.Services.AddRedLockFactory();
            options.UseProviderFactory(CreateRedisDistributedLockFactory);

            return options;
        }

        private static Func<string, IDistributedLock> CreateRedisDistributedLockFactory(IServiceProvider services)
        {
            var multiplexer = services.GetRequiredService<IConnectionMultiplexer>();
            return name => new RedisDistributedLock(name, multiplexer.GetDatabase());
        }

        private static IServiceCollection AddRedLockFactory(this IServiceCollection services) =>
            services.AddSingleton<IDistributedLockFactory, RedLockFactory>(sp => RedLockFactory.Create(
                new[]
                {
                    new RedLockMultiplexer(sp.GetRequiredService<IConnectionMultiplexer>())
                }));
    }

    public static class RedisServiceCollectionExtensions
    {
        private static string? _certificatePath = "";
        private static string? _certificateKeyPath = "";

        public static async Task<IServiceCollection> AddRedisWithSelfSignedSslCertificate(
            this IServiceCollection services,
            string connectionString, string? certificatePath, string? certificateKeyPath, ILogger logger, bool clearCache = false)
        {
            _certificatePath = certificatePath;
            _certificateKeyPath = certificateKeyPath;
            var configurationOptions = ConfigurationOptions.Parse(connectionString);
            configurationOptions.CertificateValidation += CertificateValidationCallBack!;
            if (!string.IsNullOrWhiteSpace(_certificatePath))
            {
                configurationOptions.CertificateSelection += OptionsOnCertificateSelection;
            }

            if (clearCache)
            {
                configurationOptions.AllowAdmin = true;
            }
            configurationOptions.Ssl = true;
            configurationOptions.SslProtocols = SslProtocols.Tls13;
            configurationOptions.AbortOnConnectFail = false;
            var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(configurationOptions);
            services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
            connectionMultiplexer.IncludeDetailInExceptions = true;
            logger.LogInformation($"Check SSL RedisConnection.  Is connected: {connectionMultiplexer.IsConnected}");
            if (connectionMultiplexer.IsConnected)
            {
                await RedisSmokeTest(connectionMultiplexer, logger);
            }

            if (clearCache)
            {
                await ClearCache(logger, connectionMultiplexer);
            }
            return services;
        }

        private static async Task ClearCache(ILogger logger, IConnectionMultiplexer connectionMultiplexer)
        {
            logger.LogInformation("Clearing Cache");

            try
            {
                var endPoints = connectionMultiplexer.GetEndPoints();
                foreach (var endPoint in endPoints)
                {
                    var server = connectionMultiplexer.GetServer(endPoint);
                    await server.FlushAllDatabasesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to clear cache: {ex.Message}", ex);
            }
        }

        public static async Task<IServiceCollection> AddRedisLocal(
            this IServiceCollection services,
            string connectionString, ILogger logger, bool clearCache = false)
        {
            if (clearCache)
            {
                connectionString += ",allowAdmin=true";
            }
            var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(connectionString);
            services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
            if (clearCache)
            {
                await ClearCache(logger, connectionMultiplexer);
            }

            return services;
        }

        private static X509Certificate OptionsOnCertificateSelection(object sender, string targethost, X509CertificateCollection localcertificates, X509Certificate? remotecertificate, string[] acceptableissuers)
        {
            return CreateCertFromPemFile(_certificatePath, _certificateKeyPath);
        }

        private static async Task RedisSmokeTest(IConnectionMultiplexer conn, ILogger logger)
        {
            string sampleKey = "TestKey";
            string sampleValue = "SampleValue";
            try {
                var db = conn.GetDatabase();
                logger.LogInformation("Setting Key in Redis DB");
                await db.StringSetAsync(sampleKey, sampleValue);
                logger.LogInformation($"Key successfully set: {sampleKey}");
                logger.LogInformation($"Attempting to retrieve key from DB");
                var result = await db.StringGetAsync(sampleKey);
                logger.LogInformation($"Key retrieved from DB: {result}");
                if(result.ToString() != sampleValue)
                {
                    throw new Exception($"Returned value from Smoke Test was not expected value:'{result.ToString()}' is not equal to '{sampleValue}'");
                }
                //logger.LogInformation("Deleting Key from DB");
                //await db.KeyDeleteAsync(sampleKey);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, $"Error whilst performing smoke tests of Redis DB.  Error Type: {ex.GetType().Name}, Error Message: {ex.Message}.  Inner Message: {ex.InnerException}");
            }
        }

        static X509Certificate2 CreateCertFromPemFile(string? certPath, string? keyPath)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return X509Certificate2.CreateFromPemFile(certPath!, keyPath);

            using var cert = X509Certificate2.CreateFromPemFile(certPath!, keyPath);
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
