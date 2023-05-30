using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class AddingTargetWorkflowIdToWorkflowIntervention : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssessmentToolWorkflowId",
                table: "AssessmentIntervention",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TargetWorkflowDefinitionId",
                table: "AssessmentIntervention",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentIntervention_AssessmentToolWorkflowId",
                table: "AssessmentIntervention",
                column: "AssessmentToolWorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentIntervention_AssessmentToolWorkflow_AssessmentToolWorkflowId",
                table: "AssessmentIntervention",
                column: "AssessmentToolWorkflowId",
                principalTable: "AssessmentToolWorkflow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentIntervention_AssessmentToolWorkflow_AssessmentToolWorkflowId",
                table: "AssessmentIntervention");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentIntervention_AssessmentToolWorkflowId",
                table: "AssessmentIntervention");

            migrationBuilder.DropColumn(
                name: "AssessmentToolWorkflowId",
                table: "AssessmentIntervention")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentInterventionHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);

            migrationBuilder.DropColumn(
                name: "TargetWorkflowDefinitionId",
                table: "AssessmentIntervention")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentInterventionHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
