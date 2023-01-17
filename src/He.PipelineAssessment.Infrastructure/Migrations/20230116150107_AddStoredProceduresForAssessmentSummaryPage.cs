using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class AddStoredProceduresForAssessmentSummaryPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var sqlFiles = assembly.GetManifestResourceNames().Where(file =>
                file.Contains(
                    "20230116150107_AddStoredProceduresForAssessmentSummaryPage.GetAssessmentStagesByAssessmentId.sql") ||
                file.Contains(
                    "20230116150107_AddStoredProceduresForAssessmentSummaryPage.GetStartableToolsByAssessmentId.sql"));
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropRole = "DROP ROLE db_executor";
            migrationBuilder.Sql(dropRole);
        }
    }
}
