namespace He.PipelineAssessment.Models
{
    public class Assessment
    {
        public int Id { get; set; }
        public int SpId { get; set; }
        public string SiteName { get; set; }
        public string Counterparty { get; set; }
        public string Reference { get; set; }
        public string Status { get; set; }

    }
}