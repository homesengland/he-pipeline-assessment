using Elsa.CustomInfrastructure.Config;
using Elsa.CustomModels;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Elsa.CustomInfrastructure.Data
{
    public class ElsaCustomContext : DbContext, IDataProtectionKeyContext
    {
        public ElsaCustomContext(DbContextOptions options) : base(options) { }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomActivityNavigationConfig).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDateTime = DateTime.UtcNow;

                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedDateTime = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);

        }
    }
}
