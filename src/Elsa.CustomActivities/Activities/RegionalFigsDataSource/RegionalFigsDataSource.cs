using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.RegionalFigs;

namespace Elsa.CustomActivities.Activities.RegionalFigsDataSource
{
    [Action(
              Category = "Homes England Data",
              Description = "Get Regional Figs Data Source",
              Outcomes = new[] { OutcomeNames.Done }
          )]
    public class RegionalFigsDataSource : Activity
    {
        private readonly IEsriRegionalFigsClient _client;
        private readonly IEsriRegionalFigsJsonHelper _jsonHelper;
        public RegionalFigsDataSource(IEsriRegionalFigsClient client, IEsriRegionalFigsJsonHelper jsonHelper)
        {
            _client = client;
            _jsonHelper = jsonHelper;
        }

        [ActivityInput(Hint = "Region of the record to get", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string Region { get; set; } = null!;

        [ActivityInput(Hint = "Appraisal year of the record to get", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string AppraisalYear { get; set; } = null!;

        [ActivityOutput] public RegionalFigsData? Output { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(Region), Region);
            context.JournalData.Add(nameof(AppraisalYear), AppraisalYear);

            var data = await _client.GetRegionalFigsData(Region, AppraisalYear);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToRegionalFigsData(data);
                this.Output = dataResult;
            }
            else
            {
                context.JournalData.Add("Error", "Call to GetRegionalFigsData returned null");
            }

            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes("Done"),
                new SuspendResult()
            }));
        }
    }
}
