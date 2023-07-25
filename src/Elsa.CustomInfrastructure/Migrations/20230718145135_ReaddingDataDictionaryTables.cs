using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elsa.CustomInfrastructure.Migrations
{
    public partial class ReaddingDataDictionaryTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionDataDictionaryGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionDataDictionaryGroup", x => x.Id);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "QuestionDataDictionaryGroupHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "QuestionDataDictionary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    QuestionDataDictionaryGroupId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    LegacyName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionDataDictionary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionDataDictionary_QuestionDataDictionaryGroup_QuestionDataDictionaryGroupId",
                        column: x => x.QuestionDataDictionaryGroupId,
                        principalTable: "QuestionDataDictionaryGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "QuestionDataDictionaryHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.Sql("UPDATE Question SET QuestionDataDictionaryId = NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Question_QuestionDataDictionaryId",
                table: "Question",
                column: "QuestionDataDictionaryId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDataDictionary_QuestionDataDictionaryGroupId",
                table: "QuestionDataDictionary",
                column: "QuestionDataDictionaryGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_QuestionDataDictionary_QuestionDataDictionaryId",
                table: "Question",
                column: "QuestionDataDictionaryId",
                principalTable: "QuestionDataDictionary",
                principalColumn: "Id");

            var assembly = Assembly.GetExecutingAssembly();

            var sqlFiles = assembly.GetManifestResourceNames().Where(file =>
                file.Contains(
                    "20230718145135_ReaddingDataDictionaryTables.AddDataDictionaryGroupsAndValues.sql"));

            foreach (var sqlFile in sqlFiles)
            {
                using (Stream stream = assembly.GetManifestResourceStream(sqlFile))
                using (StreamReader reader = new StreamReader(stream))
                {
                    var sqlScript = reader.ReadToEnd();
                    migrationBuilder.Sql($"EXEC(N'{sqlScript}')");
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_QuestionDataDictionary_QuestionDataDictionaryId",
                table: "Question");

            migrationBuilder.DropTable(
                name: "QuestionDataDictionary")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "QuestionDataDictionaryHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "QuestionDataDictionaryGroup")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "QuestionDataDictionaryGroupHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropIndex(
                name: "IX_Question_QuestionDataDictionaryId",
                table: "Question");
        }
    }
}
