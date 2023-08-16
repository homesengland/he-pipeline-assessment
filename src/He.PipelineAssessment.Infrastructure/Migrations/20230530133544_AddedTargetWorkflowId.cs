using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class AddedTargetWorkflowId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TargetAssessmentToolWorkflowId",
                table: "AssessmentIntervention",
                type: "int",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentIntervention_AssessmentToolWorkflow_TargetAssessmentToolWorkflowId",
                table: "AssessmentIntervention");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentIntervention_TargetAssessmentToolWorkflowId",
                table: "AssessmentIntervention");

            migrationBuilder.DropColumn(
                name: "TargetAssessmentToolWorkflowId",
                table: "AssessmentIntervention")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentInterventionHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
