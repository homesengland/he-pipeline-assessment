using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elsa.CustomInfrastructure.Migrations
{
    public partial class ExtraAssessmentQuestionFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AssessmentQuestions",
                table: "AssessmentQuestions");

            migrationBuilder.DropColumn(
                name: "FinishWorkflow",
                table: "AssessmentQuestions");

            migrationBuilder.DropColumn(
                name: "NavigateBack",
                table: "AssessmentQuestions");

            migrationBuilder.AlterColumn<string>(
                name: "WorkflowInstanceId",
                table: "AssessmentQuestions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PreviousActivityId",
                table: "AssessmentQuestions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ActivityType",
                table: "AssessmentQuestions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ActivityId",
                table: "AssessmentQuestions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AssessmentQuestions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "DuplicateKey",
                table: "AssessmentQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "ActivityName",
                table: "AssessmentQuestions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorrelationId",
                table: "AssessmentQuestions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "AssessmentQuestions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkflowDefinitionId",
                table: "AssessmentQuestions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkflowName",
                table: "AssessmentQuestions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssessmentQuestions",
                table: "AssessmentQuestions",
                column: "DuplicateKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AssessmentQuestions",
                table: "AssessmentQuestions");

            migrationBuilder.DropColumn(
                name: "DuplicateKey",
                table: "AssessmentQuestions");

            migrationBuilder.DropColumn(
                name: "ActivityName",
                table: "AssessmentQuestions");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "AssessmentQuestions");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "AssessmentQuestions");

            migrationBuilder.DropColumn(
                name: "WorkflowDefinitionId",
                table: "AssessmentQuestions");

            migrationBuilder.DropColumn(
                name: "WorkflowName",
                table: "AssessmentQuestions");

            migrationBuilder.AlterColumn<string>(
                name: "WorkflowInstanceId",
                table: "AssessmentQuestions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "PreviousActivityId",
                table: "AssessmentQuestions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AssessmentQuestions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ActivityType",
                table: "AssessmentQuestions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "ActivityId",
                table: "AssessmentQuestions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AddColumn<bool>(
                name: "FinishWorkflow",
                table: "AssessmentQuestions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NavigateBack",
                table: "AssessmentQuestions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssessmentQuestions",
                table: "AssessmentQuestions",
                column: "Id");
        }
    }
}
