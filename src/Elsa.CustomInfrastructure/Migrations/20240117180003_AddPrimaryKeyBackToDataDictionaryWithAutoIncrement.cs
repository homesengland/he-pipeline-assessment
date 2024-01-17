using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elsa.CustomInfrastructure.Migrations
{
    public partial class AddPrimaryKeyBackToDataDictionaryWithAutoIncrement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlaceholderId",
                table: "QuestionDataDictionaryGroup",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PlaceholderId",
                table: "QuestionDataDictionary",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "QuestionDataDictionaryGroup",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 0)
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "QuestionDataDictionary",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 0)
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "QuestionDataDictionaryGroup",
                newName: "PlaceholderId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "QuestionDataDictionary",
                newName: "PlaceholderId");

            migrationBuilder.AlterColumn<int>(
                name: "PlaceholderId",
                table: "QuestionDataDictionaryGroup",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("Relational:ColumnOrder", 0)
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "PlaceholderId",
                table: "QuestionDataDictionary",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("Relational:ColumnOrder", 0)
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
