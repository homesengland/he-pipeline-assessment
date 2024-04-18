using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace Elsa.CustomInfrastructure.Migrations
{
    public partial class MigrateOldDictionaryDataToNewTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var sqlFiles = assembly.GetManifestResourceNames().Where(file =>
                file.Contains(
                    "20240125180206_MigrateOldDictionaryDataToNewTables.CopyOldTableValuesToNewDataDictionary.sql"));
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

        }
    }
}
