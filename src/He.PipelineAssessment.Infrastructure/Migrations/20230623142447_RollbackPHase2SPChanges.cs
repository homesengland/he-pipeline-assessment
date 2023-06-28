﻿using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    public partial class RollbackPHase2SPChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var sqlFiles = assembly.GetManifestResourceNames().Where(file =>
                file.Contains(
                    "20230623142447_RollbackPHase2SPChanges.GetAssessmentHistoryByAssessmentId.sql") ||
                file.Contains(
                    "20230623142447_RollbackPHase2SPChanges.GetAssessmentStagesByAssessmentId.sql"));
        
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
