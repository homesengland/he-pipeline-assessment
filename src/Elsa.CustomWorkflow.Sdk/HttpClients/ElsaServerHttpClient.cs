using Elsa.CustomWorkflow.Sdk.Models.Currency;
using Elsa.CustomWorkflow.Sdk.Models.Date;
using Elsa.CustomWorkflow.Sdk.Models.MultipleChoice;
using Elsa.CustomWorkflow.Sdk.Models.StartWorkflow;
using Elsa.CustomWorkflow.Sdk.Models.Text;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Archive;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.LoadWorkflowActivity;
using System.Text;
using System.Text.Json;

namespace Elsa.CustomWorkflow.Sdk.HttpClients
{
    public interface IElsaServerHttpClient
    {
        Task<WorkflowNextActivityDataDto?> PostStartWorkflow(StartWorkflowCommandDto model);
        Task<WorkflowNextActivityDataDto?> SaveAndContinue(Models.MultipleChoice.SaveAndContinue.SaveAndContinueCommandDto model);
        Task<WorkflowNextActivityDataDto?> SaveAndContinue(Models.Currency.SaveAndContinue.SaveAndContinueCommandDto model);
        Task<WorkflowNextActivityDataDto?> SaveAndContinue(Models.Date.SaveAndContinue.SaveAndContinueCommandDto model);
        Task<WorkflowNextActivityDataDto?> SaveAndContinue(Models.Text.SaveAndContinue.SaveAndContinueCommandDto model);
        Task<WorkflowActivityDataDto<T>?> LoadWorkflowActivity<T>(LoadWorkflowActivityDto model);
        Task<CurrencyActivityDataDto?> LoadCurrencyWorkflowActivity(LoadWorkflowActivityDto model);
        Task<MultipleChoiceActivityDataDto?> LoadMultipleChoiceWorkflowActivity(LoadWorkflowActivityDto model);
        Task<DateActivityDataDto?> LoadDateWorkflowActivity(LoadWorkflowActivityDto model);
        Task<TextActivityDataDto?> LoadTextWorkflowActivity(LoadWorkflowActivityDto model);
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



        public async Task<WorkflowNextActivityDataDto?> SaveAndContinue(Models.MultipleChoice.SaveAndContinue.SaveAndContinueCommandDto model)
        {
            string data;
            var relativeUri = "multiple-choice/SaveAndContinue";

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

        public async Task<WorkflowNextActivityDataDto?> SaveAndContinue(Models.Currency.SaveAndContinue.SaveAndContinueCommandDto model)
        {
            string data;
            var relativeUri = "currency/SaveAndContinue";

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

        public async Task<WorkflowNextActivityDataDto?> SaveAndContinue(Models.Text.SaveAndContinue.SaveAndContinueCommandDto model)
        {
            string data;
            var relativeUri = "text/SaveAndContinue";

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

        public async Task<WorkflowNextActivityDataDto?> SaveAndContinue(Models.Date.SaveAndContinue.SaveAndContinueCommandDto model)
        {
            string data;
            var relativeUri = "date/SaveAndContinue";

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

        public async Task<WorkflowActivityDataDto <T>?> LoadWorkflowActivity<T>(LoadWorkflowActivityDto model)
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

                    return null;
                }
            }

            return JsonSerializer.Deserialize<WorkflowActivityDataDto<T>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<CurrencyActivityDataDto?> LoadCurrencyWorkflowActivity(LoadWorkflowActivityDto model)
        {
            return await LoadWorkflowActivity<CurrencyActivityDataDto?>(model, "currency");
        }
        public async Task<DateActivityDataDto?> LoadDateWorkflowActivity(LoadWorkflowActivityDto model)
        {
            return await LoadWorkflowActivity<DateActivityDataDto?>(model, "date");
        }
        public async Task<MultipleChoiceActivityDataDto?> LoadMultipleChoiceWorkflowActivity(LoadWorkflowActivityDto model)
        {
            return await LoadWorkflowActivity<MultipleChoiceActivityDataDto?>(model, "multiplechoice");
        }
        public async Task<TextActivityDataDto?> LoadTextWorkflowActivity(LoadWorkflowActivityDto model)
        {
            return await LoadWorkflowActivity<TextActivityDataDto?>(model, "text");
        }

        //public async Task<CurrencyActivityDataDto?> LoadCurrencyWorkflowActivity(LoadWorkflowActivityDto model)
        //{
        //    string data;
        //    var relativeUri = $"currency/LoadWorkflowActivity?workflowInstanceId={model.WorkflowInstanceId}&activityId={model.ActivityId}";

        //    using (var response = await _httpClientFactory.CreateClient("ElsaServerClient")
        //               .GetAsync(relativeUri)
        //               .ConfigureAwait(false))
        //    {
        //        data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            //TODO: Get this in logging
        //            //throw new ApplicationException($"StatusCode='{response.StatusCode}'," +
        //            //                               $"\n Message= '{data}'," +
        //            //                               $"\n Url='{relativeUri}'"); //TODO: Get the full url here

        //            return null;
        //        }
        //    }

        //    return JsonSerializer.Deserialize<CurrencyActivityDataDto> (data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //}

        private async Task<T?> LoadWorkflowActivity<T>(LoadWorkflowActivityDto model, string controller)
        {
            string data;
            var relativeUri = $"{controller}/LoadWorkflowActivity?workflowInstanceId={model.WorkflowInstanceId}&activityId={model.ActivityId}";

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

            return JsonSerializer.Deserialize<T>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


    }
}
