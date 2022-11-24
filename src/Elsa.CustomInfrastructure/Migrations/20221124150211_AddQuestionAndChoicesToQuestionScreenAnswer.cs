using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elsa.CustomInfrastructure.Migrations
{
    public partial class AddQuestionAndChoicesToQuestionScreenAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Choices",
                table: "QuestionScreenAnswer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Question",
                table: "QuestionScreenAnswer",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Choices",
                table: "QuestionScreenAnswer");

            migrationBuilder.DropColumn(
                name: "Question",
                table: "QuestionScreenAnswer");
        }
    }
}
