using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elsa.Server.Data.Migrations
{
    public partial class RemoveQuestionID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionID",
                table: "MultipleChoiceQuestions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuestionID",
                table: "MultipleChoiceQuestions",
                type: "TEXT",
                nullable: true);
        }
    }
}
