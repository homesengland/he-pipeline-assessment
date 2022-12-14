using Elsa.CustomModels;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Elsa.CustomInfrastructure.Data
{
    public class ElsaCustomContext : DbContext, IDataProtectionKeyContext
    {
        public ElsaCustomContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AssessmentQuestion> AssessmentQuestions { get; set; } = default!;
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = default!;
    }
}
