using He.PipelineAssessment.UI.Models;
using System.Text.Json;
using AutoMapper;
using Elsa.CustomModels;
using Activitydata = He.PipelineAssessment.UI.Models.Activitydata;

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
        private readonly IMapper _mapper;

        public ElsaServerHttpClient(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
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
            var activityId = multipleChoiceQuestionDataModel.workflowInstance.lastExecutedActivityId;
            var activityData =
                JsonSerializer.Deserialize<Activitydata>(multipleChoiceQuestionDataModel.workflowInstance.activityData.First(x => x.Key == activityId).Value.ToString());

            return new WorkflowNavigationViewModel
            {
                ActivityData = activityData,
                WorkflowInstanceId = multipleChoiceQuestionDataModel.workflowInstance.id,
                ActivityId = activityId
            };
        }

        public async Task<WorkflowNavigationViewModel> NavigateWorkflow(WorkflowNavigationViewModel model, bool navigateBack)
        {
            var data = "";
            var fullUri = "https://localhost:7227/multiple-choice";
            var postModel = new MultipleChoiceQuestionModel
            {
                Id = $"{model.WorkflowInstanceId}-{model.ActivityData.QuestionID}",
                QuestionID = model.ActivityData.QuestionID,
                Answer = string.Join(',', model.ActivityData.Choices.Where(x => x is { isSelected: true }).Select(x => x.answer)),
                WorkflowInstanceID = model.WorkflowInstanceId,
                NavigateBack = navigateBack,
                PreviousActivityId = "",
                ActivityID = model.ActivityId
            };

            using (var response = await _httpClient.PostAsJsonAsync(fullUri.ToString(), postModel).ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    throw new ApplicationException($"StatusCode='{response.StatusCode}'," +
                                                   $"\n Message= '{data}'," +
                                                   $"\n Url='{fullUri}'");
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var activityData = JsonSerializer.Deserialize<ActivityDataModel>(data, options);
            if (activityData.activityData.Output != null)
            {
                var output = JsonSerializer.Deserialize<MultipleChoiceQuestionModel>(activityData.activityData.Output.ToString(), options);
                foreach (var activityDataChoice in activityData.activityData.Choices)
                {
                    activityDataChoice.isSelected = output.Answer.Contains(activityDataChoice.answer);
                }
            }
            
            return new WorkflowNavigationViewModel
            {
                ActivityData = _mapper.Map<Activitydata>(activityData.activityData),
                WorkflowInstanceId = model.WorkflowInstanceId,
                ActivityId = activityData.ActivityId
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
