using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class RenameAuditableFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModifiedDate",
                table: "AssessmentToolWorkflowInstance",
                newName: "LastModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "AssessmentToolWorkflowInstance",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "LastModifiedDate",
                table: "AssessmentToolWorkflow",
                newName: "LastModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "AssessmentToolWorkflow",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "LastModifiedDate",
                table: "AssessmentToolInstanceNextWorkflow",
                newName: "LastModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "AssessmentToolInstanceNextWorkflow",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "LastModifiedDate",
                table: "AssessmentTool",
                newName: "LastModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "AssessmentTool",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "LastModifiedDate",
                table: "Assessment",
                newName: "LastModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Assessment",
                newName: "CreatedDateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModifiedDateTime",
                table: "AssessmentToolWorkflowInstance",
                newName: "LastModifiedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "AssessmentToolWorkflowInstance",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "LastModifiedDateTime",
                table: "AssessmentToolWorkflow",
                newName: "LastModifiedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "AssessmentToolWorkflow",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "LastModifiedDateTime",
                table: "AssessmentToolInstanceNextWorkflow",
                newName: "LastModifiedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "AssessmentToolInstanceNextWorkflow",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "LastModifiedDateTime",
                table: "AssessmentTool",
                newName: "LastModifiedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "AssessmentTool",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "LastModifiedDateTime",
                table: "Assessment",
                newName: "LastModifiedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "Assessment",
                newName: "CreatedDate");
        }
    }
}
