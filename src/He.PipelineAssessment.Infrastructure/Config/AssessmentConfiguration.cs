using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace He.PipelineAssessment.Infrastructure.Config
{
    public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
    {
        public void Configure(EntityTypeBuilder<Assessment> builder)
        {
            builder.ToTable(x => x.IsTemporal());

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.SpId).IsUnique();

            builder.Property(x => x.Counterparty)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.Reference)
                .HasMaxLength(EntityConfigurationConstants.StandardLength);

            builder.Property(x => x.SiteName)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.ProjectManager)
                .HasMaxLength(EntityConfigurationConstants.StandardLength);

            builder.Property(x => x.ProjectManagerEmail)
                .IsRequired(false)
                .HasMaxLength(EntityConfigurationConstants.StandardLength);

            builder.Property(x => x.Status)
                .HasMaxLength(EntityConfigurationConstants.ShortLength);

            builder.Property(x => x.LocalAuthority)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.LastModifiedBy)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.BusinessArea)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.LandType)
                .HasMaxLength(EntityConfigurationConstants.StandardLength);     
            
            builder.Property(x => x.SensitiveStatus)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.ValidData)
                .HasDefaultValue(true);

            builder.HasOne(x => x.AssessmentFund)
                .WithMany(x => x.Assessments)
                .HasForeignKey(x => x.FundId);
        }
    }
}
