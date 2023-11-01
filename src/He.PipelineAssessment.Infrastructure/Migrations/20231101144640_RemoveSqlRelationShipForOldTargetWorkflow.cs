using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class RemoveSqlRelationShipForOldTargetWorkflow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentIntervention_AssessmentToolWorkflow_TargetAssessmentToolWorkflowId",
                table: "AssessmentIntervention");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentIntervention_TargetAssessmentToolWorkflowId",
                table: "AssessmentIntervention");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AssessmentIntervention_TargetAssessmentToolWorkflowId",
                table: "AssessmentIntervention",
                column: "TargetAssessmentToolWorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentIntervention_AssessmentToolWorkflow_TargetAssessmentToolWorkflowId",
                table: "AssessmentIntervention",
                column: "TargetAssessmentToolWorkflowId",
                principalTable: "AssessmentToolWorkflow",
                principalColumn: "Id");
        }
    }
}
