using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elsa.Server.Data.Migrations
{
    public partial class UpdateIdFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkflowInstanceID",
                table: "MultipleChoiceQuestions",
                newName: "WorkflowInstanceId");

            migrationBuilder.RenameColumn(
                name: "ActivityID",
                table: "MultipleChoiceQuestions",
                newName: "ActivityId");

            migrationBuilder.AlterColumn<string>(
                name: "WorkflowInstanceId",
                table: "MultipleChoiceQuestions",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PreviousActivityId",
                table: "MultipleChoiceQuestions",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ActivityId",
                table: "MultipleChoiceQuestions",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkflowInstanceId",
                table: "MultipleChoiceQuestions",
                newName: "WorkflowInstanceID");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "MultipleChoiceQuestions",
                newName: "ActivityID");

            migrationBuilder.AlterColumn<string>(
                name: "WorkflowInstanceID",
                table: "MultipleChoiceQuestions",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "PreviousActivityId",
                table: "MultipleChoiceQuestions",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ActivityID",
                table: "MultipleChoiceQuestions",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
