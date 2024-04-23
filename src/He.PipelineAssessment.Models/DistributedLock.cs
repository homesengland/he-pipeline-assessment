using System.ComponentModel.DataAnnotations;

namespace He.PipelineAssessment.Models
{
    public class DistributedLock
    {
        [Key]
        public string LockId { get; set; }
    }
}
