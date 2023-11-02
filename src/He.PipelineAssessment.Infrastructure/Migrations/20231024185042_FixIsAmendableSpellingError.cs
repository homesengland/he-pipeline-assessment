using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class FixIsAmendableSpellingError : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAmmendable",
                table: "AssessmentToolWorkflow",
                newName: "IsAmendable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAmendable",
                table: "AssessmentToolWorkflow",
                newName: "IsAmmendable");
        }
    }
}
