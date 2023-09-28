using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.RegionalIPU;

namespace Elsa.CustomActivities.Activities.RegionalIPUDataSource
{
    [Action(
           Category = "Homes England Data",
           Description = "Get Regional IPU Data Source",
           Outcomes = new[] { OutcomeNames.Done }
       )]
    public class RegionalIPUDataSource : Activity
    {
        private readonly IEsriRegionalIPUClient _client;
        private readonly IEsriRegionalIPUJsonHelper _jsonHelper;
        public RegionalIPUDataSource(IEsriRegionalIPUClient client, IEsriRegionalIPUJsonHelper jsonHelper)
        {
            _client = client;
            _jsonHelper = jsonHelper;
        }

        [ActivityInput(Hint = "Region of the record to get", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string Region { get; set; } = null!;

        [ActivityInput(Hint = "Product of the record to get", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string Product { get; set; } = null!;

        [ActivityOutput] public RegionalIPUData? Output { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(Region), Region);
            context.JournalData.Add(nameof(Product), Product);

            var data = await _client.GetRegionalIPUData(Region, Product);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToRegionalIPUData(data);
                this.Output = dataResult;
            }
            else
            {
                context.JournalData.Add("Error", "Call to GetRegionalIPUData returned null");
            }

            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes("Done"),
                new SuspendResult()
            }));
        }
    }
}
