namespace He.PipelineAssessment.UI.Features.SinglePipeline.Sync
{
    public class SyncResponse
    {
        public bool IsSuccess => !ErrorMessages.Any();

        public IList<string> ErrorMessages { get; set; } = new List<string>();
    }
}
