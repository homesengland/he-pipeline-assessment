using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using System.Text;
using System.Text.Json;
using Elsa.CustomWorkflow.Sdk.Models;

namespace Elsa.CustomWorkflow.Sdk.HttpClients
{
    public interface IElsaServerHttpClient
    {
        Task<WorkflowNextActivityDataDto?> PostStartWorkflow(StartWorkflowCommandDto model);

        Task<WorkflowNextActivityDataDto?> SaveAndContinue(SaveAndContinueCommandDto model);
        Task<WorkflowActivityDataDto?> LoadWorkflowActivity(LoadWorkflowActivityDto model);
    }

    public class ElsaServerHttpClient : IElsaServerHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ElsaServerHttpClient(IHttpClientFactory httpClientFactoryFactory)
        {
            _httpClientFactory = httpClientFactoryFactory;
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
                    //TODO: Get this in logging
                    //throw new ApplicationException($"StatusCode='{response.StatusCode}'," +
                    //                                                  $"\n Message= '{data}'," +
                    //                                                  $"\n Url='{request.RequestUri}'");

                    return null;
                }
            }

            return JsonSerializer.Deserialize<WorkflowNextActivityDataDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<WorkflowNextActivityDataDto?> SaveAndContinue(SaveAndContinueCommandDto model)
        {
            string data;
            var relativeUri = "workflow/SaveAndContinue";

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
                    //TODO: Get this in logging
                    //throw new ApplicationException($"StatusCode='{response.StatusCode}'," +
                    //                               $"\n Message= '{data}'," +
                    //                               $"\n Url='{request.RequestUri}'");

                    return null;
                }
            }

            return JsonSerializer.Deserialize<WorkflowNextActivityDataDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<WorkflowActivityDataDto?> LoadWorkflowActivity(LoadWorkflowActivityDto model)
        {
            string data;
            var relativeUri = $"workflow/LoadWorkflowActivity?workflowInstanceId={model.WorkflowInstanceId}&activityId={model.ActivityId}";

            using (var response = await _httpClientFactory.CreateClient("ElsaServerClient")
                       .GetAsync(relativeUri)
                       .ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    //TODO: Get this in logging
                    //throw new ApplicationException($"StatusCode='{response.StatusCode}'," +
                    //                               $"\n Message= '{data}'," +
                    //                               $"\n Url='{relativeUri}'"); //TODO: Get the full url here

                    return default;
                }
            }

            return JsonSerializer.Deserialize<WorkflowActivityDataDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


    }
}
