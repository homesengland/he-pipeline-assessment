using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.SinglePipeline;
using System.Text.Json;

namespace Elsa.CustomActivities.Activities.SinglePipelineDataActivity
{
    [Action(
        Category = "Homes England Data",
        Description = "Get Single Pipeline Data Source",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class SinglePipelineDataSource : Activity
    {
        private readonly IEsriSinglePipelineClient _singlePipelineClient;
        public SinglePipelineDataSource(IEsriSinglePipelineClient singlePipelineClient)
        {
            _singlePipelineClient = singlePipelineClient;
        }

        [ActivityInput(Hint = "Id of the record to get", SupportedSyntaxes = new[] { SyntaxNames.Literal })]
        public string? SpId { get; set; }

        [ActivityOutput] public SinglePipelineData? Output { get; set; }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            var x = this.SpId;

            //call the http client
            var data = await _singlePipelineClient.GetSinglePipelineData(this.SpId);
            var result = JsonSerializer.Deserialize<EsriSinglePipelineResponse>(data);
            var dataResult = result.features.FirstOrDefault().attributes;

            this.Output = dataResult;
            context.JournalData.Add(nameof(SpId), x);


            return Done();
        }
    }
}
