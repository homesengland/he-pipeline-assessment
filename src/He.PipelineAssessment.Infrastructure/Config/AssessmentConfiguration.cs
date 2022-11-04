using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Assessments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace He.PipelineAssessment.Infrastructure.Config
{
    public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
    {
        private bool _useSeedData = false;
        private AssessmentStubData _dataGenerator;
        public AssessmentConfiguration(IConfiguration config, AssessmentStubData dataGenerator)
        {
            _useSeedData = config["Data:UseSeedData"] == "true";
            _dataGenerator = dataGenerator;
        }
        public void Configure(EntityTypeBuilder<Assessment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SpId);

            builder.Property(x => x.Counterparty)
                .HasMaxLength(500);

            builder.Property(x => x.Reference)
                .HasMaxLength(100);

            builder.Property(x => x.SiteName)
                .HasMaxLength(500);

            builder.Property(x => x.ProjectManager)
                .HasMaxLength(100);

            builder.Property(x => x.ProjectManagerEmail)
                .HasMaxLength(100);

            builder.Property(x => x.Status)
                .HasMaxLength(50);

            if (_useSeedData)
            {
                //Do Stuff here.
                builder.HasData(_dataGenerator.GetAssessments());
            }
        }
    }
}
