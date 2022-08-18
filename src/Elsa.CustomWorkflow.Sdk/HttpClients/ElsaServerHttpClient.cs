using Elsa.CustomWorkflow.Sdk.Mappers;
using Elsa.CustomWorkflow.Sdk.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Elsa.CustomWorkflow.Sdk.HttpClients
{
    public interface IElsaServerHttpClient
    {
        Task<WorkflowNavigationDto?> PostStartWorkflow(string workflowDefinitionId);
        Task<WorkflowNavigationDto?> NavigateWorkflow(WorkflowNavigationDto model, bool navigateBack = false);
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

        public async Task<WorkflowNavigationDto?> NavigateWorkflow(WorkflowNavigationDto model, bool navigateBack)
        {
            string data;
            var fullUri = "https://localhost:7227/multiple-choice";
            var postModel = model.ToMultipleChoiceQuestionDto(navigateBack);

            using (var response = await _httpClient.PostAsJsonAsync(fullUri.ToString(), postModel).ConfigureAwait(false))
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
