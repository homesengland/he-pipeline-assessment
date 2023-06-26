using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace He.PipelineAssessment.Infrastructure.Config
{
    internal class InterventionReasonConfiguration : IEntityTypeConfiguration<InterventionReason>
    {
        public void Configure(EntityTypeBuilder<InterventionReason> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(EntityConfigurationConstants.StandardLength);

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.LastModifiedBy)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.Status)
                .HasMaxLength(EntityConfigurationConstants.ShortLength);
        }
    }
}