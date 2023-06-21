using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using Azure.Core;
using Azure.Identity;
using System.Runtime;
using System.Net.Http.Headers;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.Extensions.Configuration;

namespace Elsa.CustomWorkflow.Sdk.HttpClients
{
    public interface IElsaServerHttpClient
    {
        Task<WorkflowNextActivityDataDto?> PostStartWorkflow(StartWorkflowCommandDto model);
        Task<WorkflowNextActivityDataDto?> QuestionScreenSaveAndContinue(QuestionScreenSaveAndContinueCommandDto model);
        Task<WorkflowNextActivityDataDto?> CheckYourAnswersSaveAndContinue(CheckYourAnswersSaveAndContinueCommandDto model);
        Task<WorkflowActivityDataDto?> LoadQuestionScreen(LoadWorkflowActivityDto model);
        Task<WorkflowActivityDataDto?> LoadCheckYourAnswersScreen(LoadWorkflowActivityDto model);
        Task<WorkflowActivityDataDto?> LoadConfirmationScreen(LoadWorkflowActivityDto model);
        Task<string?> LoadCustomActivities(string elsaServer);
    }

    public class ElsaServerHttpClient : IElsaServerHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ElsaServerHttpClient> _logger;
        private readonly IConfiguration _configuration;

        public ElsaServerHttpClient(IHttpClientFactory httpClientFactoryFactory, ILogger<ElsaServerHttpClient> logger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactoryFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<WorkflowNextActivityDataDto?> PostStartWorkflow(StartWorkflowCommandDto model)
        {
            string data;
            var relativeUri = "workflow/startworkflow";

            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var content = JsonSerializer.Serialize(model);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("ElsaServerClient");
            var accessToken = await GetAccessToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            using (var response = await client
                       .SendAsync(request)
                       .ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                                                      $"\n Message= '{data}'," +
                                                                      $"\n Url='{request.RequestUri}'");

                    throw new ApplicationException("Failed to start workflow");
                }
            }

            return JsonSerializer.Deserialize<WorkflowNextActivityDataDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }



        public async Task<WorkflowNextActivityDataDto?> QuestionScreenSaveAndContinue(QuestionScreenSaveAndContinueCommandDto model)
        {
            string data;
            var relativeUri = "workflow/QuestionScreenSaveAndContinue";

            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var content = JsonSerializer.Serialize(model);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("ElsaServerClient");
            var accessToken = await GetAccessToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            using (var response = await client
                       .SendAsync(request)
                       .ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Message= '{data}'," +
                                     $"\n Url='{request.RequestUri}'");

                    return null;
                }
            }

            return JsonSerializer.Deserialize<WorkflowNextActivityDataDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<WorkflowNextActivityDataDto?> CheckYourAnswersSaveAndContinue(CheckYourAnswersSaveAndContinueCommandDto model)
        {
            string data;
            var relativeUri = "workflow/CheckYourAnswersSaveAndContinue";

            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var content = JsonSerializer.Serialize(model);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("ElsaServerClient");
            var accessToken = await GetAccessToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            using (var response = await client
                       .SendAsync(request)
                       .ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Message= '{data}'," +
                                     $"\n Url='{request.RequestUri}'");

                    return null;
                }
            }

            return JsonSerializer.Deserialize<WorkflowNextActivityDataDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<WorkflowActivityDataDto?> LoadQuestionScreen(LoadWorkflowActivityDto model)
        {
            string data;
            string relativeUri = $"workflow/LoadQuestionScreen?workflowInstanceId={model.WorkflowInstanceId}&activityId={model.ActivityId}";

            var client = _httpClientFactory.CreateClient("ElsaServerClient");
            var accessToken = await GetAccessToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            using (var response = await client
                       .GetAsync(relativeUri)
                       .ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Message= '{data}'," +
                                     $"\n Url='{relativeUri}'");

                    return default;
                }
            }
            var deserializedResult = JsonSerializer.Deserialize<WorkflowActivityDataDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return deserializedResult;
        }

        public async Task<WorkflowActivityDataDto?> LoadCheckYourAnswersScreen(LoadWorkflowActivityDto model)
        {
            string data;
            string relativeUri = $"workflow/LoadCheckYourAnswersScreen?workflowInstanceId={model.WorkflowInstanceId}&activityId={model.ActivityId}";

            var client = _httpClientFactory.CreateClient("ElsaServerClient");
            var accessToken = await GetAccessToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            using (var response = await client
                       .GetAsync(relativeUri)
                       .ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Message= '{data}'," +
                                     $"\n Url='{relativeUri}'");

                    return default;
                }
            }

            return JsonSerializer.Deserialize<WorkflowActivityDataDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<WorkflowActivityDataDto?> LoadConfirmationScreen(LoadWorkflowActivityDto model)
        {
            string data;
            string relativeUri = $"workflow/LoadConfirmationScreen?workflowInstanceId={model.WorkflowInstanceId}&activityId={model.ActivityId}";

            var client = _httpClientFactory.CreateClient("ElsaServerClient");
            var accessToken = await GetAccessToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            using (var response = await client
                       .GetAsync(relativeUri)
                       .ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Message= '{data}'," +
                                     $"\n Url='{relativeUri}'");

                    return default;
                }
            }

            return JsonSerializer.Deserialize<WorkflowActivityDataDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<string?> LoadCustomActivities(string elsaServer)
        {
            string data;
            string fullUri = $"{elsaServer}/activities/properties";
            var client = _httpClientFactory.CreateClient("ElsaServerClient");
            var accessToken = await GetAccessToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
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

        private async Task<string?> GetAccessToken()
        {
            try
            {
                var credential = new ManagedIdentityCredential();

                var ccflowApplicationIdUri = _configuration["AzureManagedIdentityConfig:ElsaServerAzureApplicationIdUri"];
                var accessTokenRequest = await credential.GetTokenAsync(
                    new TokenRequestContext(scopes: new string[] { ccflowApplicationIdUri }) { }
                );

                var accessToken = accessTokenRequest.Token;

                _logger.LogError("Access Token: ", (String)accessToken);

                return accessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
