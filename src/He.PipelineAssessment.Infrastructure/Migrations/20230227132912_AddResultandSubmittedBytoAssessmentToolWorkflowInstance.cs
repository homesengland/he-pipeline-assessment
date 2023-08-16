using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class AddResultandSubmittedBytoAssessmentToolWorkflowInstance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "AssessmentToolWorkflowInstance",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubmittedBy",
                table: "AssessmentToolWorkflowInstance",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "AssessmentToolWorkflowInstance");

            migrationBuilder.DropColumn(
                name: "SubmittedBy",
                table: "AssessmentToolWorkflowInstance");
        }
    }
}
