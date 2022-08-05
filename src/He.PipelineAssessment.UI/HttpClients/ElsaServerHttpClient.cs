using System.Text;

namespace He.PipelineAssessment.UI.HttpClients
{
    public interface IElsaServerHttpClient
    {
        Task PostStartWorkflow(string workflowDefinitionId);
    }

    public class ElsaServerHttpClient : IElsaServerHttpClient
    {
        private readonly HttpClient _httpClient;

        public ElsaServerHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task PostStartWorkflow(string workflowDefinitionId)
        {
            var data = "";
            var fullUri = "https://localhost:7227/workflow/startworkflow";

            //var content = new StringContent(workflowDefinitionId, Encoding.UTF8, "application/json");

            using (var response = await _httpClient.PostAsJsonAsync(fullUri.ToString(), workflowDefinitionId).ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    throw new ApplicationException($"StatusCode='{response.StatusCode}'," +
                                                   $"\n Message= '{data}'," +
                                                   $"\n Url='{fullUri}'");
            }
        }

        private async Task Post<T>(string eventName, T eventBody) where T : class, new()
        {
            //var data = "";
            //var fullUri = _uriConfig.Value.BuildEventServiceUri(eventName);

            //var content = new StringContent(eventBody.ToJSONSerializedString(), Encoding.UTF8, "application/json");

            //using (var response = await _httpClient.PostAsync(fullUri.ToString(), content).ConfigureAwait(false))
            //{
            //    data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            //    if (!response.IsSuccessStatusCode)
            //        throw new ApplicationException($"StatusCode='{response.StatusCode}'," +
            //                                       $"\n Message= '{data}'," +
            //                                       $"\n Url='{fullUri}'");
            //}
        }
    }



}
