using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elsa.CustomInfrastructure.Migrations
{
    public partial class ChangeWorkflowInstanceToAuditableEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "QuestionWorkflowInstance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDateTime",
                table: "QuestionWorkflowInstance",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "QuestionWorkflowInstance")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "QuestionWorkflowInstanceHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);

            migrationBuilder.DropColumn(
                name: "LastModifiedDateTime",
                table: "QuestionWorkflowInstance")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "QuestionWorkflowInstanceHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
