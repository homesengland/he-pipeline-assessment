using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingSearchFieldsToInterventionList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var sqlFiles = assembly.GetManifestResourceNames().Where(file =>
                file.Contains(
                    "20260227112015_AddingSearchFieldsToInterventionList.GetInterventionList.sql"));
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
            var queryToDropInterventionListProc = "DROP PROC GetInterventionList";
            migrationBuilder.Sql(queryToDropInterventionListProc);
        }
    }
}
