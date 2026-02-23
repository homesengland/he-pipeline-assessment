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
    public class AssessmentFundConfiguration : IEntityTypeConfiguration<AssessmentFund>
    {
        public void Configure(EntityTypeBuilder<AssessmentFund> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(x => x.CreatedBy)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.LastModifiedBy)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);
        }
    }
}
