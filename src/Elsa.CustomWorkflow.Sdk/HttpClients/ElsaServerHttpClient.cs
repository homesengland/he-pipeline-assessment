using Elsa.CustomWorkflow.Sdk.Mappers;
using Elsa.CustomWorkflow.Sdk.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Elsa.CustomWorkflow.Sdk.HttpClients
{
    public interface IElsaServerHttpClient
    {
        Task<WorkflowNavigationDto?> PostStartWorkflow(string workflowDefinitionId);
        Task<WorkflowNavigationDto?> NavigateWorkflowForward(WorkflowNavigationDto model);
        Task<WorkflowNavigationDto?> NavigateWorkflowBackward(string WorkflowInstanceId, string ActivityId);
    }

    public class ElsaServerHttpClient : IElsaServerHttpClient
    {
        private readonly HttpClient _httpClient;

        public ElsaServerHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WorkflowNavigationDto?> PostStartWorkflow(string workflowDefinitionId)
        {
            string data;
            //TODO: make this uri configurable
            var fullUri = "https://localhost:7227/workflow/startworkflow";

            using (var response = await _httpClient.PostAsJsonAsync(fullUri, workflowDefinitionId).ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    throw new ApplicationException($"StatusCode='{response.StatusCode}'," +
                                                   $"\n Message= '{data}'," +
                                                   $"\n Url='{fullUri}'");
            }

            return JsonSerializer.Deserialize<WorkflowNavigationDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<WorkflowNavigationDto?> NavigateWorkflowForward(WorkflowNavigationDto model)
        {
            string data;
            var fullUri = "https://localhost:7227/multiple-choice/NavigateForward";
            var postModel = model.ToMultipleChoiceQuestionDto(false);

            using (var response = await _httpClient.PostAsJsonAsync(fullUri.ToString(), postModel).ConfigureAwait(false))//forward
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    throw new ApplicationException($"StatusCode='{response.StatusCode}'," +
                                                   $"\n Message= '{data}'," +
                                                   $"\n Url='{fullUri}'");
            }

            return JsonSerializer.Deserialize<WorkflowNavigationDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<WorkflowNavigationDto?> NavigateWorkflowBackward(string workflowInstanceId, string activityId)
        {
            string data;
            var fullUri = $"https://localhost:7227/multiple-choice/NavigateBackward?workflowInstanceId={workflowInstanceId}&activityId={activityId}";
            var model = new BackwardNavigationDto
            {
                ActivityId = activityId,
                WorkflowInstanceId = workflowInstanceId
            };

            using (var response = await _httpClient.GetAsync(fullUri.ToString()).ConfigureAwait(false))//backwards
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    throw new ApplicationException($"StatusCode='{response.StatusCode}'," +
                                                   $"\n Message= '{data}'," +
                                                   $"\n Url='{fullUri}'");
            }

            return JsonSerializer.Deserialize<WorkflowNavigationDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
