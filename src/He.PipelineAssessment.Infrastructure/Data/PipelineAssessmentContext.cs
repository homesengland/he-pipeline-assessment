using He.PipelineAssessment.Infrastructure.Config;
using He.PipelineAssessment.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
            if (userName == null)
            {
                userName = "";
            }

            foreach (var entity in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.CreatedBy = userName;
                        entity.Entity.CreatedDateTime = DateTime.UtcNow;

                        break;

                    case EntityState.Modified:
                        entity.Entity.LastModifiedBy = userName;
                        entity.Entity.LastModifiedDateTime = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);

            //this.ChangeTracker.DetectChanges();

            //var added = this.ChangeTracker.Entries()
            //            .Where(t => t.State == EntityState.Added)
            //            .Select(t => t.Entity)
            //            .ToArray();

            //foreach (var entity in added)
            //{
            //    if (entity is AuditableEntity)
            //    {
            //        var track = entity as AuditableEntity;
            //        track.CreatedDate = DateTime.Now;
            //        track.CreatedBy = UserId;
            //    }
            //}

            //var modified = this.ChangeTracker.Entries()
            //            .Where(t => t.State == EntityState.Modified)
            //            .Select(t => t.Entity)
            //            .ToArray();

            //foreach (var entity in modified)
            //{
            //    if (entity is AuditableEntity)
            //    {
            //        var track = entity as AuditableEntity;
            //        track.ModifiedDate = DateTime.Now;
            //        track.ModifiedBy = UserId;
            //    }
            //}
        }

    }
}