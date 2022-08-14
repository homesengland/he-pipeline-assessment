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
                    QuestionID = table.Column<string>(type: "TEXT", nullable: true),
                    ActivityID = table.Column<string>(type: "TEXT", nullable: true),
                    WorkflowInstanceID = table.Column<string>(type: "TEXT", nullable: true),
                    Answer = table.Column<string>(type: "TEXT", nullable: true),
                    FinishWorkflow = table.Column<bool>(type: "INTEGER", nullable: true),
                    NavigateBack = table.Column<bool>(type: "INTEGER", nullable: true),
                    PreviousActivityId = table.Column<string>(type: "TEXT", nullable: true)
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
