using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.SinglePipeline;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Elsa.CustomActivities.Activities.SinglePipelineDataSource
{
    [Action(
        Category = "Homes England Data",
        Description = "Get Single Pipeline Data Source (IDCT)",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class SinglePipelineDataSource : Activity
    {
        private readonly IEsriSinglePipelineClient _singlePipelineClient;
        private readonly IEsriSinglePipelineDataJsonHelper _jsonHelper;
        public SinglePipelineDataSource(IEsriSinglePipelineClient singlePipelineClient, IEsriSinglePipelineDataJsonHelper jsonHelper)
        {
            _singlePipelineClient = singlePipelineClient;
            _jsonHelper = jsonHelper;
        }

        [ActivityInput(Hint = "Id of the record to get", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string SpId { get; set; } = null!;

        [ActivityOutput] public SinglePipelineData? Output { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(SpId), SpId);

            var data = await _singlePipelineClient.GetSinglePipelineData(SpId);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToSinglePipelineData(data);
                this.Output = dataResult;
            }
            else
            {
                context.JournalData.Add("Error", "Call to GetSinglePipelineData returned null");
            }

            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes("Done"),
                new SuspendResult()
            }));
        }
    }
}


