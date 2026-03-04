using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Elsa.CustomModels;
using Azure.Core;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Elsa.CustomWorkflow.Sdk.HttpClients
{
    public interface IGlobalVariableHttpClient
    {
        Task<string?> LoadGlobalVariable(string url, bool includeArchived);
        Task<string?> LoadGlobalVariableGroup(string url, int groupId);
        Task<string?> LoadGlobalVariableInstance(string url, int itemId);
        Task UpdateGlobalVariableGroup(string url, DataDictionaryGroup group);
        Task CreateGlobalVariableGroup(string url, string name);
        Task UpdateGlobalVariableInstance(string url, DataDictionary item);
        Task CreateGlobalVariableInstance(string url, DataDictionary item);
        Task ArchiveGlobalVariableGroup(string url, int groupId, bool archive);
        Task ArchiveGlobalVariableInstance(string url, int itemId, bool archive);


    }

    public class GlobalVariableHttpClient : IGlobalVariableHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ElsaServerHttpClient> _logger;
        private readonly ITokenProvider _tokenProvider;

        public GlobalVariableHttpClient(IHttpClientFactory httpClientFactoryFactory, ILogger<ElsaServerHttpClient> logger, ITokenProvider provider)
        {
            _httpClientFactory = httpClientFactoryFactory;
            _logger = logger;
            _tokenProvider = provider;
        }

        public async Task ArchiveGlobalVariableGroup(string url, int groupId, bool archive)
        {
            //TODO
            return;
        }

        public async Task ArchiveGlobalVariableInstance(string url, int itemId, bool archive)
        {
            //TODO
        }

        public async Task<string?> LoadGlobalVariable(string elsaServer, bool includeArchived = false)
        {
            //TODO
            return "";
        }

        public async Task<string?> LoadGlobalVariableGroup(string elsaServer, int groupId)
        {
            //TODO
            return ""; 
        }

        public async Task<string?> LoadGlobalVariableInstance(string elsaServer, int itemId)
        {
            //TODO
            return "";
        }

        public async Task UpdateGlobalVariableGroup(string url, DataDictionaryGroup group)
        {
            //TODO
        }

        public async Task CreateGlobalVariableGroup(string url, string name)
        {
            //TODO
        }

        public async Task CreateGlobalVariableInstance(string url, DataDictionary record)
        {

            //TODO
        }

        public async Task UpdateGlobalVariableInstance(string url, DataDictionary record)
        {

            //TODO
        }

        private void AddAccessTokenToRequest(HttpClient client)
        {
            var accessToken = GetAuth0AccessToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private string GetAuth0AccessToken()
        {
            try
            {
                var token = _tokenProvider.GetToken(true);
                if (token != null)
                {
                    return token.AccessToken;
                }
                else return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return string.Empty;
            }
        }

    }
}
