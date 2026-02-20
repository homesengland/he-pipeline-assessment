using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldsInGetAssessmentHistoryByAssessmentID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var sqlFiles = assembly.GetManifestResourceNames().Where(file =>
                file.Contains(
                    "20260120170652_UpdateFieldsInGetAssessmentHistoryByAssessmentID.GetAssessmentHistoryByAssessmentId.sql"));
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
            var dropGetAssessmentStagesByAssessmentId = "DROP PROC GetAssessmentHistoryByAssessmentId";
            migrationBuilder.Sql(dropGetAssessmentStagesByAssessmentId);
        }
    }
}
