using He.PipelineAssessment.Infrastructure.Config;
using He.PipelineAssessment.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace He.PipelineAssessment.Infrastructure.Data
{
    public class PipelineAssessmentContext : DbContext, IDataProtectionKeyContext
    {
        private readonly IUserProvider _userProvider;
        public PipelineAssessmentContext(DbContextOptions<PipelineAssessmentContext> options, IUserProvider userProvider) : base(options)
        {
            _userProvider = userProvider;
        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = default!;
        public DbSet<Assessment> Assessment { get; set; } = default!;
        public DbSet<AssessmentTool> AssessmentTool { get; set; } = default!;
        public DbSet<AssessmentToolInstanceNextWorkflow> AssessmentToolInstanceNextWorkflow { get; set; } = default!;
        public DbSet<AssessmentToolWorkflow> AssessmentToolWorkflow { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssessmentConfiguration).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {

            var userName = _userProvider.GetUserName();

            if (userName is null)
            {
                userName ="";
            }

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = userName;
                        entry.Entity.CreatedDateTime = DateTime.UtcNow;

                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = userName;
                        entry.Entity.LastModifiedDateTime = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);

        }

    }
}