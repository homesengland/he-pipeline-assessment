using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class AddTartgetAssessmentToolWorkflowTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TargetAssessmentToolWorkflow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentInterventionId = table.Column<int>(type: "int", nullable: false),
                    AssessmentToolWorkflowId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetAssessmentToolWorkflow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TargetAssessmentToolWorkflow_AssessmentIntervention_AssessmentInterventionId",
                        column: x => x.AssessmentInterventionId,
                        principalTable: "AssessmentIntervention",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TargetAssessmentToolWorkflow_AssessmentToolWorkflow_AssessmentToolWorkflowId",
                        column: x => x.AssessmentToolWorkflowId,
                        principalTable: "AssessmentToolWorkflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TargetAssessmentToolWorkflow_AssessmentInterventionId",
                table: "TargetAssessmentToolWorkflow",
                column: "AssessmentInterventionId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetAssessmentToolWorkflow_AssessmentToolWorkflowId",
                table: "TargetAssessmentToolWorkflow",
                column: "AssessmentToolWorkflowId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TargetAssessmentToolWorkflow");
        }
    }
}
