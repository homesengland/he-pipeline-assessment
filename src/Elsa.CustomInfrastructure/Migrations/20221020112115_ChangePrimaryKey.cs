using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elsa.CustomInfrastructure.Migrations
{
    public partial class ChangePrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DuplicateKey",
                table: "AssessmentQuestions",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AssessmentQuestions",
                newName: "DuplicateKey");
        }
    }
}
