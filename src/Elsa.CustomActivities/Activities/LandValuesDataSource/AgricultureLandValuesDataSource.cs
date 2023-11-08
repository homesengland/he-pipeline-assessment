using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.VoaLandValues.Agricultural;
using He.PipelineAssessment.Data.VoaLandValues.Models.Agriculture;

namespace Elsa.CustomActivities.Activities.BilDataSource
{
    [Action(
        Category = "Homes England Data",
        Description = "Get VOA Agriculture Land Value Data Source",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class AgricultureLandValueDataSource : Activity
    {
        private readonly IAgricultureLandValuesClient _client;
        private readonly IAgricultureLandValuesDataJsonHelper _jsonHelper;
        public AgricultureLandValueDataSource(IAgricultureLandValuesClient client, IAgricultureLandValuesDataJsonHelper jsonHelper)
        {
            _client = client;
            _jsonHelper = jsonHelper;
        }

        [ActivityInput(Hint = "Lep Area of the record you wish to retrieve", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string LepArea { get; set; } = null!;

        [ActivityOutput] public AgricultureLandValues? Output { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(LepArea), LepArea);

            var data = await _client.GetAgricultureLandValues(LepArea);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToVoaAgricultureLandValuesData(data);
                this.Output = dataResult;
            }
            else
            {
                context.JournalData.Add("Error", "Call to GetLepAgricultureVoaLandValues returned null");
            }

            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes("Done"),
                new SuspendResult()
            }));
        }
    }
}
