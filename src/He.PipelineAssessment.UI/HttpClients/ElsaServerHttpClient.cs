using Elsa.Models;
using He.PipelineAssessment.UI.Models;
using System.Text.Json;

namespace He.PipelineAssessment.UI.HttpClients
{
    public interface IElsaServerHttpClient
    {
        Task<WorkflowNavigationViewModel> PostStartWorkflow(string workflowDefinitionId);
        Task<WorkflowNavigationViewModel> NavigateWorkflow(WorkflowNavigationViewModel model, bool navigateBack = false);
    }

    public class ElsaServerHttpClient : IElsaServerHttpClient
    {
        private readonly HttpClient _httpClient;

        public ElsaServerHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WorkflowNavigationViewModel> PostStartWorkflow(string workflowDefinitionId)
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

            var multipleChoiceQuestionDataModel = JsonSerializer.Deserialize<MultipleChoiceQuestionDataModel>(data);
            var activityData =
                JsonSerializer.Deserialize<Activitydata>(multipleChoiceQuestionDataModel.workflowInstance.activityData.First().Value.ToString());

            return new WorkflowNavigationViewModel
            {
                ActivityData = activityData,
                WorkflowInstanceId = multipleChoiceQuestionDataModel.workflowInstance.id
            };
        }

        public async Task<WorkflowNavigationViewModel> NavigateWorkflow(WorkflowNavigationViewModel model, bool navigateBack)
        {
            var data = "";
            var fullUri = "https://localhost:7227/multiple-choice";
            var postModel = new MultipleChoiceQuestionModel
            {
                QuestionID = model.ActivityData.QuestionID,
                Answer = "something...",
                WorkflowInstanceID = model.WorkflowInstanceId,
                NavigateBack = navigateBack,
                PreviousActivityId = ""
            };

            using (var response = await _httpClient.PostAsJsonAsync(fullUri.ToString(), postModel).ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    throw new ApplicationException($"StatusCode='{response.StatusCode}'," +
                                                   $"\n Message= '{data}'," +
                                                   $"\n Url='{fullUri}'");
            }

            var activityData = JsonSerializer.Deserialize<Activitydata>(data);

            return new WorkflowNavigationViewModel
            {
                ActivityData = activityData,
                WorkflowInstanceId = model.WorkflowInstanceId
            };
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
