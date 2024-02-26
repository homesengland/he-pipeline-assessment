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
    public interface IDataDictionaryHttpClient
    {
        Task<string?> LoadDataDictionary(string url, bool includeArchived);
        Task<string?> LoadDataDictionaryGroup(string url, int groupId);
        Task<string?> LoadDataDictionaryRecord(string url, int itemId);
        Task UpdateDataDictionaryGroup(string url, DataDictionaryGroup group);
        Task CreateDataDictionaryGroup(string url, string name);

        Task UpdateDataDictionaryRecord(string url, DataDictionary item);
        Task CreateDataDictionaryRecord(string url, DataDictionary item);
        Task ArchiveDataDictionaryGroup(string url, int groupId, bool archive);
        Task ArchiveDataDictionaryRecord(string url, int itemId, bool archive);


    }

    public class DataDictionaryHttpClient : IDataDictionaryHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ElsaServerHttpClient> _logger;
        private readonly ITokenProvider _tokenProvider;

        public DataDictionaryHttpClient(IHttpClientFactory httpClientFactoryFactory, ILogger<ElsaServerHttpClient> logger, ITokenProvider provider)
        {
            _httpClientFactory = httpClientFactoryFactory;
            _logger = logger;
            _tokenProvider = provider;
        }

        public async Task ArchiveDataDictionaryGroup(string url, int groupId, bool archive)
        {
            string relativeUri = $"{url}/datadictionary/ArchiveDataDictionaryGroup?t=" + DateTime.UtcNow.Ticks;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var client = _httpClientFactory.CreateClient("DataDictionaryClient");
            var content = JsonSerializer.Serialize(new { Id = groupId, IsArchived = archive });
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            AddAccessTokenToRequest(client);
            using (var response = await client
                       .SendAsync(request)
                       .ConfigureAwait(false))
            {
                await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Url='{request.RequestUri}'");

                    throw new ApplicationException("Failed to archive questions");
                }
            }
        }

        public async Task ArchiveDataDictionaryRecord(string url, int itemId, bool archive)
        {
            string relativeUri = $"{url}/datadictionary/archiveDataDictionaryRecord??t=" + DateTime.UtcNow.Ticks;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var client = _httpClientFactory.CreateClient("DataDictionaryClient");
            var content = JsonSerializer.Serialize(new { Id = itemId, IsArchived = archive });
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            AddAccessTokenToRequest(client);
            using (var response = await client
                       .SendAsync(request)
                       .ConfigureAwait(false))
            {
                await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Url='{request.RequestUri}'");

                    throw new ApplicationException("Failed to archive questions");
                }
            }
        }

        public async Task<string?> LoadDataDictionary(string elsaServer, bool includeArchived = false)
        {
            string data;
            string fullUri = $"{elsaServer}/activities/dictionary?includeArchived={includeArchived}" + "?t=" + DateTime.UtcNow.Ticks;
            var client = _httpClientFactory.CreateClient("DataDictionaryClient");
            AddAccessTokenToRequest(client);
            using (var response = await client
                       .GetAsync(fullUri)
                       .ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Message= '{data}'," +
                                     $"\n Url='{fullUri}'");

                    return default;
                }
            }
            return data;
        }

        public async Task<string?> LoadDataDictionaryGroup(string elsaServer, int groupId)
        {
            string data;
            bool includeArchived = true;
            string fullUri = $"{elsaServer}/activities/dictionary?includeArchived={includeArchived}" + "?t=" + DateTime.UtcNow.Ticks;
            var client = _httpClientFactory.CreateClient("DataDictionaryClient");
            AddAccessTokenToRequest(client);
            using (var response = await client
                       .GetAsync(fullUri)
                       .ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Message= '{data}'," +
                                     $"\n Url='{fullUri}'");

                    return default;
                }
            }
            return data;
        }

        public async Task<string?> LoadDataDictionaryRecord(string elsaServer, int itemId)
        {
            string data;
            bool includeArchived = true;
            string fullUri = $"{elsaServer}/activities/dictionary?includeArchived={includeArchived}" + "?t=" + DateTime.UtcNow.Ticks;
            var client = _httpClientFactory.CreateClient("DataDictionaryClient");
            AddAccessTokenToRequest(client);
            using (var response = await client
                       .GetAsync(fullUri)
                       .ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Message= '{data}'," +
                                     $"\n Url='{fullUri}'");

                    return default;
                }
            }
            return data;
        }

        public async Task UpdateDataDictionaryGroup(string url, DataDictionaryGroup group)
        {
            string relativeUri = $"{url}/datadictionary/UpdateDataDictionaryGroup" + "?t=" + DateTime.UtcNow.Ticks;

            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var content = JsonSerializer.Serialize(new
            {
                group = group
            });
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("DataDictionaryClient");
            AddAccessTokenToRequest(client);
            using (var response = await client
                       .SendAsync(request)
                       .ConfigureAwait(false))
            {
                await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Url='{request.RequestUri}'");

                    throw new ApplicationException("Failed to Update Data Dictionary Group");
                }
            }
        }

        public async Task CreateDataDictionaryGroup(string url, string name)
        {
            string relativeUri = $"{url}/datadictionary/CreateDataDictionaryGroup" + "?t=" + DateTime.UtcNow.Ticks;


            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var content = JsonSerializer.Serialize(new
            {
                name = name
            });
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("DataDictionaryClient");
            AddAccessTokenToRequest(client);
            using (var response = await client
                       .SendAsync(request)
                       .ConfigureAwait(false))
            {
                var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Url='{request.RequestUri}'");

                    throw new ApplicationException("Failed to archive questions");
                }
            }
        }

        public async Task CreateDataDictionaryRecord(string url, DataDictionary record)
        {

            string relativeUri = $"{url}/datadictionary/CreateDataDictionaryRecord" + "?t=" + DateTime.UtcNow.Ticks;

            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var content = JsonSerializer.Serialize(new
            {
                DictionaryRecord = record
            });
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("DataDictionaryClient");
            AddAccessTokenToRequest(client);
            using (var response = await client
                       .SendAsync(request)
                       .ConfigureAwait(false))
            {
                await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Url='{request.RequestUri}'");

                    throw new ApplicationException("Failed to create Data Dictionary Item");
                }
            }
        }

        public async Task UpdateDataDictionaryRecord(string url, DataDictionary record)
        {

            string relativeUri = $"{url}/datadictionary/UpdateDataDictionaryRecord" + "?t=" + DateTime.UtcNow.Ticks;
            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var content = JsonSerializer.Serialize(new
            {
                Record = record
            });
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("DataDictionaryClient");
            AddAccessTokenToRequest(client);
            using (var response = await client
                       .SendAsync(request)
                       .ConfigureAwait(false))
            {
                await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Url='{request.RequestUri}'");

                    throw new ApplicationException("Failed to archive questions");
                }
            }
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
