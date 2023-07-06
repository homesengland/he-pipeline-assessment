﻿using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.PCSProfile;
using He.PipelineAssessment.Data.SinglePipeline;

namespace Elsa.CustomActivities.Activities.PCSProfileDataSource
{
    [Action(
        Category = "Homes England Data",
        Description = "Get PCS Profile Data Source",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class PCSProfileDataSource : Activity
    {
        private readonly IEsriPCSProfileClient _client;
        private readonly IEsriPCSProfileDataJsonHelper _jsonHelper;
        public PCSProfileDataSource(IEsriPCSProfileClient client, IEsriPCSProfileDataJsonHelper jsonHelper)
        {
            _client = client;
            _jsonHelper = jsonHelper;
        }

        [ActivityInput(Hint = "Id of the record to get", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string ProjectIdentified { get; set; } = null!;

        [ActivityOutput] public PCSProfileData? Output { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Suspend();
        }

        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(ProjectIdentified), ProjectIdentified);

            var data = await _client.GetPCSProfileData(ProjectIdentified);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToPCSProfileData(data);
                this.Output = dataResult;
            }
            else
            {
                context.JournalData.Add("Error", "Call to GetPCSProfileData returned null");
            }

            return await Task.FromResult(new CombinedResult(new List<IActivityExecutionResult>
            {
                Outcomes("Done"),
                new SuspendResult()
            }));
        }
    }
}
