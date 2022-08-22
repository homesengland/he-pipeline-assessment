using Elsa.CustomWorkflow.Sdk.Models.StartWorkflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using System.Net.Http.Json;
using System.Text.Json;
using Elsa.CustomWorkflow.Sdk.Models.MultipleChoice.SaveAndContinue;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.LoadWorkflowActivity;

namespace Elsa.CustomWorkflow.Sdk.HttpClients
{
    public interface IElsaServerHttpClient
    {
        Task<WorkflowActivityDataDto?> PostStartWorkflow(StartWorkflowCommandDto model);
        Task<WorkflowActivityDataDto?> SaveAndContinue(SaveAndContinueCommandDto model);
        Task<WorkflowActivityDataDto?> LoadWorkflowActivity(LoadWorkflowActivityDto model);
    }

    public class ElsaServerHttpClient : IElsaServerHttpClient
    {
        private readonly HttpClient _httpClient;

        public ElsaServerHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WorkflowActivityDataDto?> PostStartWorkflow(StartWorkflowCommandDto model)
        {
            string data;
            //TODO: make this uri configurable
            var fullUri = "https://localhost:7227/workflow/startworkflow";

            using (var response = await _httpClient.PostAsJsonAsync(fullUri, model).ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    throw new ApplicationException($"StatusCode='{response.StatusCode}'," +
                                                   $"\n Message= '{data}'," +
                                                   $"\n Url='{fullUri}'");
            }

            return JsonSerializer.Deserialize<WorkflowActivityDataDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<WorkflowActivityDataDto?> SaveAndContinue(SaveAndContinueCommandDto model)
        {
            string data;
            var fullUri = "https://localhost:7227/multiple-choice/SaveAndContinue";

            using (var response = await _httpClient.PostAsJsonAsync(fullUri.ToString(), model).ConfigureAwait(false))//forward
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    throw new ApplicationException($"StatusCode='{response.StatusCode}'," +
                                                   $"\n Message= '{data}'," +
                                                   $"\n Url='{fullUri}'");
            }

            return JsonSerializer.Deserialize<WorkflowActivityDataDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<WorkflowActivityDataDto?> LoadWorkflowActivity(LoadWorkflowActivityDto model)
        {
            string data;
            var fullUri = $"https://localhost:7227/workflow/LoadWorkflowActivity?workflowInstanceId={model.WorkflowInstanceId}&activityId={model.ActivityId}";

            using (var response = await _httpClient.GetAsync(fullUri.ToString()).ConfigureAwait(false))//backwards
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    throw new ApplicationException($"StatusCode='{response.StatusCode}'," +
                                                   $"\n Message= '{data}'," +
                                                   $"\n Url='{fullUri}'");
            }

            return JsonSerializer.Deserialize<WorkflowActivityDataDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
