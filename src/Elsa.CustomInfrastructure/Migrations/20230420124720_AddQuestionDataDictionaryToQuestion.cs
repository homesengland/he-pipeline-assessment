using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elsa.CustomInfrastructure.Migrations
{
    public partial class AddQuestionDataDictionaryToQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionDataDictionaryId",
                table: "Question",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Question_QuestionDataDictionaryId",
                table: "Question",
                column: "QuestionDataDictionaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_QuestionDataDictionary_QuestionDataDictionaryId",
                table: "Question",
                column: "QuestionDataDictionaryId",
                principalTable: "QuestionDataDictionary",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_QuestionDataDictionary_QuestionDataDictionaryId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_QuestionDataDictionaryId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "QuestionDataDictionaryId",
                table: "Question")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "QuestionHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
