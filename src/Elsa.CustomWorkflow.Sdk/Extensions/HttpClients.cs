using He.PipelineAssessment.Data;
using He.PipelineAssessment.Data.Auth;
using He.PipelineAssessment.Data.LaHouseNeed;
using He.PipelineAssessment.Data.PCSProfile;
using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Data.VFM;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Elsa.CustomWorkflow.Sdk.Extensions
{
    public static class EsriHttpClients
    {
        public static void AddEsriHttpClients(this IServiceCollection services, IConfiguration config, bool isDevelopmentEnvironment)
        {
            services.AddSinglePipelineClient(config, isDevelopmentEnvironment);
            services.AddVFMClient(config, isDevelopmentEnvironment);
            services.AddHousingNeedClient(config, isDevelopmentEnvironment);
            services.AddPCSClient(config, isDevelopmentEnvironment);
            
        }

        public static void AddSinglePipelineClient(this IServiceCollection services, IConfiguration config, bool isDevelopmentEnvironment)
        {
            string serviceUrl = config["Datasources:SinglePipeline"];

            services.AddScoped<IEsriSinglePipelineClient, EsriSinglePipelineClient>();
            services.AddScoped<IEsriSinglePipelineDataJsonHelper, EsriSinglePipelineDataJsonHelper>();

            if (isDevelopmentEnvironment)
            {
                services.AddHttpClient("SinglePipelineClient", client =>
                {
                    client.BaseAddress = new Uri(serviceUrl);
                });
            }
            else
            {
                services.AddHttpClient("SinglePipelineClient", client =>
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


    }
}
