using He.PipelineAssessment.Data;
using He.PipelineAssessment.Data.Auth;
using He.PipelineAssessment.Data.ExtendedSinglePipeline;
using He.PipelineAssessment.Data.LaHouseNeed;
using He.PipelineAssessment.Data.PCSProfile;
using He.PipelineAssessment.Data.RegionalFigs;
using He.PipelineAssessment.Data.RegionalIPU;
using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Data.VFM;
using He.PipelineAssessment.Data.VoaLandValues.Agricultural;
using He.PipelineAssessment.Data.VoaLandValues.Land;
using He.PipelineAssessment.Data.VoaLandValues.Office;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Elsa.CustomWorkflow.Sdk.Extensions
{
    public static class EsriHttpClients
    {
        public static void AddEsriHttpClients(this IServiceCollection services, IConfiguration config, bool isDevelopmentEnvironment)
        {
            services.AddSinglePipelineClient(config, isDevelopmentEnvironment);
            services.AddSinglePipelineExtendedClient(config, isDevelopmentEnvironment);
            services.AddVFMClient(config, isDevelopmentEnvironment);
            services.AddHousingNeedClient(config, isDevelopmentEnvironment);
            services.AddPCSClient(config, isDevelopmentEnvironment);
            services.AddRegionalIPUClient(config, isDevelopmentEnvironment);
            services.AddRegionalFigsClient(config, isDevelopmentEnvironment);
            services.AddLandValuesClient(config, isDevelopmentEnvironment);
            services.AddOfficeLandValuesClient(config, isDevelopmentEnvironment);
            services.AddAgricultureLandValuesClient(config, isDevelopmentEnvironment);

        }
        public static void AddSinglePipelineClient(this IServiceCollection services, IConfiguration config, bool isDevelopmentEnvironment)
        {
            string serviceUrl = config["Datasources:SinglePipeline"];

            services.AddScoped<IEsriSinglePipelineClient, EsriSinglePipelineClient>();
            services.AddScoped<IEsriSinglePipelineDataJsonHelper, EsriSinglePipelineDataJsonHelper>();

            if (isDevelopmentEnvironment)
            {
                services.AddHttpClient(ClientConstants.SinglePipelineClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                });
            }
            else
            {
                services.AddHttpClient(ClientConstants.SinglePipelineClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                }).AddHttpMessageHandler<BearerTokenHandler>();
            }
        }

        public static void AddSinglePipelineExtendedClient(this IServiceCollection services, IConfiguration config, bool isDevelopmentEnvironment)
        {
            string serviceUrl = config["Datasources:SinglePipelineExtended"];

            services.AddScoped<IEsriSinglePipelineExtendedClient, EsriSinglePipelineExtendedClient>();
            services.AddScoped<IEsriSinglePipelineExtendedDataJsonHelper, EsriSinglePipelineExtendedDataJsonHelper>();

            if (isDevelopmentEnvironment)
            {
                services.AddHttpClient(ClientConstants.SinglePipelineExtendedClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                });
            }
            else
            {
                services.AddHttpClient(ClientConstants.SinglePipelineExtendedClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                }).AddHttpMessageHandler<BearerTokenHandler>();
            }
        }


        public static void AddVFMClient(this IServiceCollection services, IConfiguration config, bool isDevelopmentEnvironment)
        {
            string serviceUrl = config["Datasources:VFM"];

            services.AddScoped<IEsriVFMClient, EsriVFMClient>();
            services.AddScoped<IEsriVFMDataJsonHelper, EsriVFMDataJsonHelper>();

            if (isDevelopmentEnvironment)
            {
                services.AddHttpClient(ClientConstants.VFMClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                });
            }
            else
            {
                services.AddHttpClient(ClientConstants.VFMClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                }).AddHttpMessageHandler<BearerTokenHandler>();
            }
        }

        public static void AddHousingNeedClient(this IServiceCollection services, IConfiguration config, bool isDevelopmentEnvironment)
        {
            string serviceUrl = config["Datasources:HousingNeed"];
            if (isDevelopmentEnvironment)
            {
                services.AddHttpClient(ClientConstants.HouseNeedClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                });
            }
            else
            {
                services.AddHttpClient(ClientConstants.HouseNeedClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                }).AddHttpMessageHandler<BearerTokenHandler>();
            }
        }

        public static void AddPCSClient(this IServiceCollection services, IConfiguration config, bool isDevelopmentEnvironment)
        {
            string serviceUrl = config["Datasources:PCS"];

            services.AddScoped<IEsriLaHouseNeedClient, EsriLAHouseNeedClient>();
            services.AddScoped<IEsriLaHouseNeedDataJsonHelper, EsriLAHouseNeedDataJsonHelper>();

            services.AddScoped<IEsriPCSProfileClient, EsriPCSProfileClient>();
            services.AddScoped<IEsriPCSProfileDataJsonHelper, EsriPCSProfileDataJsonHelper>();

            if (isDevelopmentEnvironment)
            {
                services.AddHttpClient(ClientConstants.PCSClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                });
            }
            else
            {
                services.AddHttpClient(ClientConstants.PCSClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                }).AddHttpMessageHandler<BearerTokenHandler>();
            }
        }

        public static void AddRegionalIPUClient(this IServiceCollection services, IConfiguration config, bool isDevelopmentEnvironment)
        {
            string serviceUrl = config["Datasources:RegionalIPU"];

            services.AddScoped<IEsriRegionalIPUClient, EsriRegionalIPUClient>();
            services.AddScoped<IEsriRegionalIPUJsonHelper, EsriRegionalIPUJsonHelper>();

            if (isDevelopmentEnvironment)
            {
                services.AddHttpClient(ClientConstants.RegionalIPUClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                });
            }
            else
            {
                services.AddHttpClient(ClientConstants.RegionalIPUClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                }).AddHttpMessageHandler<BearerTokenHandler>();
            }
        }

        public static void AddRegionalFigsClient(this IServiceCollection services, IConfiguration config, bool isDevelopmentEnvironment)
        {
            string serviceUrl = config["Datasources:RegionalFigs"];

            services.AddScoped<IEsriRegionalFigsClient, EsriRegionalFigsClient>();
            services.AddScoped<IEsriRegionalFigsJsonHelper, EsriRegionalFigsJsonHelper>();

            if (isDevelopmentEnvironment)
            {
                services.AddHttpClient(ClientConstants.RegionalFigsClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                });
            }
            else
            {
                services.AddHttpClient(ClientConstants.RegionalFigsClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                }).AddHttpMessageHandler<BearerTokenHandler>();
            }
        }

        public static void AddLandValuesClient(this IServiceCollection services, IConfiguration config, bool isDevelopmentEnvironment)
        {
            string serviceUrl = config["Datasources:LandValues"];

            services.AddScoped<ILandValuesClient, LandValuesClient>();
            services.AddScoped<ILandValuesDataJsonHelper, LandValuesDataJsonHelper>();

            if (isDevelopmentEnvironment)
            {
                services.AddHttpClient(ClientConstants.LandValuesClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                });
            }
            else
            {
                services.AddHttpClient(ClientConstants.LandValuesClient, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                }).AddHttpMessageHandler<BearerTokenHandler>();
            }
        }
        public static void AddAgricultureLandValuesClient(this IServiceCollection services, IConfiguration config, bool isDevelopmentEnvironment)
        {
            string serviceUrl = config["Datasources:AgricultureLandValues"];

            services.AddScoped<IAgricultureLandValuesClient, AgricultureLandValuesClient>();
            services.AddScoped<IAgricultureLandValuesDataJsonHelper, AgricultureLandValuesDataJsonHelper>();

            if (isDevelopmentEnvironment)
            {
                services.AddHttpClient(ClientConstants.AgricultureLandValues, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                });
            }
            else
            {
                services.AddHttpClient(ClientConstants.AgricultureLandValues, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                }).AddHttpMessageHandler<BearerTokenHandler>();
            }
        }
        public static void AddOfficeLandValuesClient(this IServiceCollection services, IConfiguration config, bool isDevelopmentEnvironment)
        {
            string serviceUrl = config["Datasources:OfficeLandValues"];

            services.AddScoped<IOfficeLandValuesClient, OfficeLandValuesClient>();
            services.AddScoped<IOfficeLandValuesDataJsonHelper, OfficeLandValuesDataJsonHelper>();

            if (isDevelopmentEnvironment)
            {
                services.AddHttpClient(ClientConstants.OfficeLandValues, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                });
            }
            else
            {
                services.AddHttpClient(ClientConstants.OfficeLandValues, client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                }).AddHttpMessageHandler<BearerTokenHandler>();
            }
        }
    }
}
