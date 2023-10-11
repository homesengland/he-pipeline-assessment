using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Elsa.CustomWorkflow.Sdk.HttpClients
{
    public interface IElsaServerHttpClient
    {
        Task<WorkflowNextActivityDataDto?> PostStartWorkflow(StartWorkflowCommandDto model);
        Task<WorkflowNextActivityDataDto?> PostExecuteWorkflow(ExecuteWorkflowCommandDto model);
        Task<WorkflowNextActivityDataDto?> QuestionScreenSaveAndValidate(WorkflowActivityDataDto model);
        Task<WorkflowNextActivityDataDto?> QuestionScreenSaveAndContinue(QuestionScreenSaveAndContinueCommandDto model);
        Task<WorkflowNextActivityDataDto?> CheckYourAnswersSaveAndContinue(CheckYourAnswersSaveAndContinueCommandDto model);
        Task<WorkflowActivityDataDto?> LoadQuestionScreen(LoadWorkflowActivityDto model);
        Task<WorkflowActivityDataDto?> LoadCheckYourAnswersScreen(LoadWorkflowActivityDto model);
        Task<WorkflowActivityDataDto?> LoadConfirmationScreen(LoadWorkflowActivityDto model);
        Task<string?> LoadCustomActivities(string elsaServer);
        Task<string?> LoadDataDictionary(string elsaServer);
        Task PostArchiveQuestions(string[] workflowInstanceIds);
    }

    public class ElsaServerHttpClient : IElsaServerHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ElsaServerHttpClient> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ITokenProvider _tokenProvider;

        public ElsaServerHttpClient(IHttpClientFactory httpClientFactoryFactory, ILogger<ElsaServerHttpClient> logger, IConfiguration configuration, IHostEnvironment hostEnvironment, ITokenProvider provider)
        {
            _httpClientFactory = httpClientFactoryFactory;
            _logger = logger;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            _tokenProvider = provider;
        }

        public async Task<WorkflowNextActivityDataDto?> PostStartWorkflow(StartWorkflowCommandDto model)
        {
            string data;
            var relativeUri = "workflow/startworkflow" + "?t=" + DateTime.UtcNow.Ticks;

            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var content = JsonSerializer.Serialize(model);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("ElsaServerClient");
            AddAccessTokenToRequest(client);
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

        public async Task<WorkflowNextActivityDataDto?> PostExecuteWorkflow(ExecuteWorkflowCommandDto model)
        {
            string data;
            var relativeUri = "workflow/executeworkflow" + "?t=" + DateTime.UtcNow.Ticks;

            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var content = JsonSerializer.Serialize(model);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("ElsaServerClient");
            AddAccessTokenToRequest(client);
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

                    throw new ApplicationException("Failed to execute workflow");
                }
            }

            return JsonSerializer.Deserialize<WorkflowNextActivityDataDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<WorkflowNextActivityDataDto?> QuestionScreenSaveAndValidate(WorkflowActivityDataDto model)
        {
            string data;
            var relativeUri = "workflow/QuestionScreenValidateAndSave" + "?t=" + DateTime.UtcNow.Ticks;
            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var content = JsonSerializer.Serialize(model);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("ElsaServerClient");
            AddAccessTokenToRequest(client);
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

        public async Task<WorkflowNextActivityDataDto?> QuestionScreenSaveAndContinue(QuestionScreenSaveAndContinueCommandDto model)
        {
            string data;
            var relativeUri = "workflow/QuestionScreenSaveAndContinue" + "?t=" + DateTime.UtcNow.Ticks;

            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var content = JsonSerializer.Serialize(model);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("ElsaServerClient");
            AddAccessTokenToRequest(client);
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
            var relativeUri = "workflow/CheckYourAnswersSaveAndContinue" + "?t=" + DateTime.UtcNow.Ticks;

            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var content = JsonSerializer.Serialize(model);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("ElsaServerClient");
            AddAccessTokenToRequest(client);
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
            string relativeUri = $"workflow/LoadQuestionScreen?workflowInstanceId={model.WorkflowInstanceId}&activityId={model.ActivityId}" + "&t=" + DateTime.UtcNow.Ticks;

            var client = _httpClientFactory.CreateClient("ElsaServerClient");
            AddAccessTokenToRequest(client);
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
            string relativeUri = $"workflow/LoadCheckYourAnswersScreen?workflowInstanceId={model.WorkflowInstanceId}&activityId={model.ActivityId}" + "&t=" + DateTime.UtcNow.Ticks;

            var client = _httpClientFactory.CreateClient("ElsaServerClient");
            AddAccessTokenToRequest(client);
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
            string relativeUri = $"workflow/LoadConfirmationScreen?workflowInstanceId={model.WorkflowInstanceId}&activityId={model.ActivityId}" + "&t=" + DateTime.UtcNow.Ticks;

            var client = _httpClientFactory.CreateClient("ElsaServerClient");
            AddAccessTokenToRequest(client);
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
            string fullUri = $"{elsaServer}/activities/properties" + "?t=" + DateTime.UtcNow.Ticks;
            var client = _httpClientFactory.CreateClient("ElsaServerClient");
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

        public async Task<string?> LoadDataDictionary(string elsaServer)
        {
            string data;
            string fullUri = $"{elsaServer}/activities/dictionary" + "?t=" + DateTime.UtcNow.Ticks;
            var client = _httpClientFactory.CreateClient("ElsaServerClient");
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

        public async Task PostArchiveQuestions(string[] workflowInstanceIds)
        {
            var relativeUri = "workflow/archivequestions" + "?t=" + DateTime.UtcNow.Ticks;

            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var content = JsonSerializer.Serialize(new
            {
                WorkflowInstanceIds = workflowInstanceIds
            });
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient("ElsaServerClient");
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
