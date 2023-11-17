using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.ExtendedSinglePipeline;

namespace Elsa.CustomActivities.Activities.SinglePipelineExtendedDataSource
{
    [Action(
        Category = "Homes England Data",
        Description = "Get Single Pipeline Extended Data Source",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class SinglePipelineExtendedDataSource : Activity
    {
        private readonly IEsriSinglePipelineExtendedClient _singlePipelineExtendedClient;
        private readonly IEsriSinglePipelineExtendedDataJsonHelper _jsonHelper;
        public SinglePipelineExtendedDataSource(IEsriSinglePipelineExtendedClient singlePipelineClient, IEsriSinglePipelineExtendedDataJsonHelper jsonHelper)
        {
            _singlePipelineExtendedClient = singlePipelineClient;
            _jsonHelper = jsonHelper;
        }

        [ActivityInput(Hint = "Id of the record to get", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string SpId { get; set; } = null!;

        [ActivityOutput] public SinglePipelineExtendedData? Output { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(SpId), SpId);

            var data = await _singlePipelineExtendedClient.GetSinglePipelineData(SpId);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToSinglePipelineData(data);
                this.Output = dataResult;
            }
            else
            {
                context.JournalData.Add("Error", "Call to GetSinglePipelineExtendedData returned null");
            }

            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes("Done"),
                new SuspendResult()
            }));
        }
    }
}


