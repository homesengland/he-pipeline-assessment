using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsEarlyStageToAssessmentStagesStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var sqlFiles = assembly.GetManifestResourceNames().Where(file =>
                file.Contains(
                    "20250618152425_AddIsEarlyStageToAssessmentStagesStoredProcedure.GetAssessmentStagesByAssessmentId.sql") ||
                    file.Contains(
                    "20250618152425_AddIsEarlyStageToAssessmentStagesStoredProcedure.GetAssessmentHistoryByAssessmentId.sql") ||
                file.Contains(
                    "20250618152425_AddIsEarlyStageToAssessmentStagesStoredProcedure.GetStartableToolsByAssessmentId.sql"));
            foreach (var sqlFile in sqlFiles)
            {
                using (Stream stream = assembly.GetManifestResourceStream(sqlFile))
                using (StreamReader reader = new StreamReader(stream))
                {
                    var sqlScript = reader.ReadToEnd();
                    migrationBuilder.Sql($"EXEC(N'{sqlScript}')");
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropGetAssessmentStagesByAssessmentId = "DROP PROC GetAssessmentStagesByAssessmentId";
            migrationBuilder.Sql(dropGetAssessmentStagesByAssessmentId);
            var dropGetAssessmentStagesHistoryByAssessmentId = "DROP PROC GetAssessmentHistoryByAssessmentId";
            migrationBuilder.Sql(dropGetAssessmentStagesHistoryByAssessmentId);
            var dropGetStartableToolsByAssessmentId = "DROP PROC GetStartableToolsByAssessmentId";
            migrationBuilder.Sql(dropGetStartableToolsByAssessmentId);
        }
    }
}
