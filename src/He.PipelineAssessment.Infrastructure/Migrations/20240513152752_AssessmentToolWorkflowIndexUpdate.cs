using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class AssessmentToolWorkflowIndexUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssessmentToolWorkflow_WorkflowDefinitionId_AssessmentToolId_Category",
                table: "AssessmentToolWorkflow");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentToolWorkflow_WorkflowDefinitionId_AssessmentToolId_Category_Status",
                table: "AssessmentToolWorkflow",
                columns: new[] { "WorkflowDefinitionId", "AssessmentToolId", "Category", "Status" },
                unique: true,
                filter: "[Status] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssessmentToolWorkflow_WorkflowDefinitionId_AssessmentToolId_Category_Status",
                table: "AssessmentToolWorkflow");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentToolWorkflow_WorkflowDefinitionId_AssessmentToolId_Category",
                table: "AssessmentToolWorkflow",
                columns: new[] { "WorkflowDefinitionId", "AssessmentToolId", "Category" },
                unique: true);
        }
    }
}
