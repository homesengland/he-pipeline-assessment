using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class LiknAssessmentToolToInstance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssessmentToolWorkflowId",
                table: "AssessmentToolWorkflowInstance",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentToolWorkflowInstance_AssessmentToolWorkflowId",
                table: "AssessmentToolWorkflowInstance",
                column: "AssessmentToolWorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentToolWorkflowInstance_AssessmentToolWorkflow_AssessmentToolWorkflowId",
                table: "AssessmentToolWorkflowInstance",
                column: "AssessmentToolWorkflowId",
                principalTable: "AssessmentToolWorkflow",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentToolWorkflowInstance_AssessmentToolWorkflow_AssessmentToolWorkflowId",
                table: "AssessmentToolWorkflowInstance");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentToolWorkflowInstance_AssessmentToolWorkflowId",
                table: "AssessmentToolWorkflowInstance");

            migrationBuilder.DropColumn(
                name: "AssessmentToolWorkflowId",
                table: "AssessmentToolWorkflowInstance")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentToolWorkflowInstanceHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
