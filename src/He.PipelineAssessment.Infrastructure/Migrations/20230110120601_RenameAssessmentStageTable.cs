using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class RenameAssessmentStageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentStage");

            migrationBuilder.CreateTable(
                name: "AssessmentToolWorkFlowInstance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentId = table.Column<int>(type: "int", nullable: false),
                    WorkflowName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WorkflowDefinitionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WorkflowInstanceId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CurrentActivityId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CurrentActivityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubmittedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentToolWorkFlowInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentToolWorkFlowInstance_Assessment_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentToolWorkFlowInstance_AssessmentId",
                table: "AssessmentToolWorkFlowInstance",
                column: "AssessmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentToolWorkFlowInstance");

            migrationBuilder.CreateTable(
                name: "AssessmentStage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentActivityId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CurrentActivityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubmittedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WorkflowDefinitionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WorkflowInstanceId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WorkflowName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentStage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentStage_Assessment_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentStage_AssessmentId",
                table: "AssessmentStage",
                column: "AssessmentId");
        }
    }
}
