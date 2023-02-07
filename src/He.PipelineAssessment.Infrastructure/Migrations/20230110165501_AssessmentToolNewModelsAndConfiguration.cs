using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class AssessmentToolNewModelsAndConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentToolWorkFlowInstance_Assessment_AssessmentId",
                table: "AssessmentToolWorkFlowInstance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssessmentToolWorkFlowInstance",
                table: "AssessmentToolWorkFlowInstance");

            migrationBuilder.RenameTable(
                name: "AssessmentToolWorkFlowInstance",
                newName: "AssessmentToolWorkflowInstance");

            migrationBuilder.RenameIndex(
                name: "IX_AssessmentToolWorkFlowInstance_AssessmentId",
                table: "AssessmentToolWorkflowInstance",
                newName: "IX_AssessmentToolWorkflowInstance_AssessmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssessmentToolWorkflowInstance",
                table: "AssessmentToolWorkflowInstance",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AssessmentTool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentTool", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentToolInstanceNextWorkflow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentToolWorkflowInstanceId = table.Column<int>(type: "int", nullable: false),
                    NextWorkflowDefinitionId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    IsStarted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentToolInstanceNextWorkflow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentToolInstanceNextWorkflow_AssessmentToolWorkflowInstance_AssessmentToolWorkflowInstanceId",
                        column: x => x.AssessmentToolWorkflowInstanceId,
                        principalTable: "AssessmentToolWorkflowInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentToolWorkflow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentToolId = table.Column<int>(type: "int", nullable: false),
                    WorkflowDefinitionId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    IsFirstWorkflow = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    IsLatest = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentToolWorkflow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentToolWorkflow_AssessmentTool_AssessmentToolId",
                        column: x => x.AssessmentToolId,
                        principalTable: "AssessmentTool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentToolInstanceNextWorkflow_AssessmentToolWorkflowInstanceId",
                table: "AssessmentToolInstanceNextWorkflow",
                column: "AssessmentToolWorkflowInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentToolWorkflow_AssessmentToolId",
                table: "AssessmentToolWorkflow",
                column: "AssessmentToolId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentToolWorkflowInstance_Assessment_AssessmentId",
                table: "AssessmentToolWorkflowInstance",
                column: "AssessmentId",
                principalTable: "Assessment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentToolWorkflowInstance_Assessment_AssessmentId",
                table: "AssessmentToolWorkflowInstance");

            migrationBuilder.DropTable(
                name: "AssessmentToolInstanceNextWorkflow");

            migrationBuilder.DropTable(
                name: "AssessmentToolWorkflow");

            migrationBuilder.DropTable(
                name: "AssessmentTool");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssessmentToolWorkflowInstance",
                table: "AssessmentToolWorkflowInstance");

            migrationBuilder.RenameTable(
                name: "AssessmentToolWorkflowInstance",
                newName: "AssessmentToolWorkFlowInstance");

            migrationBuilder.RenameIndex(
                name: "IX_AssessmentToolWorkflowInstance_AssessmentId",
                table: "AssessmentToolWorkFlowInstance",
                newName: "IX_AssessmentToolWorkFlowInstance_AssessmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssessmentToolWorkFlowInstance",
                table: "AssessmentToolWorkFlowInstance",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentToolWorkFlowInstance_Assessment_AssessmentId",
                table: "AssessmentToolWorkFlowInstance",
                column: "AssessmentId",
                principalTable: "Assessment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
