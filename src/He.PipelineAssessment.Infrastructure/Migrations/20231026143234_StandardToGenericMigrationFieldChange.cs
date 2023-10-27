using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class StandardToGenericMigrationFieldChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsStandardVariation",
                table: "AssessmentToolWorkflow",
                newName: "IsVariation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsVariation",
                table: "AssessmentToolWorkflow",
                newName: "IsStandardVariation");
        }
    }
}
