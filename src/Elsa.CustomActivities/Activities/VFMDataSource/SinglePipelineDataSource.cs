using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.VFM;

namespace Elsa.CustomActivities.Activities.VFMDataSource
{
    [Action(
        Category = "Homes England Data",
        Description = "Get VFM LA Calculations Data Source",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class VFMDataSource : Activity
    {
        private readonly IEsriVFMClient _client;
        private readonly IEsriVFMDataJsonHelper _jsonHelper;
        public VFMDataSource(IEsriVFMClient client, IEsriVFMDataJsonHelper jsonHelper)
        {
            _client = client;
            _jsonHelper = jsonHelper;
        }

        [ActivityInput(Hint = "Id of the record to get", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string SpId { get; set; } = null!;

        [ActivityOutput] public VFMCalculationData? Output { get; set; }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(SpId), SpId);

            var data = await _client.GetVFMCalculationData(SpId);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToVFMCalculationData(data);
                this.Output = dataResult;

            }
            else
            {
                context.JournalData.Add("Error", "Call to GetVFMCalculationData returned null");
                return new SuspendResult();
            }

            return Done();
        }
    }
}
