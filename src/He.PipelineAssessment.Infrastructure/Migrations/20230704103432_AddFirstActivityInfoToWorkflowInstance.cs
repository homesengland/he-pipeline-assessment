using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class AddFirstActivityInfoToWorkflowInstance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstActivityId",
                table: "AssessmentToolWorkflowInstance",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstActivityType",
                table: "AssessmentToolWorkflowInstance",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            var assembly = Assembly.GetExecutingAssembly();

            var sqlFiles = assembly.GetManifestResourceNames().Where(file =>
                file.Contains(
                    "20230704103432_AddFirstActivityInfoToWorkflowInstance.GetAssessmentHistoryByAssessmentId.sql") ||
                file.Contains(
                    "20230704103432_AddFirstActivityInfoToWorkflowInstance.GetAssessmentStagesByAssessmentId.sql"));

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
            migrationBuilder.DropColumn(
                name: "FirstActivityId",
                table: "AssessmentToolWorkflowInstance")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentToolWorkflowInstanceHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);

            migrationBuilder.DropColumn(
                name: "FirstActivityType",
                table: "AssessmentToolWorkflowInstance")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentToolWorkflowInstanceHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
