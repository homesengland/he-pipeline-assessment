using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Infrastructure.Config
{
    public class SensitiveRecordWhitelistConfiguration: IEntityTypeConfiguration<SensitiveRecordWhitelist>
    {
        public void Configure(EntityTypeBuilder<SensitiveRecordWhitelist> builder)
        {
            builder.ToTable(x => x.IsTemporal());

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.AssessmentId);

            builder.Property(x => x.Email)
                .HasMaxLength(EntityConfigurationConstants.StandardLength);

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.LastModifiedBy)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);

        }
    }
}
