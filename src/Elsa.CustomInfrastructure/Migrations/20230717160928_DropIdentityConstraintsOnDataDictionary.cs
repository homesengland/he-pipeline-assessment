using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elsa.CustomInfrastructure.Migrations
{
    public partial class DropIdentityConstraintsOnDataDictionary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_QuestionDataDictionary_QuestionDataDictionaryId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionDataDictionary_QuestionDataDictionaryGroup_QuestionDataDictionaryGroupId",
                table: "QuestionDataDictionary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionDataDictionaryGroup",
                table: "QuestionDataDictionaryGroup")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "QuestionDataDictionaryGroupHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionDataDictionary",
                table: "QuestionDataDictionary")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "QuestionDataDictionaryHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropIndex(
                name: "IX_QuestionDataDictionary_QuestionDataDictionaryGroupId",
                table: "QuestionDataDictionary");

            migrationBuilder.DropIndex(
                name: "IX_Question_QuestionDataDictionaryId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "QuestionDataDictionaryGroup")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "QuestionDataDictionaryGroupHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);

            migrationBuilder.DropColumn(
                name: "Id",
                table: "QuestionDataDictionary")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "QuestionDataDictionaryHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);

            migrationBuilder.DropColumn(
                name: "QuestionDataDictionaryGroupId",
                table: "QuestionDataDictionary")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "QuestionDataDictionaryHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "QuestionDataDictionaryGroup",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "QuestionDataDictionary",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionDataDictionaryGroupId",
                table: "QuestionDataDictionary",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionDataDictionaryGroup",
                table: "QuestionDataDictionaryGroup",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionDataDictionary",
                table: "QuestionDataDictionary",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDataDictionary_QuestionDataDictionaryGroupId",
                table: "QuestionDataDictionary",
                column: "QuestionDataDictionaryGroupId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionDataDictionary_QuestionDataDictionaryGroup_QuestionDataDictionaryGroupId",
                table: "QuestionDataDictionary",
                column: "QuestionDataDictionaryGroupId",
                principalTable: "QuestionDataDictionaryGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
