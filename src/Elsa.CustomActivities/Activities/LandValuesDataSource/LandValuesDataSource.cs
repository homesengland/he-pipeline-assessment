using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.LandValues.Land;
using He.PipelineAssessment.Data.LandValues.Models.Land;

namespace Elsa.CustomActivities.Activities.BilDataSource
{
    [Action(
        Category = "Homes England Data",
        Description = "Get VOA Land Value Data Source",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class LandVauesDataSource : Activity
    {
        private readonly ILandValuesClient _client;
        private readonly ILandValuesDataJsonHelper _jsonHelper;
        public LandVauesDataSource(ILandValuesClient client, ILandValuesDataJsonHelper jsonHelper)
        {
            _client = client;
            _jsonHelper = jsonHelper;
        }

        [ActivityInput(Hint = "GSS Code of the record to get", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string GssCode { get; set; } = null!;

        [ActivityOutput] public LandValues? Output { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(GssCode), GssCode);

            var data = await _client.GetLandValues(GssCode);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToLandValuesData(data);
                this.Output = dataResult;
            }
            else
            {
                context.JournalData.Add("Error", "Call to GetLaVoaLandValues returned null");
            }

            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes("Done"),
                new SuspendResult()
            }));
        }
    }
}
