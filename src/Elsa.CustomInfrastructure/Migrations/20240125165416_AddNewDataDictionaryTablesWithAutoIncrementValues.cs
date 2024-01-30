using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elsa.CustomInfrastructure.Migrations
{
    public partial class AddNewDataDictionaryTablesWithAutoIncrementValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                           name: "DataDictionaryGroup",
                           columns: table => new
                           {
                               Id = table.Column<int>(type: "int", nullable: false)
                                   .Annotation("SqlServer:Identity", "1, 1"),
                               Name = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                               IsArchived = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
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
                               table.PrimaryKey("PK_DataDictionaryGroup", x => x.Id);
                           })
                           .Annotation("SqlServer:IsTemporal", true)
                           .Annotation("SqlServer:TemporalHistoryTableName", "DataDictionaryGroupHistory")
                           .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                           .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                           .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "DataDictionary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataDictionaryGroupId = table.Column<int>(type: "int", nullable: false),
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
                    LastModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsArchived = table.Column<bool>(type:"bit", nullable:false, defaultValue: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataDictionary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataDictionary_DataDictionaryGroup_DataDictionaryGroupId",
                        column: x => x.DataDictionaryGroupId,
                        principalTable: "DataDictionaryGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "DataDictionaryHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateIndex(
                name: "IX_DataDictionary_DataDictionaryGroupId",
                table: "DataDictionary",
                column: "DataDictionaryGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                   name: "DataDictionary")
                   .Annotation("SqlServer:IsTemporal", true)
                   .Annotation("SqlServer:TemporalHistoryTableName", "DataDictionaryHistory")
                   .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                   .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                   .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "DataDictionaryGroup")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "DataDictionaryGroupHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }
    }
}
