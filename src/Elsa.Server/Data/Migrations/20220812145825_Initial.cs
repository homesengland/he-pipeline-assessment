using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elsa.Server.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MultipleChoiceQuestions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    QuestionID = table.Column<string>(type: "TEXT", nullable: false),
                    WorkflowInstanceID = table.Column<string>(type: "TEXT", nullable: false),
                    Answer = table.Column<string>(type: "TEXT", nullable: false),
                    FinishWorkflow = table.Column<bool>(type: "INTEGER", nullable: false),
                    NavigateBack = table.Column<bool>(type: "INTEGER", nullable: false),
                    PreviousActivityId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoiceQuestions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MultipleChoiceQuestions");
        }
    }
}
