using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Elsa.CustomWorkflow.Sdk.HttpClients
{
    public interface IElsaServerHttpClient
    {
        Task<WorkflowNextActivityDataDto?> PostStartWorkflow(StartWorkflowCommandDto model);
        Task<WorkflowNextActivityDataDto?> SaveAndContinue(SaveAndContinueCommandDto model);
        Task<WorkflowActivityDataDto?> LoadQuestionScreen(LoadWorkflowActivityDto model);
        Task<WorkflowActivityDataDto?> LoadCheckYourAnswersScreen(LoadWorkflowActivityDto model);
    }

    public class ElsaServerHttpClient : IElsaServerHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ElsaServerHttpClient> _logger;

        public ElsaServerHttpClient(IHttpClientFactory httpClientFactoryFactory, ILogger<ElsaServerHttpClient> logger)
        {
            _httpClientFactory = httpClientFactoryFactory;
            _logger = logger;
        }

        public async Task<WorkflowNextActivityDataDto?> PostStartWorkflow(StartWorkflowCommandDto model)
        {
            string data;
            var relativeUri = "workflow/startworkflow";

            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var content = JsonSerializer.Serialize(model);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            using (var response = await _httpClientFactory.CreateClient("ElsaServerClient")
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



        public async Task<WorkflowNextActivityDataDto?> SaveAndContinue(SaveAndContinueCommandDto model)
        {
            string data;
            var relativeUri = "workflow/QuestionScreenSaveAndContinue";

            using var request = new HttpRequestMessage(HttpMethod.Post, relativeUri);
            var content = JsonSerializer.Serialize(model);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            using (var response = await _httpClientFactory.CreateClient("ElsaServerClient")
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

            using (var response = await _httpClientFactory.CreateClient("ElsaServerClient")
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

        public async Task<WorkflowActivityDataDto?> LoadCheckYourAnswersScreen(LoadWorkflowActivityDto model)
        {
            string data;
            string relativeUri = $"workflow/LoadCheckYourAnswersScreen?workflowInstanceId={model.WorkflowInstanceId}&activityId={model.ActivityId}";

            using (var response = await _httpClientFactory.CreateClient("ElsaServerClient")
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

    }
}
