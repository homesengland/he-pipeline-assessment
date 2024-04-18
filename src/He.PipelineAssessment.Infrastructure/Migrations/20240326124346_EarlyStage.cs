﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class EarlyStage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEarlyStage",
                table: "AssessmentToolWorkflow",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEarlyStage",
                table: "AssessmentToolWorkflow")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentToolWorkflowHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
