using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddValidDataToGetAssessmentSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var sqlFiles = assembly.GetManifestResourceNames().Where(file =>
                file.Contains(
                    "20251212121754_AddValidDataToGetAssessmentSP.GetAssessments.sql") ||
                file.Contains(
                    "20251212121754_AddValidDataToGetAssessmentSP.GetEconomistAssessments.sql") ||
                file.Contains(
                    "20251212121754_AddValidDataToGetAssessmentSP.GetAssessmentInterventionListByAssessmentId.sql") ||
                file.Contains(
                    "20251212121754_AddValidDataToGetAssessmentSP.GetInterventionList.sql"));
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
            var dropGetAssessments = "DROP PROC GetAssessments";
            migrationBuilder.Sql(dropGetAssessments);

            var dropGetEconomistAssessments = "DROP PROC GetEconomistAssessments";
            migrationBuilder.Sql(dropGetEconomistAssessments);

            var dropGetInterventionListByAssessmentId = "DROP PROC GetAssessmentInterventionListByAssessmentId";
            migrationBuilder.Sql(dropGetInterventionListByAssessmentId);

            var dropGetInterventionList = "DROP PROC GetInterventionList";
            migrationBuilder.Sql(dropGetInterventionList);
        }
    }
}
