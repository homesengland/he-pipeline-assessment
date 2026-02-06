namespace He.PipelineAssessment.Models.ViewModels
{
    public class SyncModel
    {
        public int UpdatedAssessmentCount { get; set; }
        public int NewAssessmentCount { get; set;}

        public int SetToInvalidAssessmentCount { get; set; }
        public bool Synced { get; set; } = false;
    }
}
