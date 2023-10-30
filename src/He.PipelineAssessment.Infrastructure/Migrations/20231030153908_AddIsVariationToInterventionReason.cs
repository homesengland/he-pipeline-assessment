using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class AddIsVariationToInterventionReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVariation",
                table: "InterventionReason",
                type: "bit",
                nullable: false,
                defaultValue: false);

            var assembly = Assembly.GetExecutingAssembly();
            var sqlFiles = assembly.GetManifestResourceNames().Where(file =>

                file.Contains(
                    "20231030153908_AddIsVariationToInterventionReason.PopulateInterventionReasonForVariations.sql")
            );
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
                name: "IsVariation",
                table: "InterventionReason")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "InterventionReasonHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
