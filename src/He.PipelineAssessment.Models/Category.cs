using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Models
{
    [Index(nameof(Category.CategoryName), IsUnique = true)]
    public class Category : AuditableEntity
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
